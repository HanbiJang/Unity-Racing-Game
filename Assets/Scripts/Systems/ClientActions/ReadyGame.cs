using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class ReadyGame : MonoBehaviour, IClientAction
{
    //현재 사용 X
    public void Do(byte[] byteData)
    {
        Debug.Log("ReadyGame()");

        ReadyGameData readyGameData = new ReadyGameData();
        readyGameData.ConvertToGameData(byteData);
        Debug.Log("userID " + readyGameData.userID + "roomID " + readyGameData.roomID);       
    }
}
