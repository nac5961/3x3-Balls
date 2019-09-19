using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CueBall : MonoBehaviour
{
    public float deadzone;

    private Vector3 respawnPos;
    private bool onPoolTable;

    public Vector3 RespawnPos
    {
        set { respawnPos = value; }
    }

    public bool OnPoolTable
    {
        get { return onPoolTable; }
    }

    // Start is called before the first frame update
    void Start()
    {
        onPoolTable = false;   
    }

    // Update is called once per frame
    void Update()
    {
        Respawn(false);
        //MoveLeftOrRight();
        //Jump();
    }

    public void Respawn(bool forced)
    {
        if (forced)
        {
            transform.position = respawnPos;
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        }
        else
        {
            if (transform.position.y <= deadzone)
            {
                transform.position = respawnPos;
                GetComponent<Rigidbody>().velocity = Vector3.zero;
                GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            }
        }
    }

    private void MoveLeftOrRight()
    {
        if (!SceneInfo.instance.IsAiming && !SceneInfo.instance.IsTurnOver)
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                GetComponent<Rigidbody>().AddTorque(transform.right * -3.0f);
                Debug.Log("turning left");
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                GetComponent<Rigidbody>().AddTorque(transform.right * 3.0f);
                Debug.Log("turning right");
            }
        }
    }

    private void Jump()
    {
        if (!SceneInfo.instance.IsAiming && !SceneInfo.instance.IsTurnOver)
        {
            if (Physics.Raycast(transform.position, -Vector3.up, 1.0f) && GetComponent<Rigidbody>().velocity.y == 0)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    GetComponent<Rigidbody>().AddForce(transform.up * 200.0f);
                    Debug.Log("jumped");
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pool Collider"))
        {
            onPoolTable = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Pool Collider"))
        {
            onPoolTable = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Respawn(true);
        }
        else if (collision.gameObject.CompareTag("Hole"))
        {
            if (!SceneInfo.instance.IsRoundOver)
            {
                Respawn(true);
            }
        }
    }
}
