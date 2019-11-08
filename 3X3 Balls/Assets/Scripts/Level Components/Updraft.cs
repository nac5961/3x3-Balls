using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Updraft : MonoBehaviour
{
    private Vector3 upwardsForce;

    // Start is called before the first frame update
    void Start()
    {
        upwardsForce = new Vector3(0.0f, 15.0f, 0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Ball"))
        {
            other.gameObject.GetComponent<Rigidbody>().AddForce(upwardsForce);

            Vector3 forwardForce = other.gameObject.GetComponent<Rigidbody>().velocity.normalized;
            forwardForce = new Vector3(forwardForce.x, 0.0f, forwardForce.z) * 3.0f;
            other.gameObject.GetComponent<Rigidbody>().AddForce(forwardForce);
        }
    }
}
