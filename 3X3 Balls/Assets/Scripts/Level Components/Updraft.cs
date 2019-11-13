using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Updraft : MonoBehaviour
{
    public float force;
    private Vector3 upwardsForce;

    // Start is called before the first frame update
    void Start()
    {
        upwardsForce = new Vector3(0.0f, force, 0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (SceneInfo.instance.GameStart && !SceneInfo.instance.Paused)
        {
            //Only affect ball
            if (other.gameObject.CompareTag("Ball"))
            {
                //Apply upwards force
                other.gameObject.GetComponent<Rigidbody>().AddForce(upwardsForce);

                //Add forwards force to constantly move the ball forwards
                Vector3 forwardForce = other.gameObject.GetComponent<Rigidbody>().velocity.normalized;
                forwardForce = new Vector3(forwardForce.x, 0.0f, forwardForce.z) * 3.0f;
                other.gameObject.GetComponent<Rigidbody>().AddForce(forwardForce);
            }
        }
    }
}
