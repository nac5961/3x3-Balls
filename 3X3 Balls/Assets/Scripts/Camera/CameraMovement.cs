using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public GameObject overview;

    private GameObject cueBall;

    public GameObject CueBall
    {
        set { cueBall = value; }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Aim()
    {
        transform.position = GameInfo.instance.Cue.transform.position + new Vector3(0.0f, 2.5f, 0.0f);
        transform.LookAt(cueBall.transform);
    }

    public void ShowOverview()
    {
        transform.position = overview.transform.position;
        transform.rotation = overview.transform.rotation;
    }
}
