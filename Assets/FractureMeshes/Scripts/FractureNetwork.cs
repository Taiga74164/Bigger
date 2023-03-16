using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using UnityEngine;

public class FractureNetwork : MonoBehaviour
{

    #region Params
    int _nodeCount;
    public List<FractureNetworkNode> network;
    List<FractureNetworkNode> foundation;
    bool isCollapsing;
    bool hasCollapsed = false;

    [SerializeField]
    [Tooltip("Time in seconds after a piece of rubble collides with the ground and when it is culled.")]
    private float rubbleCullDelay = 15.0f;

    //Async
    CancellationTokenSource cancellationTokenSource;

    [Header("Debug")]
    public bool DebugNetwork = false;
    public bool DebugFoundation = false;
    #endregion

    private void Start()
    {
        InitializeFractureNetwork();
        cancellationTokenSource = new CancellationTokenSource();
    }

    private void Update()
    {
        DrawDebug();
    }

    #region Initialize Network
    /*
     * Generate array of FractureNetworkNodes which will be used as the basis for pathfinding.
     */
    public void InitializeFractureNetwork()
    {
        network = new List<FractureNetworkNode>();  
        _nodeCount = transform.childCount;
        foundation = new List<FractureNetworkNode>();

        Dictionary<SubFracture, FractureNetworkNode> nodeDict = new Dictionary<SubFracture, FractureNetworkNode>();


        //create array of subfracture node class
        for (int i = 0; i < _nodeCount; i++)
        {
            SubFracture sf = transform.GetChild(i).GetComponent<SubFracture>();

            FractureNetworkNode currentNode = new FractureNetworkNode(sf, sf.transform.position);

            nodeDict.Add(sf, currentNode);
            network.Add(currentNode);
        }

        for (int i = 0; i < _nodeCount; i++)
        {
            GameObject cell = transform.GetChild(i).gameObject;

            //add temporary box collider
            //box collider will automatically scale to bounds of submesh
            //this box collider will be used to find neighboring meshes
            //neighboring meshes will be added to the Connections of _subfracture 
            MeshRenderer meshRenderer = cell.GetComponent<MeshRenderer>();
            Collider[] hitColliders = Physics.OverlapBox(meshRenderer.bounds.center, meshRenderer.bounds.size / 2, cell.transform.rotation);

            foreach (Collider c in hitColliders)
            {
                //check for other subfracture meshes in fractured mesh
                //must have subfracture component
                //must be a child of the same parent(part of the same fractured mesh)
                //must not be a submesh of the current cell (i.e, a separate fragment of the fractured mesh)
                if (c.gameObject.GetComponent<SubFracture>() && c.gameObject != cell)
                {
                    nodeDict[cell.GetComponent<SubFracture>()].neighbours.Add(nodeDict[c.gameObject.GetComponent<SubFracture>()]);
                    nodeDict[c.gameObject.GetComponent<SubFracture>()].neighbours.Add(nodeDict[cell.GetComponent<SubFracture>()]);
                }
            }

            //determine which blocks are touching the ground
            int layerMask = 1 << 9;
            RaycastHit _hit;
            if (Physics.Raycast(cell.GetComponent<MeshRenderer>().bounds.center, Vector3.down, out _hit, cell.GetComponent<MeshRenderer>().bounds.size.y / 2f, layerMask))
            {

                if (_hit.collider.gameObject.tag == "Ground")
                {
                    nodeDict[cell.GetComponent<SubFracture>()].isFoundation = true;
                    foundation.Add(nodeDict[cell.GetComponent<SubFracture>()]);
                }
            }

            //get closest foundation piece to each node and store reference to that piece
            //will be used for determining structural integrity
            foreach (FractureNetworkNode node in network)
            {
                float minDistance = Mathf.Infinity;
                FractureNetworkNode closestFoundation = null;
                foreach (FractureNetworkNode f in foundation)
                {
                    if(Vector3.Distance(f.positionWS, node.positionWS) < minDistance)
                    {
                        minDistance = Vector3.Distance(f.positionWS, node.positionWS);
                        closestFoundation = f;
                    }

                }
                node.foundationTarget = closestFoundation;


            }


            //store reference to network node on Subfracture monobehaviour
            foreach (FractureNetworkNode node in network)
            {

                node.subFracture._node = node;
                node.subFracture._cullDelay = rubbleCullDelay;
                if (node.isFoundation)
                {
                    node.subFracture.GetComponent<MeshRenderer>().material.color = Color.green;
                }
            }

        }
    }
    #endregion
   
    #region Pathfinding
    /*
     * Loop through remaining nodes in network and pathfind to the foundations
     * If the node does not connect to a foundation, break it
     * Return true if a path is found, false if no path exists
     * modified A*
     */
    public bool PathTo(FractureNetworkNode startNode, FractureNetworkNode endNode)
    {
        Heap<FractureNetworkNode> openSet = new Heap<FractureNetworkNode> (_nodeCount);
        HashSet<FractureNetworkNode> closedSet = new HashSet<FractureNetworkNode>();

        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            FractureNetworkNode currentNode = openSet.RemoveFirst();

            closedSet.Add(currentNode);

            if (currentNode.isFoundation || currentNode == endNode) //path to foudnation found
            {
                return true;
            }

            foreach(FractureNetworkNode neighbour in currentNode.neighbours)
            {
                if (neighbour.isBroken || closedSet.Contains(neighbour) ||!network.Contains(neighbour))
                {
                    continue;
                }

                float newCostToNeighbour = currentNode.gCost + HeightDistance(currentNode, neighbour);

                if(newCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    neighbour.gCost = newCostToNeighbour;
                    neighbour.hCost = HeightDistance(neighbour, endNode);

                    if (!openSet.Contains(neighbour))
                    {
                        openSet.Add(neighbour);
                    }

                }
                
            }
        }
        return false;
    }
    
    /*
     * Calculate the difference in height between two nodes
     * When determining if a node is grounded, we want to find the fastest path downwards
     */

    float HeightDistance(FractureNetworkNode nodeA, FractureNetworkNode nodeB) 
    {
        return (nodeA.positionWS.y - nodeB.positionWS.y)*-1;
    } 


    #endregion

    public async void StartCollapse()
    {
        if (!isCollapsing)
        {
            Debug.Log("building has started collapsing");
            isCollapsing = true;
            while (isCollapsing)
            {
                await Task.Run(() => BreakFloatingNodes(), cancellationTokenSource.Token);
                if (hasCollapsed)
                {
                    cancellationTokenSource.Cancel();
                    break;
                }
            }
            cancellationTokenSource.Cancel();
            cancellationTokenSource.Dispose();
        }

        //terminate async task
    }

    #region Debug
    private void DrawDebug()
    {
        if (DebugNetwork)
        {
            foreach (FractureNetworkNode node in network)
            {
                foreach (FractureNetworkNode neighbour in node.neighbours)
                {
                    Debug.DrawLine(node.positionWS, neighbour.positionWS, Color.yellow);
                }
            }
        }
        
        if (DebugFoundation)
        {
            foreach (FractureNetworkNode node in network)
            {
                Debug.DrawLine(node.positionWS, node.foundationTarget.positionWS, Color.red);
            }
        }
    }
    #endregion

    public void BreakFloatingNodes()
    {
        if(hasCollapsed)
        {
            Debug.Log("async task cancelled");
            cancellationTokenSource.Cancel();
        }

        foreach (FractureNetworkNode node in network)
        {
            if (node.isBroken) continue; //ignore broken nodes
            if (PathTo(node, node.foundationTarget)) continue; //if connected to foundation continue   
            node.isBroken = true; //set all that aren't already broken or connected to the foundation to true     
        }

        //check if whole building has collapsed
        hasCollapsed = true;
        foreach (FractureNetworkNode node in network)
        {
            if (!node.isBroken && !node.isFoundation) //if there is an unbroken part of the mesh and that node is not a foundation piece
            {
                hasCollapsed = false;
                break;
            }
        }
    }

    private void OnApplicationQuit()
    {
        cancellationTokenSource?.Cancel();
        cancellationTokenSource?.Dispose();
    }
}
