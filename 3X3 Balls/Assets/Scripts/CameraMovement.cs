using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private bool locked;
    public bool preHit;
    public bool postHit;

    // Start is called before the first frame update
    void Start()
    {
        locked = true;
        preHit = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (postHit)
        {
            
        }
        
    }

    public void MoveWithPoolCue(Transform poolCue)
    {
        if (locked && preHit)
        {
            transform.position = poolCue.position + new Vector3(0.0f, poolCue.gameObject.GetComponent<MeshRenderer>().bounds.extents.y * 2.0f, 0.0f);
            transform.LookAt(poolCue.gameObject.GetComponent<PoolCue>().targetBall.transform);
        }
    }
}
