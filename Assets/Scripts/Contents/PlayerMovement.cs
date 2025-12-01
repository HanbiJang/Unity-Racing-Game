using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField, Header("Score Text")]
    Text m_txScore;
    float m_Score;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        m_Score += 10 * Time.deltaTime;
        m_txScore.text = Mathf.Floor(m_Score).ToString();
    }
}
