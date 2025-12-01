using System;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class DummyServer : MonoBehaviour
{
    #region private members
    private TcpListener tcpListener;
    private Thread tcpListenerThread;
    private TcpClient connectedTcpClient;
    #endregion

    void Awake()
    {
        Debug.Log("Start Server");

        // Start TcpServer background thread
        tcpListenerThread = new Thread(new ThreadStart(ListenForIncommingRequest));
        tcpListenerThread.IsBackground = true;
        tcpListenerThread.Start();
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Runs in background TcpServerThread; Handles incomming TcpClient requests
    private void ListenForIncommingRequest()
    {
        try
        {
            tcpListener = new TcpListener(IPAddress.Parse("127.0.0.1"), 8888);
            tcpListener.Start();
            Debug.Log("Server is listening");

            while (true)
            {
                using (connectedTcpClient = tcpListener.AcceptTcpClient())
                {
                    // Get a stream object for reading
                    using (NetworkStream stream = connectedTcpClient.GetStream())
                    {
                        int bytes = 0;
                        // Read incomming stream into byte array.
                        do
                        {
                            // TODO
                            byte[] buffer = new byte[4096];

                            bytes = stream.Read(buffer, 0, buffer.Length); //Thread was being aborted.

                            //받은 데이터 다시 클라에게 보내기
                            byte[] b1h = new byte[8];
                            byte[] b2h = new byte[8];
                            byte[] b3h = new byte[8];

                            byte[] b1 = new byte[4];
                            byte[] b2 = new byte[10];
                            byte[] b3 = new byte[15];

                            b1h[0] = (byte)BitConverter.ToInt32(buffer, 0);
                            b1h[4] = (byte)b1.Length;

                            b2h[0] = (byte)BitConverter.ToInt32(buffer, 0);
                            b2h[4] = (byte)b2.Length;

                            b3h[0] = (byte)BitConverter.ToInt32(buffer, 0);
                            b3h[4] = (byte)b3.Length;

                            Array.Copy(buffer, Defines.HEADERSIZE, b1, 0, b1.Length);
                            Array.Copy(buffer, Defines.HEADERSIZE + b1.Length, b2, 0, b2.Length);
                            Array.Copy(buffer, Defines.HEADERSIZE + b1.Length + b2.Length, b3, 0, b3.Length);

                            // [1] 쪼개서 보내기  
                            byte[] b1packet = new byte[8 + b1.Length];
                            byte[] b2packet = new byte[8 + b2.Length];
                            byte[] b3packet = new byte[8 + b3.Length];

                            Array.Copy(b1h, 0, b1packet, 0, b1h.Length);
                            Array.Copy(b1, 0, b1packet, b1h.Length, b1.Length);

                            Array.Copy(b2h, 0, b2packet, 0, b2h.Length);
                            Array.Copy(b2, 0, b2packet, b2h.Length, b2.Length);

                            Array.Copy(b3h, 0, b3packet, 0, b3h.Length);
                            Array.Copy(b3, 0, b3packet, b3h.Length, b3.Length);

/*                            Debug.Log("Server data 1/3 send");
                            stream.Write(b1packet, 0, b1packet.Length);

                            Debug.Log("Server data 2/3 send");
                            stream.Write(b2packet, 0, b2packet.Length);

                            Debug.Log("Server data 3/3 send");
                            stream.Write(b3packet, 0, b3packet.Length);*/

                            // [2] 한꺼번에 보내기
                            //stream.Write(buffer,0,16); 


                        } while (bytes > 0);
                    }
                }
            }
        }
        catch (SocketException socketException)
        {
            Debug.Log("SocketException " + socketException.ToString());
        }
    }
}