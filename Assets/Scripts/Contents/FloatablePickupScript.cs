using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatablePickupScript : PickupScript
{
    public override void OnPicked(Vector3 CrusherPosition) 
    {
        if(bPicked) { return; }
        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.useGravity = true;
        rb.AddForceAtPosition(new Vector3(Random.Range(1000f,-1000f), 3000f, Random.Range(1000f, -1000f)),CrusherPosition);
        bPicked = true;
        //SoundManager.instance.PlaySound(me);
        //GameModeManager.instance.m_PlayerScore += 3000;
        //

        Destroy(gameObject, 0.7f);
    }

    public override void OnMissed()
    {
        Debug.Log("Missed : "+name);

        Destroy(gameObject, 0.9f);
    }
}
