using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillZScript : MonoBehaviour
{
    //[SerializeField]
    //Transform m_RoadRespawnTransform;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PickupItem")) 
        {
            if (other.gameObject.GetComponent<FloatablePickupScript>() != null)
            {
                Debug.Log(name + " : PickupItem trigger");
                //GameModeManager.instance.m_PlayerHealth -= 30;
                Destroy(other.gameObject);
            }
        }

        if (other.CompareTag("RoadBlock")) 
        {
            Debug.Log(name + " : RoadBlock trigger");
            //other.transform.position = m_RoadRespawnTransform.position;
        }
    }
}
