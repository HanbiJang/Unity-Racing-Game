using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 노트 오브젝트 풀 — Instantiate/Destroy 대신 비활성화/재활성화로 재사용
public class NoteObjectPool : MonoBehaviour
{
    public static NoteObjectPool Instance { get; private set; }

    [System.Serializable]
    public class PoolEntry
    {
        public GameObject prefab;
        [Tooltip("게임 시작 시 미리 만들어 둘 노트 수")]
        public int initialSize = 10;
    }

    // NodeSpwaner.m_NodeList와 동일한 순서로 등록 (0~2: 일반, 3: 폭탄)
    [SerializeField] List<PoolEntry> entries = new List<PoolEntry>();

    // 풀 ID → 대기 중인 노트 큐
    readonly Dictionary<int, Queue<GameObject>> _pools = new Dictionary<int, Queue<GameObject>>();

    public int EntryCount => entries.Count;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;

        // Start가 아닌 Awake에서 초기화 — 어떤 스크립트보다 먼저 풀이 준비되도록
        WarmUp();
    }

    // 게임 시작 시 노트를 미리 생성해 풀에 채워 둠
    void WarmUp()
    {
        if (entries.Count == 0)
        {
            Debug.LogError("[NoteObjectPool] entries가 비어있음! Inspector에서 프리팹을 등록해야 합니다.");
            return;
        }

        for (int i = 0; i < entries.Count; i++)
        {
            if (entries[i].prefab == null)
            {
                Debug.LogError($"[NoteObjectPool] entries[{i}].prefab이 null입니다. Inspector를 확인하세요.");
                continue;
            }
            _pools[i] = new Queue<GameObject>();
            for (int j = 0; j < entries[i].initialSize; j++)
                _pools[i].Enqueue(CreateNote(i));
        }
        Debug.Log($"[NoteObjectPool] 풀 준비 완료 ({entries.Count}종)");
    }

    GameObject CreateNote(int id)
    {
        var obj = Instantiate(entries[id].prefab, transform);
        obj.GetComponent<PickupScript>()?.SetPoolId(id);
        obj.SetActive(false);
        return obj;
    }

    // 풀에서 노트 꺼내기 (풀이 비면 새로 만듦)
    public GameObject Get(int id, Vector3 pos)
    {
        if (!_pools.TryGetValue(id, out var pool))
        {
            Debug.LogError($"[NoteObjectPool] 없는 풀 ID: {id} — entries 등록 확인 필요");
            return null;
        }

        var obj = pool.Count > 0 ? pool.Dequeue() : CreateNote(id);

        obj.transform.SetParent(null);
        obj.transform.SetPositionAndRotation(pos, Quaternion.identity);
        obj.GetComponent<PickupScript>()?.ResetForPool();
        obj.SetActive(true);
        return obj;
    }

    // 노트를 풀에 반납
    public void Return(GameObject note)
    {
        if (note == null) return;

        var ps = note.GetComponent<PickupScript>();
        int id = ps != null ? ps.PoolId : -1;

        if (id < 0 || !_pools.ContainsKey(id))
        {
            Destroy(note);
            return;
        }

        note.SetActive(false);
        note.transform.SetParent(transform);
        _pools[id].Enqueue(note);
    }
}
