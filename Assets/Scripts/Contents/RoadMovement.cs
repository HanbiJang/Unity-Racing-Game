using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadMovement : MonoBehaviour
{
    void FixedUpdate()
    {
        if (GameModeManager.instance.bGameOver)
            return;
        //transform.position -= Vector3.forward * GameModeManager.instance.m_RoadMoveSpeed * Time.deltaTime;
    }

   // protected abstract void Move();
}
