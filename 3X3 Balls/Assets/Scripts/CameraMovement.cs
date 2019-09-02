using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private GameObject cue;

    private bool isAiming;
    private bool followCueBall;
    private bool followHitBall;
    private bool showMultipleBalls;

    public GameObject Cue
    {
        set { cue = value; }
    }

    public bool IsAiming
    {
        get { return isAiming; }
        set { isAiming = value; }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isAiming)
        {
            Aim();
        }
    }

    private void Aim()
    {
        transform.position = cue.transform.position + new Vector3(0.0f, 1.5f, 0.0f);
        transform.LookAt(cue.GetComponent<PoolCue>().CueBall.transform);
    }
}
