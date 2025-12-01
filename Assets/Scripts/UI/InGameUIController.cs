using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameUIController : MonoBehaviour
{
    public GameObject m_InGameUI;
    [SerializeField, Header("in-game UI Elements")]
    Slider m_ProgressBar;
    [SerializeField]
    Text m_txScore;
    [SerializeField]
    Text m_txCurrentRanking;

    [SerializeField, Range(0f, 1f)]
    float m_HealthRate;
    [SerializeField]
    Image m_HealthBarImageLeft;
    [SerializeField]
    Image m_HealthBarImageRight;


    //find in-game ui elements
    public void DoBeforeGameStart()
    {
        if (m_InGameUI == null)
            m_InGameUI = GameObject.FindWithTag("InGameUI");

        foreach (Transform t in m_InGameUI.transform)
            if (t.CompareTag("MusicProgressBar"))
                m_ProgressBar = t.GetComponent<Slider>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //m_txScore.text = Mathf.Floor(GameModeManager.instance.m_PlayerScore).ToString();
        //m_ProgressBar.value = ((GameModeManager.instance.m_CurrentTime) / (GameModeManager.instance.g_SoundLength));
        //Debug.Log(m_ProgressBar.value);
        //m_HealthRate = (float)GameModeManager.instance.m_PlayerHealth / GameModeManager.instance.m_PlayerMaxHealth;

        m_HealthBarImageLeft.color = m_HealthBarImageRight.color = 0.7f * Color.red * Mathf.Lerp(1, 0, m_HealthRate);
    }
}
