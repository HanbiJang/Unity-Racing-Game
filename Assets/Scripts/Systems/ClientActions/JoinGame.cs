using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class JoinGame : MonoBehaviour, IClientAction
{
    
    public void Do(byte[] byteData)
    {
        Debug.Log("JoinGame()");

        JoinGameData data = new JoinGameData();
        data.ConvertToGameData(byteData);

        //userId, roomID를 GameState에 저장함
        Debug.Log("userID " + data.UserID + "roomID " + data.RoomID);
        GameState.Instance.UserId = data.UserID;
        GameState.Instance.RoomId = data.RoomID;

        //클라이언트는 매칭된 상태, 게임 씬을 비동기 로딩하고 (Ready 패킷을 보내며) 대기함
        ServerInterface.Instance.AsyncLoadAndReady(1);
        GameModeManager.instance.GameStart();
    }
}
