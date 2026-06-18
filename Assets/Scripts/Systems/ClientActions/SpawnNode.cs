using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnNode : MonoBehaviour, IClientAction
{
    // 판정을 Z거리 기반으로만 처리하므로 시간 기반 예상 타이밍 계산 미사용
    // private float CalculateExpectedTime(GameObject node, int nodeTimeMs) { ... }
    // 중복 패킷 필터링용 (NodeTimeMs 기준)
    private static readonly System.Collections.Generic.HashSet<int> s_ProcessedNodeTimes
        = new System.Collections.Generic.HashSet<int>();

    public static void ClearProcessedNodes() => s_ProcessedNodeTimes.Clear();


    //서버에서 준 노드 데이터를 기반으로 노드를 스폰하는 코드
    public void Do(byte[] byteData)
    {
        //데이터 저장
        SpawnNodeData data = new SpawnNodeData();
        data.ConvertToGameData(byteData);

        // 동일 NodeTimeMs 패킷 중복 차단
        if (!s_ProcessedNodeTimes.Add(data.NodeTimeMs))
        {
            Debug.LogWarning($"[SpawnNode] 중복 패킷 무시 — NodeTimeMs={data.NodeTimeMs}");
            return;
        }
        // Debug.Log("NodeType " + data.NodeType + "NodePos " + data.NodePos);
        
        // #region agent log
        try {
            string logEntry = $"{{\"sessionId\":\"debug-session\",\"runId\":\"run1\",\"hypothesisId\":\"H1\",\"location\":\"SpawnNode.cs:Do\",\"message\":\"Received SpawnNodeData from server\",\"data\":{{\"nodeType\":{data.NodeType},\"nodePos\":{data.NodePos},\"nodeTimeMs\":{data.NodeTimeMs}}},\"timestamp\":{System.DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}}}\n";
            System.IO.File.AppendAllText(@"d:\GitRepo\Unity Racing Game\.cursor\debug.log", logEntry);
        } catch {}
        // #endregion

        // #region agent log
        try {
            string logEntry = $"{{\"sessionId\":\"debug-session\",\"runId\":\"run1\",\"hypothesisId\":\"E\",\"location\":\"SpawnNode.cs:15\",\"message\":\"Received NodeType from server\",\"data\":{{\"nodeType\":{data.NodeType},\"nodePos\":{data.NodePos}}},\"timestamp\":{System.DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}}}\n";
            System.IO.File.AppendAllText(@"d:\GitRepo\Unity Racing Game\.cursor\debug.log", logEntry);
        } catch {}
        // #endregion

        //위 data 활용
        // 씬 전환 중일 수 있으므로 여러 번 시도
        NodeSpwaner ns = FindObjectOfType<NodeSpwaner>();

        if (ns == null)
        {
            Debug.LogWarning("NodeSpwaner not found! Scene might still be loading. Skipping node spawn.");
            return;
        }

        // #region agent log
        try {
            string logEntry = $"{{\"sessionId\":\"debug-session\",\"runId\":\"run1\",\"hypothesisId\":\"D\",\"location\":\"SpawnNode.cs:25\",\"message\":\"NodeSpwaner found, checking m_NodeList\",\"data\":{{\"nodeListCount\":{ns.NodeListCount},\"nodeListIsNull\":{ns.IsNodeListNull.ToString().ToLower()}}},\"timestamp\":{System.DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}}}\n";
            System.IO.File.AppendAllText(@"d:\GitRepo\Unity Racing Game\.cursor\debug.log", logEntry);
        } catch {}
        // #endregion

        // 노드 타입에 따라 다른 노드 스폰 (0: ObjectA, 1: ObjectB, 2: ObjectC, 3: AFail, 4: BFail, 5: CFail)
        // Fail 타입은 3번 프리팹 사용
        int nodeId = data.NodeType < 3 ? data.NodeType : 3;
        
        // #region agent log
        try {
            string logEntry = $"{{\"sessionId\":\"debug-session\",\"runId\":\"run1\",\"hypothesisId\":\"C\",\"location\":\"SpawnNode.cs:30\",\"message\":\"Calculated nodeId\",\"data\":{{\"originalNodeType\":{data.NodeType},\"calculatedNodeId\":{nodeId},\"nodeListCount\":{ns.NodeListCount}}},\"timestamp\":{System.DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}}}\n";
            System.IO.File.AppendAllText(@"d:\GitRepo\Unity Racing Game\.cursor\debug.log", logEntry);
        } catch {}
        // #endregion
        
        // 인덱스 범위 체크 및 클램핑 (m_NodeList 크기에 맞춤)
        if (ns.IsNodeListNull || ns.NodeListCount == 0)
        {
            Debug.LogError($"[SpawnNode] NodeList is null or empty! Cannot spawn node.");
            return;
        }
        
        // nodeId를 유효한 범위로 클램핑
        if (nodeId < 0)
            nodeId = 0;
        else if (nodeId >= ns.NodeListCount)
        {
            Debug.LogWarning($"[SpawnNode] nodeId {nodeId} is out of range (max: {ns.NodeListCount - 1}). Clamping to {ns.NodeListCount - 1}");
            nodeId = ns.NodeListCount - 1;
        }
        
        // 노드 위치에 따라 스폰
        GameObject spawnedNode = null;
        
        // #region agent log
        try {
            bool isValidIndex = !ns.IsNodeListNull && nodeId >= 0 && nodeId < ns.NodeListCount;
            string logEntry = $"{{\"sessionId\":\"debug-session\",\"runId\":\"run1\",\"hypothesisId\":\"A\",\"location\":\"SpawnNode.cs:35\",\"message\":\"Before spawning, validating nodeId\",\"data\":{{\"nodeId\":{nodeId},\"nodeListCount\":{ns.NodeListCount},\"nodePos\":{data.NodePos},\"isValidIndex\":{isValidIndex.ToString().ToLower()}}},\"timestamp\":{System.DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}}}\n";
            System.IO.File.AppendAllText(@"d:\GitRepo\Unity Racing Game\.cursor\debug.log", logEntry);
        } catch {}
        // #endregion
        
        switch (data.NodePos)
        {
            case 0: // Left
                spawnedNode = ns.SpawnNodeLeft(nodeId);
                break;
            case 1: // Center
                spawnedNode = ns.SpawnNodeCentre(nodeId);
                break;
            case 2: // Right
                spawnedNode = ns.SpawnNodeRight(nodeId);
                break;
            default:
                spawnedNode = ns.SpawnNodeCentre(nodeId);
                break;
        }

        // 스폰된 노드에 타입 정보 설정
        if (spawnedNode != null)
        {
            PickupScript ps = spawnedNode.GetComponent<PickupScript>();
            if (ps != null)
            {
                ps.nodeType = data.NodeType;

                // 판정은 Z거리 기반으로만 처리하므로 expectedTime 사용 안 함
                // float expectedTime = CalculateExpectedTime(spawnedNode, data.NodeTimeMs);
                // ps.SetExpectedTime(expectedTime);
            }
        }

        // 노트가 스폰될 때마다 진행 인덱스 증가
        if (GameModeManager.instance != null)
        {
            GameModeManager.instance.currentNoteIndex++;
            // Debug.Log($"Note Progress: {GameModeManager.instance.currentNoteIndex} / {GameModeManager.instance.totalNoteCount}");
        }
    }
}
