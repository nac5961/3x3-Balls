using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public float deceleration;

    private Vector3 prevPos;
    private bool isScored;

    private bool wasPaused;
    private bool wasMoving;
    private Vector3 pausedVelocity;
    private Vector3 pausedAngularVelocity;

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
    public void PauseBall()
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
    public void ResumeBall()
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
    private void Respawn()
    {
        transform.position = prevPos;

        //Forcefully stop the ball when respawned, otherwise it will continue moving
        //with the same velocity.
        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.Sleep(); //Make sure to have the rigidbody sleep. Rigidbody may not detect that it is done moving with just a zeroed out velocity.
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
            if (rb.velocity.magnitude <= 0.1f)
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
        if (collision.gameObject.CompareTag("Ground") || (collision.gameObject.CompareTag("Hole") && gameObject != SceneInfo.instance.TargetBall))
        {
            if (!isScored)
            {
                Respawn();
            }
        }
        else if (collision.gameObject.CompareTag("Hole") && gameObject == SceneInfo.instance.TargetBall)
        {
            //Player scored only the target ball during their turn
            if (SceneInfo.instance.ActiveBall != SceneInfo.instance.TargetBall)
            {
                isScored = true;

                //Stop colliding with other stuff and make invisible
                gameObject.GetComponent<SphereCollider>().isTrigger = true;
                gameObject.GetComponent<MeshRenderer>().enabled = false;

                //Stop moving
                Rigidbody rb = gameObject.GetComponent<Rigidbody>();
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                rb.Sleep();

                SceneInfo.instance.SwitchTargetBall();
            }

            //Player scored both their ball and the target ball so respawn them
            else
            {
                if (!isScored)
                {
                    Respawn();
                }
            }
        }
    }
}
