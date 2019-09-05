using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public GameObject overview;

    private Cue cueScript;

    public Cue CueScript
    {
        set { cueScript = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!SceneInfo.instance.IsAiming)
        {
            FollowBall();
        }
    }

    public void Aim()
    {
        transform.position = cueScript.transform.position + new Vector3(0.0f, 2.5f, 0.0f);
        transform.LookAt(cueScript.ActiveCueBall.transform);
    }

    public void FollowBall()
    {
        if (cueScript.ActiveCueBall.GetComponent<CueBall>().OnPoolTable)
        {
            transform.position = overview.transform.position;
            transform.rotation = overview.transform.rotation;
        }
        else
        {
            Vector3 offset = cueScript.ActiveCueBall.GetComponent<Rigidbody>().velocity.normalized * 6.0f;
            offset = new Vector3(-offset.x, 1.5f, -offset.z);

            transform.position = cueScript.ActiveCueBall.transform.position + offset;
            transform.LookAt(cueScript.ActiveCueBall.transform);
        }
    }
}
