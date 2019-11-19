using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrangeWallTrigger : MonoBehaviour
{
    public Image fade;

    private GameObject scoredBall;
    private Vector3 scoredBallVelocity;
    private Vector3 targetPos;
    private Vector3 upOffset;

    private int orangeWallLayer;
    private int switchLayer;

    private bool trigger;
    private float percentage;
    private float fadeSpeed;
    private float timer;
    private float waitTime;
    private bool fadeIn;

    // Start is called before the first frame update
    void Start()
    {
        scoredBallVelocity = Vector3.zero;
        targetPos = Vector3.zero;
        upOffset = new Vector3(0.0f, 0.01f, 0.0f);

        orangeWallLayer = 10;
        switchLayer = 12;

        trigger = false;
        percentage = 0.0f;
        fadeSpeed = 2.0f;
        timer = 0.0f;
        waitTime = 0.5f;
        fadeIn = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneInfo.instance.GameStart && !SceneInfo.instance.Paused)
        {
            if (trigger)
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
        //Fade overlay in
        if (fadeIn)
        {
            percentage += fadeSpeed * Time.deltaTime;
            percentage = Mathf.Clamp(percentage, 0.0f, 1.0f);

            Color color = fade.color;
            color.a = percentage;
            fade.color = color;

            if (percentage >= 1.0f)
            {
                fadeIn = false;
            }
        }

        //Fade overlay out
        else
        {
            if (timer < waitTime)
            {
                timer += Time.deltaTime;

                if (timer >= waitTime)
                {
                    //Ensure the active ball doesn't collide with walls on switch
                    SceneInfo.instance.ActiveBall.layer = switchLayer;

                    //Move player to scored ball
                    SceneInfo.instance.ActiveBall.transform.position = targetPos;

                    //Remove scored ball
                    scoredBall.GetComponent<Ball>().IsScored = true;
                    scoredBall.GetComponent<Collider>().isTrigger = true;
                    scoredBall.GetComponent<MeshRenderer>().enabled = false;
                    scoredBall.GetComponent<Rigidbody>().velocity = Vector3.zero;
                    scoredBall.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                    scoredBall.GetComponent<Rigidbody>().useGravity = false; //turn off gravity, otherwise ball will continuosly fall due to being a trigger

                    //Give the player ball the scored ball's velocity
                    SceneInfo.instance.ActiveBall.GetComponent<Rigidbody>().AddForce(scoredBallVelocity, ForceMode.VelocityChange);

                    //Add a little extra force just in case the scored ball's velocity
                    //wasn't enough to move the ball out of the wall
                    Vector3 boost = transform.up * 10.0f;
                    SceneInfo.instance.ActiveBall.GetComponent<Rigidbody>().AddForce(boost, ForceMode.VelocityChange);

                    //Make sure to allow the game to end the turn again
                    SceneInfo.instance.DontEndTurn = false;
                }
            }
            else
            {
                percentage -= fadeSpeed * Time.deltaTime;
                percentage = Mathf.Clamp(percentage, 0.0f, 1.0f);

                Color color = fade.color;
                color.a = percentage;
                fade.color = color;

                //Hide fade UI
                if (percentage <= 0.0f)
                {
                    trigger = false;
                    fade.gameObject.SetActive(false);
                }
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
            targetPos = scoredBall.transform.position + upOffset; //offset makes sure ball isn't stuck in floor

            //Make sure the turn cannot end, just in case the balls stop
            //moving during the fade
            SceneInfo.instance.DontEndTurn = true;

            //This prevents multiple orange balls from being scored at once
            //by making the scored orange ball the only ball that can move
            //through walls.
            scoredBall.layer = switchLayer;
            transform.parent.parent.GetComponent<OrangeWalls>().ResetWallLayers();

            //Show fade UI
            fade.gameObject.SetActive(true);

            trigger = true;
            percentage = 0.0f;
            timer = 0.0f;
            fadeIn = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //Put the player's ball back to its default layer
        if (other.gameObject.CompareTag("Ball") && other.gameObject == SceneInfo.instance.ActiveBall)
        {
            SceneInfo.instance.ActiveBall.layer = 0;
        }
    }
}
