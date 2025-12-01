using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreBroadcast : MonoBehaviour, IClientAction
{
    //서버에서 점수 데이터를 전달 후, 클라이언트가 하는 일
    public void Do(byte[] byteData)
    {
        Debug.Log("ScoreBroadcast()");

        //data save
        ScoreBroadcastData data = new ScoreBroadcastData();
        data.ConvertToGameData(byteData);
        Debug.Log("UserCount " + data.UserCount + "ScoreLIst ... Count");

        GameState.Instance.UserCount = data.UserCount;
        GameState.Instance.ScoreLIst = data.ScoreLIst;


        //GameState에 저장된 데이터를 활용
        //...

    }
}
