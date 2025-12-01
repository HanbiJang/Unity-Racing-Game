using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Judgement : MonoBehaviour, IClientAction
{
    //현재 사용 X, 
    public void Do(byte[] byteData)
    {
        Debug.Log("Judgement()");

        JudgementData judgementData = new JudgementData();
        judgementData.ConvertToGameData(byteData);
        Debug.Log("userID " + judgementData.userID + "NodeType " + judgementData.NodeType);

    }
}