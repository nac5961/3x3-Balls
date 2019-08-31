using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolCue : MonoBehaviour
{

    public GameObject targetBall;
    public Camera view;

    public float hitAngle;
    public float rotationSpeed;

    private float boundsY;

    private Vector3 middleOfScreen;
    private Vector3 startVector;

    // Start is called before the first frame update
    void Start()
    {
        transform.rotation = Quaternion.identity;
        boundsY = GetComponent<MeshRenderer>().bounds.extents.y;


        //middleOfScreen = new Vector3(Screen.width / 2, Screen.height / 2, 0.0f);
        //startVector = middleOfScreen - new Vector3(Screen.width / 2, 0.0f, 0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        RotateByKey();

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
        Bounds ballBounds = targetBall.GetComponent<MeshRenderer>().bounds;

        //Note: This works based on the assumption that the pivot point is at the center of the pool cue.
        float xOffset = boundsY + ballBounds.extents.x;
        float yOffset = ballBounds.extents.y;
        float zOffset = 0.0f;

        //Reset position and angle
        transform.position = targetBall.transform.position + new Vector3(-xOffset, yOffset, zOffset);
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, hitAngle);
    }

    /// <summary>
    /// Rotates the pool cue with keyboard controls
    /// </summary>
    private void RotateByKey()
    {
        //Rotate left
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.RotateAround(targetBall.transform.position, Vector3.up, rotationSpeed * Time.deltaTime);
            view.GetComponent<CameraMovement>().MoveWithPoolCue(transform);
        }

        //Rotate right
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.RotateAround(targetBall.transform.position, Vector3.up, -rotationSpeed * Time.deltaTime);
            view.GetComponent<CameraMovement>().MoveWithPoolCue(transform);
        }
    }

    /// <summary>
    /// Rotates the pool cue with mouse movement
    /// </summary>
    private void RotateByMouse()
    {
        //Vector3 mousePos = GetMousePosition();
        //Vector3 toMouse = mousePos - middleOfScreen;

        ////Get angle from start vector to mouse vector
        //float angle = Mathf.Atan2(toMouse.y - startVector.y, toMouse.x - startVector.x) * Mathf.Rad2Deg * -2.0f;


        //transform.LookAt(targetBall.transform.position, Vector3.up);
        ////transform.rotation = new v

        //Debug.Log(angle);
    }

    private void OnMouseUp()
    {
        Vector3 force = targetBall.transform.position - transform.position;
        force = new Vector3(force.x, 0.0f, force.z) * 400.0f;
        targetBall.GetComponent<Rigidbody>().AddForce(force, ForceMode.Force);
    }

}
