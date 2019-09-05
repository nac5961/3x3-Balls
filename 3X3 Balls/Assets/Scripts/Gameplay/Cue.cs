using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cue : MonoBehaviour
{
    public float hitAngle;
    public float rotationSpeed;
    public float maxForce;

    private GameObject p1CueBall;
    private GameObject p2CueBall;
    private GameObject activeCueBall;
    private AimDisplay aimUI;
    private float boundsY;

    public GameObject P1CueBall
    {
        set { p1CueBall = value; }
    }

    public GameObject P2CueBall
    {
        set { p2CueBall = value; }
    }

    public GameObject ActiveCueBall
    {
        get { return activeCueBall; }
    }

    // Start is called before the first frame update
    void Start()
    {
        aimUI = GameObject.Find("Canvas").GetComponent<AimDisplay>();
        boundsY = GetComponent<MeshRenderer>().bounds.extents.y;

        AlignWithBall();
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneInfo.instance.IsAiming && !SceneInfo.instance.DisableControls)
        {
            RotateByKey();
            HitBall();
        }
    }

    /// <summary>
    /// Aligns the pool cue with the ball based on the size of the cue and the ball
    /// </summary>
    public void AlignWithBall()
    {
        SceneInfo.instance.IsAiming = true;

        activeCueBall = SceneInfo.instance.Turn == GameInfo.instance.P1Type ? p1CueBall : p2CueBall;

        Bounds ballBounds = activeCueBall.GetComponent<MeshRenderer>().bounds;

        //Note: This works based on the assumption that the pivot point is at the center of the pool cue.
        float xOffset = boundsY + ballBounds.extents.x;
        float yOffset = ballBounds.extents.y;
        float zOffset = 0.0f;

        //Reset position and angle
        transform.position = activeCueBall.transform.position + new Vector3(-xOffset, yOffset, zOffset);
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, hitAngle);

        Camera.main.GetComponent<CameraMovement>().Aim();
    }

    /// <summary>
    /// Rotates the pool cue with keyboard controls
    /// </summary>
    private void RotateByKey()
    {
        if (!SceneInfo.instance.IsTakingShot)
        {
            //Rotate left
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                transform.RotateAround(activeCueBall.transform.position, Vector3.up, -rotationSpeed * Time.deltaTime);
                Camera.main.GetComponent<CameraMovement>().Aim();
            }

            //Rotate right
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                transform.RotateAround(activeCueBall.transform.position, Vector3.up, rotationSpeed * Time.deltaTime);
                Camera.main.GetComponent<CameraMovement>().Aim();
            }
        }
    }

    private void HitBall()
    {
        if (!SceneInfo.instance.IsTakingShot && Input.GetKeyDown(KeyCode.Space))
        {
            SceneInfo.instance.IsTakingShot = true;
        }
        else if (SceneInfo.instance.IsTakingShot && Input.GetKeyDown(KeyCode.Backspace))
        {
            SceneInfo.instance.IsTakingShot = false;
        }
        else if (SceneInfo.instance.IsTakingShot && Input.GetKeyDown(KeyCode.Space))
        {
            SceneInfo.instance.IsAiming = false;
            SceneInfo.instance.IsTakingShot = false;

            float power = maxForce * aimUI.GetFillAmount();

            Vector3 force = activeCueBall.transform.position - transform.position;
            force = new Vector3(force.x, 0.0f, force.z) * power;
            activeCueBall.GetComponent<Rigidbody>().AddForce(force, ForceMode.Force);
        }
    }
}
