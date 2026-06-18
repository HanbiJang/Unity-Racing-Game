using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 리듬게임 판정 시스템
/// Perfect, Good, Bad, Miss 판정을 처리합니다.
/// </summary>
public class JudgmentSystem : MonoBehaviour
{
    private static JudgmentSystem instance;

    public static JudgmentSystem Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<JudgmentSystem>();
                if (instance == null)
                {
                    GameObject tmp = new GameObject();
                    tmp.name = typeof(JudgmentSystem).Name;
                    instance = tmp.AddComponent<JudgmentSystem>();
                }
            }
            return instance;
        }
    }

    [Header("판정 윈도우 설정 (초 단위)")]
    [SerializeField]
    private float perfectWindow = 0.04f; // Perfect 판정 범위

    [SerializeField]
    private float goodWindow = 0.10f;    // Good 판정 범위

    [SerializeField]
    private float badWindow = 0.18f;     // Bad 판정 범위 — 반드시 goodWindow보다 커야 함

    // 시간 기반 판정 미사용으로 입력 지연 보정도 미사용
    // [Header("입력 지연 보정 (초 단위)")]
    // [SerializeField]
    // private float inputDelayCompensation = 0.0f;

    [Header("판정 점수")]
    [SerializeField]
    private int perfectScore = 100;

    [SerializeField]
    private int goodScore = 50;

    [SerializeField]
    private int badScore = 10;

    [SerializeField]
    private int missScore = 0;

    /// <summary>
    /// 판정 결과 타입
    /// </summary>
    public enum JudgmentType
    {
        Perfect = 0,
        Good = 1,
        Bad = 2,
        Miss = 3
    }

    /// <summary>
    /// 판정 결과 데이터
    /// </summary>
    public class JudgmentResult
    {
        public JudgmentType type;
        public float timeDifference;  // 예상 타이밍과의 차이 (초)
        public int score;

        public JudgmentResult(JudgmentType type, float timeDifference, int score)
        {
            this.type = type;
            this.timeDifference = timeDifference;
            this.score = score;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    // 시간 기반 판정 — 현재 미사용 (Z거리 기반 JudgeByDistance로 대체됨)
    // public JudgmentResult Judge(float expectedTime, float currentTime) { ... }

    /// <summary>
    /// 판정 타입을 문자열로 반환합니다.
    /// </summary>
    public static string GetJudgmentTypeString(JudgmentType type)
    {
        switch (type)
        {
            case JudgmentType.Perfect:
                return "Perfect";
            case JudgmentType.Good:
                return "Good";
            case JudgmentType.Bad:
                return "Bad";
            case JudgmentType.Miss:
                return "Miss";
            default:
                return "Unknown";
        }
    }

    /// <summary>
    /// 판정 윈도우 설정을 가져옵니다.
    /// </summary>
    public float GetPerfectWindow() => perfectWindow;
    public float GetGoodWindow() => goodWindow;
    public float GetBadWindow() => badWindow;

    /// <summary>
    /// 판정 윈도우 설정을 변경합니다.
    /// </summary>
    public void SetJudgmentWindows(float perfect, float good, float bad)
    {
        perfectWindow = perfect;
        goodWindow = good;
        badWindow = bad;
    }

    [Header("거리 기반 판정 범위 (Z축)")]
    [SerializeField] private float perfectDistanceWindow = 0.4f;  // |zDiff| 이하면 Perfect
    [SerializeField] private float goodDistanceWindow    = 1.0f;  // |zDiff| 이하면 Good
    [SerializeField] private float badDistanceWindow     = 1.92f; // |zDiff| 이하면 Bad, 초과면 Miss

    /// <summary>
    /// 노트와 플레이어 간 Z 거리 기반으로 판정합니다.
    /// 트리거 존 중앙에 가까울수록 Perfect, 멀수록 Bad.
    /// </summary>
    public JudgmentResult JudgeByDistance(float noteZ, float playerZ)
    {
        float zDiff = Mathf.Abs(noteZ - playerZ);

        JudgmentType type;
        int score;

        if (zDiff <= perfectDistanceWindow)
        {
            type  = JudgmentType.Perfect;
            score = perfectScore;
        }
        else if (zDiff <= goodDistanceWindow)
        {
            type  = JudgmentType.Good;
            score = goodScore;
        }
        else if (zDiff <= badDistanceWindow)
        {
            type  = JudgmentType.Bad;
            score = badScore;
        }
        else
        {
            type  = JudgmentType.Miss;
            score = missScore;
        }

        Debug.Log($"[JudgmentSystem] DistanceJudge: {GetJudgmentTypeString(type)}, zDiff={zDiff:F3}");
        return new JudgmentResult(type, zDiff, score);
    }
}


