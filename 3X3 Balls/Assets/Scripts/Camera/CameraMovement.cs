using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public GameObject overview;

    private Cue cueScript;
    private Vector3 offset;
    private bool setOffset;

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
        if (!setOffset)
        {
            offset = transform.position - cueScript.ActiveCueBall.transform.position;
            setOffset = true;
        }
        //if (!SceneInfo.instance.IsAiming)
        //{
        //    FollowBall();
        //}
    }

    void FixedUpdate()
    {
        if (cueScript.ActiveCueBall != null)
        {
            if (cueScript.ActiveCueBall.GetComponent<CueBall>().OnPoolTable)
            {
                transform.position = overview.transform.position;
                transform.rotation = overview.transform.rotation;
            }
            else if (!SceneInfo.instance.IsAiming)
            {
                transform.position = Vector3.Slerp(transform.position, cueScript.ActiveCueBall.transform.position + offset, 0.5f);
                transform.LookAt(cueScript.ActiveCueBall.transform);
            }
        }
    }
    public void Aim()
    {
        transform.position = cueScript.transform.position + new Vector3(0.0f, 2.5f, 0.0f);
        transform.LookAt(cueScript.ActiveCueBall.transform);
    }

    public void FollowBall()
    {
        //if (cueScript.ActiveCueBall.GetComponent<CueBall>().OnPoolTable)
        //{
        //    transform.position = overview.transform.position;
        //    transform.rotation = overview.transform.rotation;
        //}
        //else
        //{
        //    Vector3 offset = cueScript.ActiveCueBall.GetComponent<Rigidbody>().velocity.normalized * 6.0f;
        //    offset = new Vector3(-offset.x, 1.5f, -offset.z);

        //    transform.position = cueScript.ActiveCueBall.transform.position + offset;
        //    transform.LookAt(cueScript.ActiveCueBall.transform);
        //}
    }
}
