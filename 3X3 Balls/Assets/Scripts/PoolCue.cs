using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PoolCue : MonoBehaviour
{
    public float hitAngle;
    public float rotationSpeed;
    public float hitMeterSpeed;

    private GameObject cueBall;
    private GameObject hitMeter;
    private float hitMeterPerecent;

    private float boundsY;

    public GameObject CueBall
    {
        get { return cueBall; }
        set { cueBall = value; }
    }

    public GameObject HitMeter
    {
        get { return hitMeter; }
        set { hitMeter = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        transform.rotation = Quaternion.identity;
        boundsY = GetComponent<MeshRenderer>().bounds.extents.y;

        AlignWithBall();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameInfo.instance.Reset)
        {
            GameInfo.instance.Reset = false;
            AlignWithBall();
        }

        if (!GameInfo.instance.PostHit)
        {
            RotateByKey();
            HitBall();
        }
        
        if (GameInfo.instance.FirstHit)
        {
            MoveCueBall();
        }

        //Debug
        if (Input.GetKeyDown(KeyCode.P))
        {
            AlignWithBall();
        }
    }

    /// <summary>
    /// Gets mouse position in screen coordinates
    /// </summary>
    private Vector3 GetMousePosition()
    {
        return Input.mousePosition;
    }

    /// <summary>
    /// Aligns the pool cue with the ball based on the size of the cue and the ball
    /// </summary>
    private void AlignWithBall()
    {
        Camera.main.GetComponent<CameraMovement>().IsAiming = true;

        transform.parent = cueBall.transform;

        Bounds ballBounds = cueBall.GetComponent<MeshRenderer>().bounds;

        //Note: This works based on the assumption that the pivot point is at the center of the pool cue.
        float xOffset = boundsY + ballBounds.extents.x;
        float yOffset = ballBounds.extents.y;
        float zOffset = 0.0f;

        //Reset position and angle
        transform.position = cueBall.transform.position + new Vector3(-xOffset, yOffset, zOffset);
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, hitAngle);
    }

    private void MoveWithBall()
    {
        
    }

    private void MoveCueBall()
    {
        float moveSpeed = 3.0f;

        //Move left
        if (Input.GetKey(KeyCode.A))
        {
            cueBall.transform.position += new Vector3(-moveSpeed * Time.deltaTime, 0.0f, 0.0f);
        }

        //Move right
        else if (Input.GetKey(KeyCode.D))
        {
            cueBall.transform.position += new Vector3(moveSpeed * Time.deltaTime, 0.0f, 0.0f);
        }
    }

    /// <summary>
    /// Rotates the pool cue with keyboard controls
    /// </summary>
    private void RotateByKey()
    {
        //Rotate left
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.RotateAround(cueBall.transform.position, Vector3.up, rotationSpeed * Time.deltaTime);
        }

        //Rotate right
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.RotateAround(cueBall.transform.position, Vector3.up, -rotationSpeed * Time.deltaTime);
        }
    }

    /// <summary>
    /// Rotates the pool cue with mouse movement
    /// </summary>
    private void RotateByMouse()
    {
        
    }

    private void HitBall()
    {
        if (GameInfo.instance.PreHit)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                GameInfo.instance.PreHit = false;
                hitMeter.SetActive(true);
                hitMeterPerecent = 0.0f;
                hitMeterSpeed = Mathf.Abs(hitMeterSpeed);
                hitMeter.GetComponent<Image>().fillAmount = 0;
            }
        }
        else
        {

            if (!GameInfo.instance.PostHit)
            {
                hitMeterPerecent += hitMeterSpeed * Time.deltaTime;
                hitMeterPerecent = Mathf.Clamp(hitMeterPerecent, 0.0f, 1.0f);
                hitMeter.GetComponent<Image>().fillAmount = hitMeterPerecent;

                if (hitMeterPerecent >= 1.0f || hitMeterPerecent <= 0.0f)
                {
                    hitMeterSpeed = -hitMeterSpeed;
                }

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    if (GameInfo.instance.FirstHit)
                    {
                        GameInfo.instance.FirstHit = false;
                        cueBall.GetComponent<Rigidbody>().useGravity = true;
                    }

                    float currForce = 300.0f;
                    currForce *= hitMeter.GetComponent<Image>().fillAmount;

                    Vector3 force = cueBall.transform.position - transform.position;
                    force = new Vector3(force.x, 0.0f, force.z) * currForce;
                    cueBall.GetComponent<Rigidbody>().AddForce(force, ForceMode.Force);
                    Camera.main.GetComponent<CameraMovement>().IsAiming = false;
                    GameInfo.instance.PostHit = true;

                    transform.parent = null;
                    hitMeter.SetActive(false);
                }
            }
        }
    }
}
