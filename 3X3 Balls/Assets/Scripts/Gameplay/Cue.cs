using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cue : MonoBehaviour
{
    public float maxForce;

    private AimDisplay aimUI;

    private bool isAnimatingHit;
    private float animationTime;
    private float animationSpeed;

    //Animation
    private Vector3 minPos;
    private Vector3 maxPos;
    private float distance;

    // Start is called before the first frame update
    void Start()
    {
        isAnimatingHit = false;
        animationTime = 0.0f;
        animationSpeed = 1.0f;
        distance = 8.0f;

        aimUI = GameObject.Find("Canvas").GetComponent<AimDisplay>();
    }

    // Update is called once per frame
    void Update()
    {
        AlignWithBall();
        HitBall();
    }

    /// <summary>
    /// Positions the cue on the active ball based on the camera.
    /// </summary>
    public void AlignWithBall()
    {
        if (SceneInfo.instance.ActiveBall)
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
            transform.RotateAround(transform.position, Camera.main.transform.right, -80.0f);
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

    private void HitBall()
    {
        if (isAnimatingHit)
        {
            animationTime += animationSpeed * Time.deltaTime;
            animationTime = Mathf.Clamp(animationTime, 0.0f, 1.0f);

            Vector3.Lerp(transform.position, minPos, animationTime);

            if (animationTime >= 1.0f)
            {
                isAnimatingHit = false;

                float power = maxForce * aimUI.GetFillAmount();
                Vector3 force = SceneInfo.instance.ActiveBall.transform.position - transform.position;
                force = new Vector3(force.x, 0.0f, force.z) * power;
                SceneInfo.instance.ActiveBall.GetComponent<Rigidbody>().AddForce(force, ForceMode.Force);
            }
        }
        else
        {
            //Start taking shot
            if (!SceneInfo.instance.IsTakingShot && Input.GetAxis("Hit") > 0.0f)
            {
                SceneInfo.instance.IsTakingShot = true;
                FindAnimationPoints();
            }

            //Stop taking shot
            else if (SceneInfo.instance.IsTakingShot && Input.GetAxis("Cancel") > 0.0f)
            {
                SceneInfo.instance.IsTakingShot = false;
            }

            //Take shot
            else if (SceneInfo.instance.IsTakingShot && Input.GetAxis("Hit") > 0.0f)
            {
                SceneInfo.instance.IsAiming = false;
                SceneInfo.instance.IsTakingShot = false;

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


    /// <summary>
    /// Aligns the pool cue with the ball based on the size of the cue and the ball
    /// </summary>
    //public void AlignWithBall()
    //{
    //    SceneInfo.instance.IsAiming = true;

    //    activeCueBall = SceneInfo.instance.Turn == GameInfo.instance.P1Type ? p1CueBall : p2CueBall;

    //    Bounds ballBounds = activeCueBall.GetComponent<MeshRenderer>().bounds;

    //    //Note: This works based on the assumption that the pivot point is at the center of the pool cue.
    //    float xOffset = boundsY + ballBounds.extents.x;
    //    float yOffset = ballBounds.extents.y;
    //    float zOffset = 0.0f;

    //    //Reset position and angle
    //    transform.position = activeCueBall.transform.position + new Vector3(-xOffset, yOffset, zOffset);
    //    transform.rotation = Quaternion.Euler(0.0f, 0.0f, hitAngle);
    //}

    //private void HitBall()
    //{
    //    if (!SceneInfo.instance.IsTakingShot && Input.GetKeyDown(KeyCode.Space))
    //    {
    //        SceneInfo.instance.IsTakingShot = true;
    //    }
    //    else if (SceneInfo.instance.IsTakingShot && Input.GetKeyDown(KeyCode.Backspace))
    //    {
    //        SceneInfo.instance.IsTakingShot = false;
    //    }
    //    else if (SceneInfo.instance.IsTakingShot && Input.GetKeyDown(KeyCode.Space))
    //    {
    //        SceneInfo.instance.IsAiming = false;
    //        SceneInfo.instance.IsTakingShot = false;

    //        float power = maxForce * aimUI.GetFillAmount();

    //        Vector3 force = activeCueBall.transform.position - transform.position;
    //        force = new Vector3(force.x, 0.0f, force.z) * power;
    //        activeCueBall.GetComponent<Rigidbody>().AddForce(force, ForceMode.Force);
    //    }

    //    //SHOWCASE
    //    else if (SceneInfo.instance.IsTakingShot && Input.GetKeyDown(KeyCode.R))
    //    {
    //        SceneInfo.instance.IsAiming = false;
    //        SceneInfo.instance.IsTakingShot = false;

    //        float power = 250.0f;

    //        Vector3 force = activeCueBall.transform.position - transform.position;
    //        force = new Vector3(force.x, 0.0f, force.z) * power;
    //        activeCueBall.GetComponent<Rigidbody>().AddForce(force, ForceMode.Force);
    //    }

    //    else if (SceneInfo.instance.IsTakingShot && Input.GetKeyDown(KeyCode.T))
    //    {
    //        SceneInfo.instance.IsAiming = false;
    //        SceneInfo.instance.IsTakingShot = false;

    //        float power = maxForce;

    //        Vector3 force = activeCueBall.transform.position - transform.position;
    //        force = new Vector3(force.x, 0.0f, force.z) * power;
    //        activeCueBall.GetComponent<Rigidbody>().AddForce(force, ForceMode.Force);
    //    }
    //}
}
