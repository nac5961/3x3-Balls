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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Pool Collider")
        {
            onPoolTable = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Pool Collider")
        {
            onPoolTable = false;
        }
    }
}
