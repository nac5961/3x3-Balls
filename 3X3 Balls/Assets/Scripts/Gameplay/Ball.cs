using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public Vector3 prevPos;

    private bool updatePos;
    protected bool needsToRespawn;
    private float waitTime = 2.0f;
    private float timer = 0.0f;

    private bool scored;

    // Start is called before the first frame update
    void Start()
    {
        prevPos = transform.position;
        needsToRespawn = false;
        scored = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneInfo.instance.GameStart && !SceneInfo.instance.Paused)
        {
            if (SceneInfo.instance.IsAiming)
            {
                UpdatePrevPos();
            }
            if (SceneInfo.instance.IsTurnOver)
            {
                timer = 0.0f;
            }
        }
    }

    void FixedUpdate()
    {
        if (SceneInfo.instance.GameStart && !SceneInfo.instance.Paused)
        {
            if (SceneInfo.instance.IsHit)
            {
                timer += Time.deltaTime;

                if (timer >= waitTime)
                {
                    StopMoving();
                }              
            }
        }
    }

    private void UpdatePrevPos()
    {
        prevPos = transform.position;
    }

    private void Respawn()
    {
        transform.position = prevPos;

        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.Sleep();

        needsToRespawn = false;
    }

    private void StopMoving()
    {
        Rigidbody rb = gameObject.GetComponent<Rigidbody>();

        if (rb.velocity.magnitude != 0)
        {
            if (rb.velocity.magnitude <= 0.1f)
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                rb.Sleep();
                Debug.Log("stop");
            }
            else if (rb.velocity.magnitude <= 1.0f)
            {
                Debug.Log("slowing");
                rb.velocity *= 0.9f;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") || (collision.gameObject.CompareTag("Hole") && gameObject != SceneInfo.instance.TargetBall))
        {
            if (!scored)
            {
                Respawn();
            }
            
            //needsToRespawn = true;
        }
        else if (collision.gameObject.CompareTag("Hole") && gameObject == SceneInfo.instance.TargetBall)
        {
            gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            gameObject.GetComponent<Rigidbody>().Sleep();

            scored = true;
            SceneInfo.instance.SwitchTargetBall();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") || (collision.gameObject.CompareTag("Hole") && gameObject != SceneInfo.instance.TargetBall))
        {
            needsToRespawn = false;
        }
    }
}
