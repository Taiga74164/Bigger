using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FractureNetworkNode : IHeapItem<FractureNetworkNode>
{
    #region Pathfinding Properties

    public float gCost;
    public float hCost;
    public float fCost
    {
        get
        {
            return gCost + hCost;
        }
    }

    public HashSet<FractureNetworkNode> neighbours;

    #endregion

    #region Fracture Properties

    public SubFracture subFracture;
    public Vector3 positionWS;
    public bool isFoundation;
    public FractureNetworkNode foundationTarget;
    public bool isBroken;
    #endregion

    #region HeapItem Interface
    int heapIndex;
    //implement heapitem interface
    public int HeapIndex
    {
        get
        {
            return heapIndex;
        }
        set
        {
            heapIndex = value;
        }
    }
    #endregion

    #region CompareTo Interface
    public int CompareTo(FractureNetworkNode nodeToCompare)
    {
        int compare = fCost.CompareTo(nodeToCompare.fCost);
        if(compare == 0)
        {
            compare = hCost.CompareTo(nodeToCompare.hCost);
        }
        return -compare;
    }

    #endregion

    #region Constructor
    /*
     * Constructor
     */
    public FractureNetworkNode(SubFracture _subFracture, Vector3 _position)
    {
        subFracture = _subFracture;
        positionWS = _position;
        neighbours = new HashSet<FractureNetworkNode>();
    }

    #endregion


}
