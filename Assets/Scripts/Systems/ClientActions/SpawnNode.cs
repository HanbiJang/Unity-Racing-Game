using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnNode : MonoBehaviour, IClientAction
{
    //서버에서 준 노드 데이터를 기반으로 노드를 스폰하는 코드
    public void Do(byte[] byteData)
    {
        Debug.Log("SpawnNode()");

        //데이터 저장
        SpawnNodeData data = new SpawnNodeData();
        data.ConvertToGameData(byteData);
        Debug.Log("NodeType " + data.NodeType + "NodePos " + data.NodePos);


        //위 data 활용
        //GameModeManager.instance.m_ObjectSpawner.SpawnNodeCentre();

        NodeSpwaner ns = GameObject.FindWithTag("EditorOnly").GetComponent<ProxyScript>().proxy.GetComponent<NodeSpwaner>();
        if (ns == null)
            Debug.Log("rotqlfk");
        else
            ns.SpawnNodeCentre();
        //...           
    }
}
