using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGame : MonoBehaviour, IClientAction
{
    /// <summary>
    /// 해당 방을 탈출하면서 종료한다
    /// </summary>
    /// <param name="byteData"></param>
    public void Do(byte[] byteData)
    {
        Debug.Log("EndGame()");

        //해당 방의 데이터
        EndGameData endGameData = new EndGameData();
        endGameData.ConvertToGameData(byteData);
        Debug.Log("userID " + endGameData.UserID + "roomID " + endGameData.RoomID);

        //서버와의 연결을 종료한다
        ServerInterface.Instance.DisconnectToTcpServer();
        Application.Quit(); //게임 종료
    }
}
