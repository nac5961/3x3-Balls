﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ShotType
{
    Normal,
    Jump,
    Curve
}

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

    //Special Shot
    private ShotType shot;
    private Vector3 jumpForce;
    private float maxCurvePower;
    private float minCurvePower;
    private bool curveRight;
    private bool specialShotEnabled;

    //Sound
    private bool playedHitSound;
    private float minVolume;
    private float maxVolume;

    // Start is called before the first frame update
    void Start()
    {
        //UI
        shotUI = UIGameInfo.instance.ShotUI.GetComponent<ShotUI>();

        //Animation
        animationTime = 0.0f;
        isAnimatingHit = false;

        //Special Shot
        shot = ShotType.Normal;
        jumpForce = new Vector3(0.0f, 180.0f, 0.0f);
        maxCurvePower = 400.0f;
        minCurvePower = 200.0f;
        curveRight = true;
        specialShotEnabled = true;

        //Sound
        playedHitSound = false;
        minVolume = 0.2f;
        maxVolume = 0.5f;
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

            //Cue Hit Sound Effect
            //We need to play the sound before the force is applied,
            //otherwise the sound will look like it is delayed when playing the game,
            //since it doesn't play fast enough.
            if (animationTime >= 0.3f && !playedHitSound)
            {
                playedHitSound = true;

                //Change volume based on power
                float volume = Mathf.Clamp(shotUI.GetFillAmount() / 1.0f, minVolume, maxVolume);
                AudioInfo.instance.PlayCueHitSoundEffect(volume);
            }
        }

        //Input
        else
        {
            if (!Camera.main.GetComponent<ThirdPersonCamera>().Locked)
            {
                Ball player = SceneInfo.instance.ActiveBall.GetComponent<Ball>();

                //Start taking jump shot
                if (player.CanJumpShot && specialShotEnabled && !SceneInfo.instance.IsTakingShot && Input.GetButton("JumpHit") && Input.GetButtonDown("Hit"))
                {
                    shot = ShotType.Jump;
                    shotUI.SetBorder(shot);

                    SceneInfo.instance.IsTakingShot = true;
                    FindAnimationPoints();

                    UIGameInfo.instance.DisplayShotUI();
                }

                //Start taking curve shot
                else if (player.CanCurveShot && specialShotEnabled && !SceneInfo.instance.IsTakingShot && Input.GetButton("CurveHit") && Input.GetButtonDown("Hit"))
                {
                    shot = ShotType.Curve;
                    shotUI.SetBorder(shot, curveRight);

                    SceneInfo.instance.IsTakingShot = true;
                    FindAnimationPoints();

                    UIGameInfo.instance.DisplayShotUI();
                }

                //Start taking normal shot
                else if (!SceneInfo.instance.IsTakingShot && !Input.GetButton("JumpHit") && !Input.GetButton("CurveHit") && Input.GetButtonDown("Hit"))
                {
                    shot = ShotType.Normal;
                    shotUI.SetBorder(shot);

                    SceneInfo.instance.IsTakingShot = true;
                    FindAnimationPoints();

                    UIGameInfo.instance.DisplayShotUI();
                }

                //Stop taking shot
                else if (SceneInfo.instance.IsTakingShot && !shotUI.PowerSet && Input.GetButtonDown("Cancel"))
                {
                    SceneInfo.instance.IsTakingShot = false;

                    UIGameInfo.instance.HideShotUI(false);
                }

                //Change curve direction
                else if (SceneInfo.instance.IsTakingShot && shot == ShotType.Curve && Input.GetButtonDown("CurveDirectionToggle"))
                {
                    curveRight = !curveRight;
                    shotUI.SetCurveBorder(curveRight);
                }

                //Hold shot
                else if (SceneInfo.instance.IsTakingShot && Input.GetButtonDown("Hit"))
                {
                    shotUI.PowerSet = true;
                    shotUI.ShowReleaseUI();
                }

                //Take shot
                else if (SceneInfo.instance.IsTakingShot && shotUI.PowerSet && Input.GetButtonUp("Hit"))
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
                        specialShotEnabled = true;

                        player.Spin = SpinType.Normal;
                        UIGameInfo.instance.AimUI.GetComponent<AimUI>().SetCenterHit();
                    }
                    else if (Input.GetButtonDown("TopSpin"))
                    {
                        specialShotEnabled = false;

                        player.Spin = SpinType.Top;
                        UIGameInfo.instance.AimUI.GetComponent<AimUI>().SetTopHit();
                    }
                    else if (Input.GetButtonDown("BackSpin"))
                    {
                        specialShotEnabled = false;

                        player.Spin = SpinType.Back;
                        UIGameInfo.instance.AimUI.GetComponent<AimUI>().SetBottomHit();
                    }
                    else if (Input.GetButtonDown("LeftSpin"))
                    {
                        specialShotEnabled = false;

                        player.Spin = SpinType.Left;
                        UIGameInfo.instance.AimUI.GetComponent<AimUI>().SetLeftHit();
                    }
                    else if (Input.GetButtonDown("RightSpin"))
                    {
                        specialShotEnabled = false;

                        player.Spin = SpinType.Right;
                        UIGameInfo.instance.AimUI.GetComponent<AimUI>().SetRightHit();
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

            //Enable collisions after first hit
            if (SceneInfo.instance.ActiveBall.GetComponent<Collider>().isTrigger)
            {
                SceneInfo.instance.ActiveBall.GetComponent<Collider>().isTrigger = false;
                SceneInfo.instance.ActiveBall.GetComponent<Rigidbody>().useGravity = true;
            }

            //Apply force
            float power = maxForce * fillAmount;
            Vector3 force = SceneInfo.instance.ActiveBall.transform.position - transform.position;
            force = new Vector3(force.x, 0.0f, force.z).normalized * power;

            switch (shot)
            {
                case ShotType.Jump:
                    SceneInfo.instance.ActiveBall.GetComponent<Ball>().CanJumpShot = false;

                    SceneInfo.instance.ActiveBall.GetComponent<Rigidbody>().AddForce(jumpForce);
                    break;
                case ShotType.Curve:
                    SceneInfo.instance.ActiveBall.GetComponent<Ball>().CanCurveShot = false;

                    //Curve force is a leftward or rightward force to have the ball
                    //move to the side then curve back in.
                    float curvePower = Mathf.Clamp(maxCurvePower * fillAmount, minCurvePower, maxCurvePower);
                    Vector3 curveForce = Quaternion.Euler(0.0f, 90.0f, 0.0f) * force;
                    curveForce = curveForce.normalized * curvePower;
                    curveForce = curveRight ? curveForce : -curveForce;
                    SceneInfo.instance.ActiveBall.GetComponent<Rigidbody>().AddForce(curveForce);

                    //Torque allows the ball to curve in after the leftward/rightward force
                    Vector3 torque = force;
                    torque = torque.normalized * (curvePower - 50.0f);
                    torque = curveRight ? torque : -torque;
                    SceneInfo.instance.ActiveBall.GetComponent<Rigidbody>().AddTorque(torque);
                    break;
                default:
                    break;
            }

            //Add the standard forwards force
            SceneInfo.instance.ActiveBall.GetComponent<Rigidbody>().AddForce(force);

            //Update stroke count
            SceneInfo.instance.UpdatePlayerScore();
            UIGameInfo.instance.GeneralUI.GetComponent<GeneralUI>().SetStrokeCount();

            SceneInfo.instance.IsHit = true;

            //Always reset after each shot
            playedHitSound = false;

            //Always reset after each shot since players are on the center
            //shot at the start of their turn
            curveRight = true;
            specialShotEnabled = true;
        }
    }
}
