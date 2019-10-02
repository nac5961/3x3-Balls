using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    //Rigidbody
    public float deceleration;
    public float ballBounceLimit;
    public float floorBounceLimit;

    //Spawning
    private Vector3 eightBallSpawn;
    private Vector3 prevPos;
    private bool inBounds;

    //Scoring
    private bool isScored;

    //Pausing (Rigidbody)
    private bool wasPaused;
    private bool wasMoving;
    private Vector3 pausedVelocity;
    private Vector3 pausedAngularVelocity;

    public Vector3 EightBallSpawn
    {
        set { eightBallSpawn = value; }
    }
    public bool InBounds
    {
        get { return inBounds; }
    }
    public bool IsScored
    {
        get { return isScored; }
    }
    public bool WasMoving
    {
        get { return wasMoving; }
    }

    // Start is called before the first frame update
    void Start()
    {
        prevPos = transform.position;
        isScored = false;

        wasPaused = false;
        wasMoving = false;
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
                else if (SceneInfo.instance.IsHit)
                {
                    StopMoving();
                }
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
        if (gameObject == SceneInfo.instance.TargetBall)
        {
            RaycastHit hitInfo;
            if (Physics.Raycast(respawnPos, Vector3.down, out hitInfo))
            {
                if (!hitInfo.transform.CompareTag("Score Area"))
                {
                    respawnPos = eightBallSpawn;
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
            //Velocity is low enough to forefully stop movement
            if (rb.velocity.magnitude <= 0.01f)
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                rb.Sleep(); //Make sure to have the rigidbody sleep. Rigidbody may not detect that it is done moving with just a zeroed out velocity.
            }

            //Slow down the velocity
            else if (rb.velocity.magnitude <= 1.0f)
            {
                rb.velocity *= deceleration;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Rigidbody rb = GetComponent<Rigidbody>();

        //Limit how high the ball bounces off of certain objects
        if (collision.gameObject.CompareTag("Cue Ball") || collision.gameObject.CompareTag("Eight Ball") || collision.gameObject.CompareTag("Wall"))
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

        //Check if the ball is in the scoring area
        if (collision.gameObject.CompareTag("Score Area"))
        {
            inBounds = true;
        }

        //Check if ball needs to be respawned (Fell off course or was knocked into a hole)
        if (collision.gameObject.CompareTag("Ground") || (collision.gameObject.CompareTag("Hole") && gameObject != SceneInfo.instance.TargetBall))
        {
            //This takes into account bouncing.
            //If the target ball switches when the previous target ball is bouncing
            //in the hole, the previous target ball will respawn.
            if (!isScored)
            {
                Respawn();
            }
        }

        //Check if ball is scored
        else if (collision.gameObject.CompareTag("Hole") && gameObject == SceneInfo.instance.TargetBall)
        {
            //Player scored only the target ball during their turn
            if (SceneInfo.instance.TargetBall != SceneInfo.instance.ActiveBall)
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
            }

            //Player scored both their ball and the target ball so respawn them
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
