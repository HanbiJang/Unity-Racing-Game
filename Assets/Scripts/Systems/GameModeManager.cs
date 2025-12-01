using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameModeManager : MonoBehaviour
{
    public static GameModeManager instance;

    [SerializeField, Header("Move Speed of Tile")]
    float g_RoadMoveSpeed;
    public float m_RoadMoveSpeed { get { return g_RoadMoveSpeed; } private set { g_RoadMoveSpeed = value; } }


    [SerializeField, Header("Length of Background Music(seconds)")]
    public float g_SoundLength;

    // Player Info
    public float m_PlayerScore;
    public int m_PlayerMaxHealth;
    public int m_PlayerHealth;

    public float s_RoadMoveSpeed;

    public void SetSpeed() { s_RoadMoveSpeed = m_RoadMoveSpeed; }


    public ObjectSpawner m_ObjectSpawner;

    public float m_CurrentTime { get; private set; }

    /// <summary>
    /// 재시작을 위해 게임 데이터를 초기화하는 함수
    /// </summary>
    public void ResetGame()
    {
        m_PlayerScore = 0f;
        m_CurrentTime = 0f;
        m_PlayerHealth = m_PlayerMaxHealth; // 체력 복구
        bGameOver = false; // 게임 오버 상태 해제

        // 스포너(장애물 생성기)가 작동 중이라면 멈춤
        if (m_ObjectSpawner != null)
        {
            m_ObjectSpawner.StopSpawning();
        }
    }

    public void GameStart()
    {
        m_CurrentTime = 0f;

        //if(LoadingRoad() == DONE)

        if (UIManager.instance.m_MenuUI != null && UIManager.instance.m_InGameUI != null)
        {
            UIManager.instance.m_MenuUI.SetActive(false);
            UIManager.instance.m_InGameUI.SetActive(true);
        }

        bGameOver = false;
    }

    // Start is called before the first frame update
    void Awake()
    {
        Screen.SetResolution(1080, 1920, false);
        if (instance == null)
            instance = this;

        DontDestroyOnLoad(gameObject);

        Apply();
    }

    public void Apply()
    {
        m_RoadMoveSpeed = g_RoadMoveSpeed;
        SetSpeed();
    }

    public bool bGameOver { get; private set; } = false;
    void Update()
    {
        if (m_ObjectSpawner == null) return;

        m_CurrentTime += Time.deltaTime;
        if (bGameOver)
            return;


        m_PlayerScore += Time.deltaTime * 330f;//= m_CurrentTime;

        if (m_ObjectSpawner.IsInvoking() && m_CurrentTime >= g_SoundLength - 7.7f)
        {
            //bGameOver = true;
            m_ObjectSpawner.StopSpawning();
        }
        if (m_CurrentTime >= g_SoundLength)
        {
            bGameOver = true;
            //m_ObjectSpawner.StopSpawning();
        }


    }
}
