using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PoolCue : MonoBehaviour
{
    public float hitAngle;
    public float rotationSpeed;
    public float maxForce;

    private GameObject cueBall;
    private AimUI aimUI;
    private float boundsY;

    public GameObject CueBall
    {
        get { return cueBall; }
        set { cueBall = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        aimUI = GameObject.Find("UIManager").GetComponent<AimUI>();
        boundsY = GetComponent<MeshRenderer>().bounds.extents.y;

        //Child cue to ball, to allow cue to follow ball during first hit movement
        transform.parent = cueBall.transform;

        AlignWithBall();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameInfo.instance.IsAiming)
        {
            RotateByKey();
            HitBall();
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
    public void AlignWithBall()
    {
        GameInfo.instance.IsAiming = true;

        Bounds ballBounds = cueBall.GetComponent<MeshRenderer>().bounds;

        //Note: This works based on the assumption that the pivot point is at the center of the pool cue.
        float xOffset = boundsY + ballBounds.extents.x;
        float yOffset = ballBounds.extents.y;
        float zOffset = 0.0f;

        //Reset position and angle
        transform.position = cueBall.transform.position + new Vector3(-xOffset, yOffset, zOffset);
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, hitAngle);

        Camera.main.GetComponent<CameraMovement>().Aim();
    }

    /// <summary>
    /// Rotates the pool cue with keyboard controls
    /// </summary>
    private void RotateByKey()
    {
        if (!GameInfo.instance.IsTakingShot)
        {
            //Rotate left
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                transform.RotateAround(cueBall.transform.position, Vector3.up, rotationSpeed * Time.deltaTime);
                Camera.main.GetComponent<CameraMovement>().Aim();
            }

            //Rotate right
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                transform.RotateAround(cueBall.transform.position, Vector3.up, -rotationSpeed * Time.deltaTime);
                Camera.main.GetComponent<CameraMovement>().Aim();
            }
        }
    }

    private void HitBall()
    {
        //Toggle animation
        if (!GameInfo.instance.IsTakingShot && Input.GetKeyDown(KeyCode.Space))
        {
            GameInfo.instance.IsTakingShot = true;

            //Reset meter
            aimUI.animationSpeed = Mathf.Abs(aimUI.animationSpeed);
            aimUI.powerMeter.GetComponent<Image>().fillAmount = 0.0f;
        }
        else if (GameInfo.instance.IsTakingShot && Input.GetKeyDown(KeyCode.Backspace))
        {
            GameInfo.instance.IsTakingShot = false;

            //Reset meter
            aimUI.powerMeter.GetComponent<Image>().fillAmount = 0.0f;
        }

        else if (GameInfo.instance.IsTakingShot && Input.GetKeyDown(KeyCode.Space))
        {
            GameInfo.instance.IsAiming = false;
            GameInfo.instance.IsTakingShot = false;

            if (GameInfo.instance.FirstHit)
            {
                GameInfo.instance.FirstHit = false;
                cueBall.GetComponent<Rigidbody>().useGravity = true;
                transform.parent = null; //Unchild after first hit since the cue doesn't need to move with the ball anymore
            }

            float power = maxForce * aimUI.powerMeter.GetComponent<Image>().fillAmount;

            Vector3 force = cueBall.transform.position - transform.position;
            force = new Vector3(force.x, 0.0f, force.z) * power;
            cueBall.GetComponent<Rigidbody>().AddForce(force, ForceMode.Force);

            aimUI.powerMeter.GetComponent<Image>().fillAmount = 0.0f;

            Camera.main.GetComponent<CameraMovement>().ShowOverview();
        }
    }
}
