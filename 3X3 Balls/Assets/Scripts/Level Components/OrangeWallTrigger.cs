using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrangeWallTrigger : MonoBehaviour
{
    public GameObject nextDestination;

    private GameObject scoredBall;
    private Vector3 scoredBallVelocity;
    private Vector3 startPos;
    private Vector3 endPos;

    private int orangeWallLayer;

    private bool triggered;
    private float percentage;
    private float moveSpeed;
    private float timer;
    private float waitTime;

    // Start is called before the first frame update
    void Start()
    {
        scoredBallVelocity = Vector3.zero;
        startPos = Vector3.zero;
        endPos = Vector3.zero;

        orangeWallLayer = 10;

        triggered = false;
        percentage = 0.0f;
        moveSpeed = 0.5f;
        timer = 0.0f;
        waitTime = 0.2f;
    }

    // Update is called once per frame
    void Update()
    {
        if (triggered)
        {
            SwapBalls();
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            scoredBall = GameObject.Find("Orange Ball(Clone)");
            scoredBallVelocity = scoredBall.GetComponent<Rigidbody>().velocity;
            startPos = scoredBall.transform.position;
            endPos = SceneInfo.instance.ActiveBall.transform.position;

            SceneInfo.instance.ActiveBall.layer = 11;

            scoredBall.GetComponent<Rigidbody>().velocity = Vector3.zero;
            scoredBall.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

            SceneInfo.instance.ActiveBall.GetComponent<Rigidbody>().velocity = Vector3.zero;
            SceneInfo.instance.ActiveBall.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

            //other.gameObject.GetComponent<Collider>().isTrigger = true;
            //other.gameObject.GetComponent<MeshRenderer>().enabled = false;

            //transform.parent.parent.GetComponent<OrangeWalls>().ResetWalls();

            triggered = true;
        }
    }

    private void SwapBalls()
    {
        if (percentage < 1.0f)
        {
            if (timer < waitTime)
            {
                timer += Time.deltaTime;
            }
            else
            {
                percentage += moveSpeed * Time.deltaTime;
                percentage = Mathf.Clamp(percentage, 0.0f, 1.0f);

                scoredBall.transform.position = Vector3.Lerp(startPos, endPos, percentage);
                SceneInfo.instance.ActiveBall.transform.position = Vector3.Lerp(endPos, startPos, percentage);

                if (percentage >= 1.0f)
                {
                    scoredBall.GetComponent<Ball>().IsScored = true;
                    scoredBall.GetComponent<Collider>().isTrigger = true;
                    scoredBall.GetComponent<MeshRenderer>().enabled = false;

                    //SceneInfo.instance.ActiveBall.GetComponent<Rigidbody>().velocity = scoredBallVelocity;

                    Vector3 extraForce = scoredBallVelocity.normalized * 3.0f;
                    SceneInfo.instance.ActiveBall.GetComponent<Rigidbody>().AddForce(scoredBallVelocity, ForceMode.VelocityChange);
                    SceneInfo.instance.ActiveBall.GetComponent<Rigidbody>().AddForce(extraForce, ForceMode.VelocityChange);
                }
            }
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
            scoredBallVelocity = scoredBall.GetComponent<Rigidbody>().velocity;
            startPos = scoredBall.transform.position;
            endPos = SceneInfo.instance.ActiveBall.transform.position;

            SceneInfo.instance.ActiveBall.layer = 11;

            scoredBall.GetComponent<Rigidbody>().velocity = Vector3.zero;
            scoredBall.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

            SceneInfo.instance.ActiveBall.GetComponent<Rigidbody>().velocity = Vector3.zero;
            SceneInfo.instance.ActiveBall.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

            //other.gameObject.GetComponent<Collider>().isTrigger = true;
            //other.gameObject.GetComponent<MeshRenderer>().enabled = false;

            //transform.parent.parent.GetComponent<OrangeWalls>().ResetWalls();

            triggered = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Ball") && other.gameObject == SceneInfo.instance.ActiveBall)
        {
            triggered = false;
            timer = 0.0f;
            percentage = 0.0f;
            SceneInfo.instance.ActiveBall.layer = 0;
        }
    }
}
