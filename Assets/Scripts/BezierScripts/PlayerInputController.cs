using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputController : MonoBehaviour
{
    public Vector3 LeftPosition;
    public Vector3 RightPosition;
    public Vector3 MidPosition;

    public bool bLeft = false;
    public bool bRight = false;

    void Update()
    {
        if (Input.GetAxisRaw("Horizontal") < 0)
        {
            bLeft = true;
            bRight = false;
        }
        if (Input.GetAxisRaw("Horizontal") > 0)
        {
            bRight = true;
            bLeft = false;
        }
        if (Mathf.Approximately(Input.GetAxisRaw("Horizontal"), 0f))
        {
            bRight = false;
            bLeft = false;
        }

        //transform.position = MidPosition + (bLeft ? LeftPosition : Vector3.zero) + (bRight ? RightPosition : Vector3.zero);
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PickupItem") )
        {
            PickupScript ps = other.gameObject.GetComponent<PickupScript>();
            ps.OnPicked(transform.position);
        }
    }
}
