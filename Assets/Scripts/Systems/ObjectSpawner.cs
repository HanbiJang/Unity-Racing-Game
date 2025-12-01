using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ObjectSpawner : MonoBehaviour
{
    //public List<GameObject> pfb_Roads = new List<GameObject>();
    public List<GameObject> pfb_PickUps = new List<GameObject>();


    [SerializeField]
    float LeftOffset;
    [SerializeField]
    float RightOffset;

    void Start()
    {
        InvokeRepeating("spawnpickupTest", 0.3f + 0.5714f * 3, 0.5714f);
    }


    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.T)) 
        {
            SpawnPickup(0);
        }
    }

    public void StopSpawning()
    {
        CancelInvoke();
    }

    void spawnpickupTest() 
    {
        SpawnPickup(0);
    }

    public bool SpawnPickup(int id)
    {
        int randomIdx = Random.Range(0,3);


        GameObject go = Instantiate(
                pfb_PickUps[id],
                transform.position 
                + new Vector3(
                
                (randomIdx == 2?RightOffset:randomIdx == 1?0f:LeftOffset)
                
                ,2f,0f),
                Quaternion.identity
            );

        return go != null;
    }
}
