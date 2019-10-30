using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpinType
{
    Normal,
    Top,
    Back,
    Left,
    Right
}

public class Ball : MonoBehaviour
{
    //Rigidbody
    public float deceleration;
    public float ballBounceLimit;
    public float floorBounceLimit;

    private bool startOfHit;
    private bool appliedSpin;
    private bool removedTorque;
    private Vector3 initalVelocity;
    private Vector3 initialAngularVelocity;
    private Vector3 prevVelocity;
    private float minMagnitude;
    private float maxTorque;

    //Spin
    private SpinType spin;

    //Spawning
    private Vector3 targetBallSpawn;
    private Vector3 prevPos;
    private bool inBounds;

    //Scoring
    private bool isScored;
    private int scoreCount;

    //Pausing (Rigidbody)
    private bool wasPaused;
    private bool wasMoving;
    private Vector3 pausedVelocity;
    private Vector3 pausedAngularVelocity;

    public SpinType Spin
    {
        set { spin = value; }
    }
    public Vector3 TargetBallSpawn
    {
        set { targetBallSpawn = value; }
    }
    public bool InBounds
    {
        get { return inBounds; }
    }
    public bool IsScored
    {
        get { return isScored; }
    }
    public int ScoreCount
    {
        get { return scoreCount; }
        set { scoreCount = value; }
    }
    public bool WasMoving
    {
        get { return wasMoving; }
    }

    // Start is called before the first frame update
    void Start()
    {
        startOfHit = false;
        appliedSpin = false;
        removedTorque = false;
        initalVelocity = Vector3.zero;
        initialAngularVelocity = Vector3.zero;
        prevVelocity = Vector3.zero;
        minMagnitude = 35.0f; //min magnitude of velocity needed to achieve full torque
        maxTorque = 200.0f;

        spin = SpinType.Normal;

        prevPos = transform.position;
        isScored = false;
        scoreCount = 0;

        wasPaused = false;
        wasMoving = false;

        GetComponent<Rigidbody>().maxAngularVelocity = 100.0f; //used for spinning the ball (back spin, front spin, etc)
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        if (SceneInfo.instance.GameStart)
        {
            if (SceneInfo.instance.Paused)
            {
                if (!wasPaused)
                {
                    PauseBall();
                }
            }
            else
            {
                if (wasPaused)
                {
                    ResumeBall();
                }
                else if (SceneInfo.instance.IsAiming)
                {
                    ResetSpinData();
                }
                else if (SceneInfo.instance.IsHit)
                {
                    SetSpinData();
                    StopMoving();
                }
            }
        }
    }

    private void ResetSpinData()
    {
        if (startOfHit && gameObject == SceneInfo.instance.ActiveBall)
        {
            startOfHit = false;
            appliedSpin = false;
            removedTorque = false;
            initalVelocity = Vector3.zero;
            initialAngularVelocity = Vector3.zero;
            prevVelocity = Vector3.zero;
        }
    }

    private void SetSpinData()
    {
        if (gameObject == SceneInfo.instance.ActiveBall)
        {
            prevVelocity = GetComponent<Rigidbody>().velocity;

            if (!startOfHit)
            {
                startOfHit = true;
                initalVelocity = GetComponent<Rigidbody>().velocity;
                initialAngularVelocity = GetComponent<Rigidbody>().angularVelocity;
            }
        }
    }

    /// <summary>
    /// Stops the ball from moving on pause.
    /// </summary>
    private void PauseBall()
    {
        wasPaused = true;

        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        if (!rb.IsSleeping())
        {
            wasMoving = true;
            pausedVelocity = rb.velocity;
            pausedAngularVelocity = rb.angularVelocity;
            rb.Sleep();
        }
    }

    /// <summary>
    /// Continues ball movement when the game is resumed.
    /// </summary>
    private void ResumeBall()
    {
        wasPaused = false;

        if (wasMoving)
        {
            wasMoving = false;

            Rigidbody rb = gameObject.GetComponent<Rigidbody>();
            rb.WakeUp();
            rb.AddForce(pausedVelocity, ForceMode.VelocityChange);
            rb.AddTorque(pausedAngularVelocity, ForceMode.VelocityChange);
        }
    }

    /// <summary>
    /// Updates the ball's previous position.
    /// </summary>
    public void UpdatePrevPos()
    {
        prevPos = transform.position;
    }

    /// <summary>
    /// Sets the ball's position to its last saved previous position and stops its movement.
    /// </summary>
    public void Respawn()
    {
        Vector3 respawnPos = prevPos;

        //If somehow a player scores without landing on the scoring area,
        //then we need to make sure that they respawn at that area,
        //instead of at their previous position.
        if (SceneInfo.instance.TargetBalls.Contains(gameObject))
        {
            RaycastHit hitInfo;
            if (Physics.Raycast(respawnPos, Vector3.down, out hitInfo))
            {
                if (!hitInfo.transform.CompareTag("Score Area"))
                {
                    respawnPos = targetBallSpawn;
                }
            }
        }

        //Respawn slightly above ground to have the ball drop when respawned
        transform.position = respawnPos + new Vector3(0.0f, 3.0f, 0.0f);

        //Forcefully stop the velocity when respawned, otherwise it will continue moving
        //with the same velocity.
        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    /// <summary>
    /// Slows down the ball and when slow enough, completely stops the ball.
    /// </summary>
    private void StopMoving()
    {
        Rigidbody rb = gameObject.GetComponent<Rigidbody>();

        if (!rb.IsSleeping())
        {
            //Slow down the velocity
            if (rb.velocity.magnitude <= 0.4f)
            {
                rb.velocity *= deceleration;
            }

            //Slow down the angular velocity
            if (rb.angularVelocity.magnitude <= 0.4f)
            {
                rb.angularVelocity *= deceleration;
            }

            //Velocity is low enough to forefully stop movement
            if (rb.velocity.magnitude <= 0.01f && rb.angularVelocity.magnitude <= 0.01f)
            {
                //Always default to Normal spin for the start of each turn
                if (gameObject == SceneInfo.instance.ActiveBall)
                {
                    spin = SpinType.Normal;
                    UIGameInfo.instance.AimUI.GetComponent<AimUI>().SetCenterHit();
                }

                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                rb.Sleep(); //Make sure to have the rigidbody sleep. Rigidbody may not detect that it is done moving with just a zeroed out velocity.
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Rigidbody rb = GetComponent<Rigidbody>();

        //Apply spin
        if (collision.gameObject.CompareTag("Ball") && gameObject == SceneInfo.instance.ActiveBall)
        {
            if (!appliedSpin)
            {
                appliedSpin = true;

                if (spin != SpinType.Normal)
                {
                    //Calculate torque              
                    Vector3 torque = Vector3.zero;
                    float velocityScale = prevVelocity.magnitude / initalVelocity.magnitude * Mathf.Clamp(prevVelocity.magnitude / minMagnitude, 0.0f, 1.0f);
                    float torqueScale = maxTorque * velocityScale;

                    switch (spin)
                    {
                        case SpinType.Top:
                            torque = initialAngularVelocity.normalized * torqueScale;
                            break;
                        case SpinType.Back:
                            torque = initialAngularVelocity.normalized * -torqueScale;
                            break;
                        case SpinType.Left:
                            torque = Quaternion.Euler(0.0f, -45.0f, 0.0f) * initialAngularVelocity.normalized * torqueScale;
                            break;
                        case SpinType.Right:
                            torque = Quaternion.Euler(0.0f, 45.0f, 0.0f) * initialAngularVelocity.normalized * torqueScale;
                            break;
                        default:
                            break;
                    }

                    //Apply torque
                    GetComponent<Rigidbody>().AddTorque(torque, ForceMode.VelocityChange);
                }
            }
        }
        else if (collision.gameObject.CompareTag("Wall") && gameObject == SceneInfo.instance.ActiveBall)
        {
            //If the ball hits a wall first, we want to not have it apply torque
            //on ball collision, since the torque is lost when the ball collides
            //with the wall.
            if (!appliedSpin)
            {
                appliedSpin = true;
                removedTorque = true; //set to true so we don't enter the if-statement below
            }

            //If spin is already applied, the ball will lose its spin when it
            //hits the wall. So, zero out its spin to have it spin normally.
            else
            {
                if (!removedTorque)
                {
                    removedTorque = true;
                    GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                }
            }
        }

        //Limit how high the ball bounces off of certain objects
        if (collision.gameObject.CompareTag("Ball") || collision.gameObject.CompareTag("Wall"))
        {
            if (rb.velocity.y > ballBounceLimit)
            {
                rb.velocity = new Vector3(rb.velocity.x, ballBounceLimit, rb.velocity.z);
            }
        }
        else if (collision.gameObject.CompareTag("Floor"))
        {
            if (rb.velocity.y > floorBounceLimit)
            {
                rb.velocity = new Vector3(rb.velocity.x, floorBounceLimit, rb.velocity.z);
            }
        }
        else if (collision.gameObject.CompareTag("Bumper"))
        {
            if (rb.velocity.y > 0.5f)
            {
                rb.velocity = new Vector3(rb.velocity.x, 0.5f, rb.velocity.z);
            }
        }

        //Check if the ball is in the scoring area
        if (collision.gameObject.CompareTag("Score Area"))
        {
            inBounds = true;
        }

        //Check if ball needs to be respawned (Fell off course or was knocked into a hole)
        if (collision.gameObject.CompareTag("Ground") || (collision.gameObject.CompareTag("Hole") && !SceneInfo.instance.TargetBalls.Contains(gameObject)))
        {
            //This takes into account bouncing.
            //If the target ball switches when the previous target ball is bouncing
            //in the hole, the previous target ball will respawn.
            // **The check in the if-statement was needed when TargetBalls was not a list**
            if (!isScored)
            {
                Respawn();
            }
        }

        //Check if ball is scored
        else if (collision.gameObject.CompareTag("Hole") && SceneInfo.instance.TargetBalls.Contains(gameObject))
        {
            //Player scored only target balls during their turn
            if (gameObject != SceneInfo.instance.ActiveBall)
            {
                isScored = true;

                //Stop colliding with other stuff and make invisible
                gameObject.GetComponent<SphereCollider>().isTrigger = true;
                gameObject.GetComponent<MeshRenderer>().enabled = false;

                //Stop moving
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                rb.Sleep();

                //Setup next target
                SceneInfo.instance.SwitchTargetBall();

                SceneInfo.instance.ActiveBall.GetComponent<Ball>().ScoreCount++;

                if (SceneInfo.instance.ActiveBall.GetComponent<Ball>().ScoreCount > 1)
                {
                    SceneInfo.instance.AddBonusScore(1);
                    UIGameInfo.instance.GeneralUI.GetComponent<GeneralUI>().SetStrokeCount();
                }

                if (gameObject.name.Contains("Special"))
                {
                    SceneInfo.instance.AddBonusScore(3);
                    UIGameInfo.instance.GeneralUI.GetComponent<GeneralUI>().SetStrokeCount();
                }
            }

            //Player scored both their ball and target balls so respawn them
            else
            {
                Respawn();
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        //Check if the ball left the scoring area
        if (collision.gameObject.CompareTag("Score Area"))
        {
            inBounds = false;
        }
    }
}
