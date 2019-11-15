using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrangeWallTrigger : MonoBehaviour
{
    public GameObject nextDestination;

    private GameObject scoredBall;

    private int orangeWallLayer;

    private bool triggered;
    private float timer;
    private float waitTime;

    // Start is called before the first frame update
    void Start()
    {
        orangeWallLayer = 10;

        triggered = false;
        timer = 0.0f;
        waitTime = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (triggered)
        {
            GoToNextDestination();
        }
    }

    private void GoToNextDestination()
    {
        if (timer < waitTime)
        {
            timer += Time.deltaTime;
        }
        else
        {
            triggered = false;

            scoredBall.GetComponent<Ball>().IsScored = true;

            SceneInfo.instance.ActiveBall.transform.position = nextDestination.transform.position;
            SceneInfo.instance.ActiveBall.GetComponent<Rigidbody>().velocity = Vector3.zero;
            SceneInfo.instance.ActiveBall.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ball") && other.name.Contains("Orange") && transform.parent.gameObject.layer == orangeWallLayer)
        {
            scoredBall = other.gameObject;

            other.gameObject.GetComponent<Collider>().isTrigger = true;
            other.gameObject.GetComponent<MeshRenderer>().enabled = false;

            transform.parent.parent.GetComponent<OrangeWalls>().ResetWalls();

            triggered = true;
        }
    }
}
