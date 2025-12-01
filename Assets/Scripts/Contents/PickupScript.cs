using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PickupScript : MonoBehaviour
{
    public bool bPicked;
    public abstract void OnPicked(Vector3 CrusherPosition);
    public abstract void OnMissed();
}