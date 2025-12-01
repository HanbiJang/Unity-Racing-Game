using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;

public class testerror : MonoBehaviour
{

    private Thread tcpListenerThread;
    TcpClient tm;

    // Start TcpServer background thread

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (ServerInterface.Instance != null) Debug.Log("?");

            tcpListenerThread = new Thread(new ThreadStart(none));
            tcpListenerThread.IsBackground = true;
            tcpListenerThread.Start();


        }
    }

    void none() 
    {
        //TcpClient tm = new TcpClient(ip, 8888);
        Debug.Log("none");
    }
}


