using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cue : MonoBehaviour
{
    //Rotation
    public float hitAngle;

    //Force
    public float maxForce;

    //UI
    public float marginOfError;
    private ShotUI shotUI;

    //Animation
    public float distance;
    public float animationSpeed;
    private float animationTime;
    private bool isAnimatingHit;
    private Vector3 startPos;
    private Vector3 minPos;
    private Vector3 maxPos;

    // Start is called before the first frame update
    void Start()
    {
        //UI
        shotUI = UIGameInfo.instance.ShotUI.GetComponent<ShotUI>();

        //Animation
        animationTime = 0.0f;
        isAnimatingHit = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneInfo.instance.GameStart && !SceneInfo.instance.Paused)
        {
            if (SceneInfo.instance.IsAiming && !SceneInfo.instance.IsTakingShot)
            {
                AlignWithBall();
            }

            if ((!SceneInfo.instance.DisableControls && SceneInfo.instance.IsAiming) || isAnimatingHit)
            {
                ProcessHitInput();
            }
        }
    }

    private void FixedUpdate()
    {
        if (SceneInfo.instance.GameStart && !SceneInfo.instance.Paused)
        {
            if (isAnimatingHit)
            {
                HitBall();
            }
        }
    }

    /// <summary>
    /// Positions the cue on the active ball based on the camera.
    /// </summary>
    private void AlignWithBall()
    {
        if (!Camera.main.GetComponent<ThirdPersonCamera>().Locked)
        {
            GameObject ball = SceneInfo.instance.ActiveBall;
            SphereCollider collider = ball.GetComponent<SphereCollider>();

            //Always reset rotation so the transform.RotateAround doesn't stack
            transform.rotation = Quaternion.identity;

            //Zero out the y to prevent the cue from moving up and down
            Vector3 toCam = Camera.main.transform.position - ball.transform.position;
            toCam = new Vector3(toCam.x, 0.0f, toCam.z);

            //Move to the edge of the ball
            transform.position = ball.transform.position + (toCam.normalized * collider.radius);

            //Rotate the cue around itself to not have the rotation change its position
            transform.RotateAround(transform.position, Camera.main.transform.right, hitAngle);
        }
    }

    /// <summary>
    /// Sets the minimum and maximum positions for animating the cue away from and
    /// towards the ball.
    /// </summary>
    private void FindAnimationPoints()
    {
        minPos = transform.position;

        Vector3 toCue = transform.position - SceneInfo.instance.ActiveBall.transform.position;

        maxPos = minPos + (toCue.normalized * distance);
    }

    /// <summary>
    /// Processes input for hitting the ball and animates the cue.
    /// </summary>
    private void ProcessHitInput()
    {
        //Animation
        if (isAnimatingHit)
        {
            animationTime += animationSpeed * Time.deltaTime;
            animationTime = Mathf.Clamp(animationTime, 0.0f, 1.0f);

            transform.position = Vector3.Lerp(startPos, minPos, animationTime);
        }

        //Input
        else
        {
            if (!Camera.main.GetComponent<ThirdPersonCamera>().Locked)
            {
                //Start taking shot
                if (!SceneInfo.instance.IsTakingShot && Input.GetButtonDown("Hit"))
                {
                    SceneInfo.instance.IsTakingShot = true;
                    FindAnimationPoints();

                    UIGameInfo.instance.DisplayShotUI();
                }

                //Stop taking shot
                else if (SceneInfo.instance.IsTakingShot && Input.GetButtonDown("Cancel"))
                {
                    SceneInfo.instance.IsTakingShot = false;

                    UIGameInfo.instance.HideShotUI(false);
                }

                //Take shot
                else if (SceneInfo.instance.IsTakingShot && Input.GetButtonDown("Hit"))
                {
                    SceneInfo.instance.IsAiming = false;
                    SceneInfo.instance.IsTakingShot = false;

                    startPos = transform.position;
                    animationTime = 0.0f;
                    isAnimatingHit = true;

                    if (shotUI.GetFillAmount() >= 1.0f - marginOfError)
                    {
                        UIGameInfo.instance.HideShotUI(true, true);
                    }
                    else
                    {
                        UIGameInfo.instance.HideShotUI(true);
                    }
                }

                //Set spin type
                else if (!SceneInfo.instance.IsTakingShot)
                {
                    if (Input.GetButtonDown("NormalSpin"))
                    {
                        SceneInfo.instance.ActiveBall.GetComponent<Ball>().Spin = SpinType.Normal;
                    }
                    else if (Input.GetButtonDown("TopSpin"))
                    {
                        SceneInfo.instance.ActiveBall.GetComponent<Ball>().Spin = SpinType.Top;
                    }
                    else if (Input.GetButtonDown("BackSpin"))
                    {
                        SceneInfo.instance.ActiveBall.GetComponent<Ball>().Spin = SpinType.Back;
                    }
                    else if (Input.GetButtonDown("LeftSpin"))
                    {
                        SceneInfo.instance.ActiveBall.GetComponent<Ball>().Spin = SpinType.Left;
                    }
                    else if (Input.GetButtonDown("RightSpin"))
                    {
                        SceneInfo.instance.ActiveBall.GetComponent<Ball>().Spin = SpinType.Right;
                    }
                }

                //Move according to the power
                if (SceneInfo.instance.IsTakingShot)
                {
                    transform.position = Vector3.Lerp(minPos, maxPos, shotUI.GetFillAmount());
                }
            }
        }
    }

    /// <summary>
    /// Applies the necessary force to the ball.
    /// </summary>
    private void HitBall()
    {
        //Finished animating
        if (animationTime >= 1.0f)
        {
            isAnimatingHit = false;

            float fillAmount = shotUI.GetFillAmount();

            //Apply bonus if at max
            if (fillAmount >= 1.0f - marginOfError)
            {
                fillAmount = 1.2f;
            }

            //SHOWCASE (DELETE ME)
            if (Input.GetKey(KeyCode.K))
            {
                fillAmount = 1.2f;
            }

            //Enable collisions after first hit
            if (SceneInfo.instance.ActiveBall.GetComponent<SphereCollider>().isTrigger)
            {
                SceneInfo.instance.ActiveBall.GetComponent<SphereCollider>().isTrigger = false;
                SceneInfo.instance.ActiveBall.GetComponent<Rigidbody>().useGravity = true;
            }

            //Apply force
            float power = maxForce * fillAmount;
            Vector3 force = SceneInfo.instance.ActiveBall.transform.position - transform.position;
            force = new Vector3(force.x, 0.0f, force.z).normalized * power;
            SceneInfo.instance.ActiveBall.GetComponent<Rigidbody>().AddForce(force);

            //Update stroke count
            SceneInfo.instance.UpdatePlayerScore();
            UIGameInfo.instance.GeneralUI.GetComponent<GeneralUI>().SetStrokeCount();

            SceneInfo.instance.IsHit = true;
        }
    }
}
