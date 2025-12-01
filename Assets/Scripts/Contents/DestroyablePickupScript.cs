using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyablePickupScript : PickupScript
{

    public override void OnPicked(Vector3 CrusherPosition)
    {
        if (bPicked) { return; }
        Debug.Log("AAAAAAAAAAAAA");

        bPicked = true;
    }

    public override void OnMissed()
    {
        Debug.Log("Missed : " + name);

        Destroy(gameObject, 0.9f);
    }

}
