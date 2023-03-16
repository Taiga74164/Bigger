using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System.Threading;

public class AsyncTaskTest : MonoBehaviour
{

    CancellationTokenSource cancellationTokenSource;
    // Start is called before the first frame update
    void Start()
    {
        //CountToOneMillion();
        CountToOneMillionAsync();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    async void CountToOneMillionAsync()
    {
        int number = 0;
        cancellationTokenSource = new CancellationTokenSource();
        while (number < 1000000)
        {
            number++;
            Debug.Log(number);
            await Task.Delay(2, cancellationTokenSource.Token);
        }
        Debug.Log("i've finished counting!");
    }

    void CountToOneMillion()
    {
        int number = 0;
        while (number < 1000000)
        {
            number++;
            Debug.Log(number);
        }
        Debug.Log("i've finished counting!");
    }

    private void OnApplicationQuit()
    {
        cancellationTokenSource?.Dispose();
        cancellationTokenSource = null;
    }
}
