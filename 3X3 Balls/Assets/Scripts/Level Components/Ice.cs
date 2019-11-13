using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ice : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionStay(Collision collision)
    {
        if (SceneInfo.instance.GameStart && !SceneInfo.instance.Paused)
        {
            //Only affect ball
            if (collision.gameObject.CompareTag("Ball"))
            {
                //Add forwards force to constantly move the ball forwards
                Vector3 forwardForce = collision.gameObject.GetComponent<Rigidbody>().velocity.normalized;
                forwardForce = new Vector3(forwardForce.x, 0.0f, forwardForce.z) * 0.7f; //0.7f allows ball to move slow enough with noticeably speeding up
                collision.gameObject.GetComponent<Rigidbody>().AddForce(forwardForce);
            }
        }
    }
}
