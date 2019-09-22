﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cue : MonoBehaviour
{
    //Rotation
    public float hitAngle;

    //Force
    public float maxForce;

    //UI
    private AimDisplay aimUI;

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
        aimUI = GameObject.Find("Canvas").GetComponent<AimDisplay>();

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

            if ((!SceneInfo.instance.DisableControls && SceneInfo.instance.IsAiming) || (!SceneInfo.instance.DisableControls && isAnimatingHit))
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
    /// Handles input for hitting the ball and animates the cue during a hit.
    /// </summary>
    private void HitBall()
    {
        if (isAnimatingHit)
        {
            animationTime += animationSpeed * Time.deltaTime;
            animationTime = Mathf.Clamp(animationTime, 0.0f, 1.0f);

            transform.position = Vector3.Lerp(startPos, minPos, animationTime);

            //Hit ball
            if (animationTime >= 1.0f)
            {
                isAnimatingHit = false;

                float power = maxForce * aimUI.GetFillAmount();
                Vector3 force = SceneInfo.instance.ActiveBall.transform.position - transform.position;
                force = new Vector3(force.x, 0.0f, force.z).normalized * power;
                SceneInfo.instance.ActiveBall.GetComponent<Rigidbody>().AddForce(force, ForceMode.Force);

                SceneInfo.instance.IsHit = true;
            }
        }
        else
        {
            //Start taking shot
            if (!SceneInfo.instance.IsTakingShot && Input.GetButtonDown("Hit"))
            {
                SceneInfo.instance.IsTakingShot = true;
                FindAnimationPoints();
            }

            //Stop taking shot
            else if (SceneInfo.instance.IsTakingShot && Input.GetButtonDown("Cancel"))
            {
                SceneInfo.instance.IsTakingShot = false;
            }

            //Take shot
            else if (SceneInfo.instance.IsTakingShot && Input.GetButtonDown("Hit"))
            {
                SceneInfo.instance.IsAiming = false;
                SceneInfo.instance.IsTakingShot = false;

                startPos = transform.position;
                animationTime = 0.0f;
                isAnimatingHit = true;
            }

            //Move according to the power
            if (SceneInfo.instance.IsTakingShot)
            {
                transform.position = Vector3.Lerp(minPos, maxPos, aimUI.GetFillAmount());
            }
        }
    }
}
