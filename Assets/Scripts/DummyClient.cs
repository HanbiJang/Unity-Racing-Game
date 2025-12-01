using System.Collections;
using UnityEngine;

public class DummyClient : MonoBehaviour
{
    ReadyGameData testData;

    public void Start()
    {
        //test Packet        
        ServerInterface.Instance.ConnectToTcpServer("127.0.0.1", 8888);
        StartCoroutine(Start5s());

    }

    IEnumerator Start5s()
    {

        while (true)
        {
            ServerInterface.Instance.ReceiveDataFromServer(ServerInterface.Instance.SocketConnection);
           
            yield return new WaitForSeconds(0.01f);
        }

    }

    void Update()
    {

    }

}