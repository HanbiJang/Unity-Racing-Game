using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEditor;
using UnityEngine;

public class TestJoinButton : MonoBehaviour
{
    [SerializeField]
    UInt64 userID = 22;

    [SerializeField]
    UInt64 roomID = 33;

    public void JoinTest()
    {
        Debug.Log("Btn Clicked");
        JoinGameData joinGameData = new JoinGameData(userID, roomID);

        ServerInterface.Instance.SendDataToServer(ServerInterface.Instance.SocketConnection, joinGameData, (int)EPacketID.JoinGame);

    }
}
