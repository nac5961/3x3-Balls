using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    //Position
    private Transform target;
    private Vector3 desiredPos;
    private Vector3 adjDesiredPos;

    private float distance;
    private float adjDistance;
    private float maxDistance;
    private float minDistance;
    private float zoomSpeed;
    private float smoothTime;

    //Rotation
    private float xRot;
    private float yRot;
    private float maxXRot;
    private float minXRot;
    private float rotationSpeed;

    //Collision
    private LayerMask mask;
    private bool isColliding;
    private Vector3[] clipPoints;

    //Player
    private List<float> playerRotations;

    //Views
    private GameObject levelOverview;
    private GameObject scoreAreaOverview;
    private bool locked;
    private bool autoSwitch;
    private Vector3 playerOverhead;
    private Quaternion playerOverheadRotation;

    public Transform Target
    {
        set { target = value; }
    }
    public GameObject LevelOverview
    {
        set { levelOverview = value; }
    }
    public GameObject ScoreAreaOverview
    {
        set { scoreAreaOverview = value; }
    }
    public bool Locked
    {
        get { return locked; }
    }

    // Start is called before the first frame update
    void Start()
    {
        //Position
        target = SceneInfo.instance.ActiveBall.transform;
        distance = -11.0f;
        adjDistance = distance;
        maxDistance = -11.0f;
        minDistance = -15.0f;
        zoomSpeed = 400.0f;
        smoothTime = 0.05f;

        //Rotation
        xRot = -25.0f; //Sets start rotation
        yRot = -88.0f; //Sets start rotation
        maxXRot = -25.0f; //Change to 25.0f or similar to allow camera to get close to target
        minXRot = -80.0f;
        rotationSpeed = 60.0f;

        //Collision
        mask =~ LayerMask.GetMask("IgnoreCam");
        isColliding = false;
        clipPoints = new Vector3[5];

        //Player
        playerRotations = new List<float>();
        for (int i = 0; i < GameInfo.instance.Players; i++)
        {
            playerRotations.Add(yRot);
        }

        //Views
        locked = false;
        autoSwitch = false;
        playerOverhead = new Vector3(0.0f, 24.87f, 0.0f);
        playerOverheadRotation = Quaternion.Euler(90.0f, 0.0f, 0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneInfo.instance.GameStart && !SceneInfo.instance.Paused && !SceneInfo.instance.DisableControls)
        {
            //Check to see if view needs to automatically switch to the
            //score area overview after a hit
            if (SceneInfo.instance.IsHit && !autoSwitch)
            {
                AutomaticallySwitchView();
            }

            //Check to see if the player is trying to manually switch their view.
            //But make sure they cannot do that if they're taking a shot, or the
            //view is automatically switched to the score area.
            if (!SceneInfo.instance.IsTakingShot && !autoSwitch)
            {
                ManuallySwitchView();
            }

            //Rotate the camera around the ball as long as the view is not locked
            //to another view.
            //Also do not allow rotation during a shot.
            if (!SceneInfo.instance.IsTakingShot && !locked)
            {
                RotateAroundTarget();
            }

            //Zoom();
        }
    }

    void FixedUpdate()
    {
        if (SceneInfo.instance.GameStart && !SceneInfo.instance.Paused)
        {
            //Follow the ball unless the view is locked to another view
            if (!locked)
            {
                FollowTarget();
                LookAtTarget();
            }

            //SetClipPoints();
            //SetAdjustedDistance();
        }
    }

    /// <summary>
    /// Automatically switches the camera view to the score area overview.
    /// </summary>
    private void AutomaticallySwitchView()
    {
        //If the ball is in the score area and is moving at a slow speed
        if (SceneInfo.instance.ActiveBall.GetComponent<Ball>().InBounds && SceneInfo.instance.ActiveBall.GetComponent<Rigidbody>().velocity.magnitude < 2.0f)
        {
            autoSwitch = true;
            locked = true;

            UIGameInfo.instance.HideAllUI();

            Camera.main.transform.position = scoreAreaOverview.transform.position;
            Camera.main.transform.rotation = scoreAreaOverview.transform.rotation;
        }
    }

    /// <summary>
    /// Switches the camera view to another view based on player input.
    /// </summary>
    private void ManuallySwitchView()
    {
        //Show the player overview
        if (Input.GetButton("CameraViewOverhead") && Input.GetButton("CameraViewLevel"))
        {
            if (!locked)
            {
                UIGameInfo.instance.DisplayCamViewUI();
            }

            locked = true;
            Camera.main.transform.position = SceneInfo.instance.ActiveBall.transform.position + playerOverhead;
            Camera.main.transform.rotation = playerOverheadRotation;
        }

        //Show the level overview
        else if (Input.GetButton("CameraViewLevel"))
        {
            if (!locked)
            {
                RenderSettings.fog = false; //hide fog
                UIGameInfo.instance.DisplayCamViewUI();
            }

            locked = true;
            Camera.main.transform.position = levelOverview.transform.position;
            Camera.main.transform.rotation = levelOverview.transform.rotation;
        }

        //Stop the manual switch
        else
        {
            if (locked)
            {
                RenderSettings.fog = true; //show fog again
                UIGameInfo.instance.HideCamViewUI();
            }

            locked = false;
        }
    }

    /// <summary>
    /// Keeps track of the previous player's rotation and sets the rotation
    /// to the next player's rotation.
    /// </summary>
    /// <param name="prevPlayer">previous player's number (Player 0, 1, 2, etc); Note: Player 1 is Player 0</param>
    /// <param name="nextPlayer">next player's number (Player 0, 1, 2, etc); Note: Player 1 is Player 0</param>
    public void UpdatePlayerRotations(int prevPlayer, int nextPlayer)
    {
        //Save previous player's rotation
        playerRotations[prevPlayer] = yRot;

        //Set rotation to next player's rotation
        yRot = playerRotations[nextPlayer];

        //Reset x rotation to ideal rotation
        xRot = maxXRot;

        autoSwitch = false;
        locked = false;
    }

    /// <summary>
    /// Smoothly follows the target. Adjusts its position based on information
    /// gathered from any blocking objects.
    /// </summary>
    private void FollowTarget()
    {
        Vector3 velocity = Vector3.zero;

        //Calculate the offset to place the camera (taking into account rotation
        //and distance)
        desiredPos = Quaternion.Euler(xRot, yRot, 0.0f) * -Vector3.forward * distance;
        desiredPos += target.position;

        if (isColliding)
        {
            //Calculate the adjusted offset to place the camera (taking into account rotation
            //and the adjusted distance)
            adjDesiredPos = Quaternion.Euler(xRot, yRot, 0.0f) * Vector3.forward * adjDistance;
            adjDesiredPos += target.position;

            transform.position = Vector3.SmoothDamp(transform.position, adjDesiredPos, ref velocity, smoothTime * Time.deltaTime);
        }
        else
        {
            transform.position = Vector3.SmoothDamp(transform.position, desiredPos, ref velocity, smoothTime * Time.deltaTime);
        }
    }

    /// <summary>
    /// Zooms in and out from the target.
    /// </summary>
    //private void Zoom()
    //{
    //    distance += Input.GetAxis("Mouse ScrollWheel") * zoomSpeed * Time.deltaTime;
    //    distance = Mathf.Clamp(distance, minDistance, maxDistance);
    //}

    /// <summary>
    /// Rotates the camera around the target.
    /// </summary>
    private void RotateAroundTarget()
    {
        xRot += -Input.GetAxis("Vertical") * rotationSpeed * Time.deltaTime;
        yRot += Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime;

        xRot = Mathf.Clamp(xRot, minXRot, maxXRot);
    }

    /// <summary>
    /// Rotates the camera towards the target.
    /// </summary>
    private void LookAtTarget()
    {
        //Note: This is smoother than transform.LookAt()
        Quaternion targetRotation = Quaternion.LookRotation(target.position - transform.position);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 1.0f);
    }

    /// <summary>
    /// Sets the clip points of the camera (Outer edges and middle of camera).
    /// </summary>
    //private void SetClipPoints()
    //{
    //    //Find coordinates for clip points relative to the camera's position
    //    float x = Mathf.Tan(Camera.main.fieldOfView / 3.4f);
    //    float y = x / Camera.main.aspect;
    //    float z = Camera.main.nearClipPlane;
    //    x *= z;

    //    //Find the top left clip point (Rotate relative to camera)
    //    Vector3 topLeft = (Camera.main.transform.rotation * new Vector3(-x, y, z)) + desiredPos;

    //    //Find the top right clip point (Rotate relative to camera)
    //    Vector3 topRight = (Camera.main.transform.rotation * new Vector3(x, y, z)) + desiredPos;

    //    //Find the bottom left clip point (Rotate relative to camera)
    //    Vector3 bottomLeft = (Camera.main.transform.rotation * new Vector3(-x, -y, z)) + desiredPos;

    //    //Find the bottom right clip point (Rotate relative to camera)
    //    Vector3 bottomRight = (Camera.main.transform.rotation * new Vector3(x, -y, z)) + desiredPos;

    //    //Find the clip point slightly behind the camera
    //    Vector3 middle = Camera.main.transform.position - Camera.main.transform.forward;

    //    //Set clip points
    //    clipPoints[0] = topLeft;
    //    clipPoints[1] = topRight;
    //    clipPoints[2] = bottomLeft;
    //    clipPoints[3] = bottomRight;
    //    clipPoints[4] = middle;
    //}

    /// <summary>
    /// Finds the closest distance from the target to any objects 
    /// in-between the camera and target.
    /// This allows the camera to be repositioned in front of any blocking
    /// objects.
    /// </summary>
    //private void SetAdjustedDistance()
    //{
    //    adjDistance = -1.0f;

    //    for (int i = 0; i < clipPoints.Length; i++)
    //    {
    //        RaycastHit hitInfo;
    //        if (Physics.Raycast(target.position, (clipPoints[i] - target.position).normalized, out hitInfo, Vector3.Distance(clipPoints[i], target.position), mask))
    //        {
    //            //Find shortest distance to blocking object
    //            if (adjDistance == -1.0f || hitInfo.distance < adjDistance)
    //            {
    //                adjDistance = hitInfo.distance;
    //            }
    //        }
    //    }

    //    if (adjDistance == -1.0f)
    //    {
    //        isColliding = false;
    //        adjDistance =  0.0f;
    //    }
    //    else
    //    {
    //        isColliding = true;
    //    }
    //}
}
