using PathCreation.Examples;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerFollower : PathFollower
{
    [SerializeField]
    PlayerInputController ctr;

    [SerializeField]
    Camera m_Camera;


    // Update is called once per frame
    protected override void Update()
    {
        //Vector3 ang = Quaternion.ToEulerAngles(transform.rotation);
        base.Update();
        ctr.transform.localPosition = new Vector3((ctr.bLeft ? ctr.LeftPosition.x : 0f) + (ctr.bRight ? ctr.RightPosition.x : 0f), 0f,0f);
        //transform.rotation = Quaternion.Euler(ang.x,ang.y,0);
    }
}
