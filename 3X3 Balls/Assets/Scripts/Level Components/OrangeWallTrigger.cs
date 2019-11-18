using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrangeWallTrigger : MonoBehaviour
{
    private GameObject scoredBall;
    private Vector3 scoredBallVelocity;
    private Vector3 startPos;
    private Vector3 endPos;
    private Vector3 upOffset;

    private int orangeWallLayer;
    private int switchLayer;

    private bool triggered;
    private float percentage;
    private float moveSpeed;

    // Start is called before the first frame update
    void Start()
    {
        scoredBallVelocity = Vector3.zero;
        startPos = Vector3.zero;
        endPos = Vector3.zero;
        upOffset = new Vector3(0.0f, 0.01f, 0.0f);

        orangeWallLayer = 10;
        switchLayer = 12;

        triggered = false;
        percentage = 0.0f;
        moveSpeed = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneInfo.instance.GameStart && !SceneInfo.instance.Paused)
        {
            if (triggered)
            {
                SwapBalls();
            }
        }
    }

    /// <summary>
    /// Swaps the orange ball with the player ball.
    /// </summary>
    private void SwapBalls()
    {
        if (percentage < 1.0f)
        {
            percentage += moveSpeed * Time.deltaTime;
            percentage = Mathf.Clamp(percentage, 0.0f, 1.0f);

            scoredBall.transform.position = Vector3.Lerp(startPos, endPos, percentage);
            SceneInfo.instance.ActiveBall.transform.position = Vector3.Lerp(endPos, startPos, percentage);

            if (percentage >= 1.0f)
            {
                //Remove scored ball
                scoredBall.GetComponent<Ball>().IsScored = true;
                scoredBall.GetComponent<Collider>().isTrigger = true;
                scoredBall.GetComponent<MeshRenderer>().enabled = false;
                scoredBall.GetComponent<Rigidbody>().useGravity = false; //turn off gravity, otherwise ball will continuosly fall due to being a trigger

                //Give the player ball the scored ball's velocity
                SceneInfo.instance.ActiveBall.GetComponent<Rigidbody>().AddForce(scoredBallVelocity, ForceMode.VelocityChange);

                //Add a little extra force just in case the scored ball's velocity
                //wasn't enough to move the ball out of the wall
                Vector3 boost = transform.up * 10.0f;
                SceneInfo.instance.ActiveBall.GetComponent<Rigidbody>().AddForce(boost, ForceMode.VelocityChange);

                //Make sure to remove the temp lock
                Camera.main.GetComponent<ThirdPersonCamera>().TempUnlockCam();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Make sure this is an orange ball and the trigger
        //is inside an orange wall
        if (other.gameObject.CompareTag("Ball") && other.name.Contains("Orange") && transform.parent.gameObject.layer == orangeWallLayer)
        {
            //Set variables
            scoredBall = other.gameObject;
            scoredBallVelocity = scoredBall.GetComponent<Rigidbody>().velocity;
            startPos = scoredBall.transform.position + upOffset; //offset makes sure ball isn't stuck in floor
            endPos = SceneInfo.instance.ActiveBall.transform.position + upOffset; //offset makes sure ball isn't stuck in floor

            //Temporarily lock the camera to perform the switch without camera stutters
            Camera.main.GetComponent<ThirdPersonCamera>().TempLockCam();

            //Zero out movement to lerp
            scoredBall.GetComponent<Rigidbody>().velocity = Vector3.zero;
            scoredBall.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

            SceneInfo.instance.ActiveBall.GetComponent<Rigidbody>().velocity = Vector3.zero;
            SceneInfo.instance.ActiveBall.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

            //Set appropriate layers for collision.
            //This prevents multiple orange balls from being scored at once
            //by making the scored orange ball and the player ball the
            //only 2 balls that can move through the walls.
            scoredBall.layer = switchLayer;
            SceneInfo.instance.ActiveBall.layer = switchLayer;
            transform.parent.parent.GetComponent<OrangeWalls>().ResetWallLayers();

            triggered = true;
            percentage = 0.0f;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Ball") && other.gameObject == SceneInfo.instance.ActiveBall)
        {
            triggered = false;
            SceneInfo.instance.ActiveBall.layer = 0; //Put ball back to its default layer
        }
    }
}
