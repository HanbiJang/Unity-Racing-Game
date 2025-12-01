using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class RetryGame : MonoBehaviour, IClientAction
{
    //현재 사용 X
    public void Do(byte[] byteData)
    {
        Debug.Log("RetryGame()");

        RetryData data = new RetryData();
        data.ConvertToGameData(byteData);
        Debug.Log("userID " + data.UserID + "BodyLength " + data.BodyLength);

        
    }
}
