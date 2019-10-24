using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableWall : MonoBehaviour
{
    public bool canBreak;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            if (canBreak && collision.gameObject.GetComponent<Rigidbody>().velocity.magnitude > 0.3f)
            {
                gameObject.SetActive(false);
            }
            else
            {
                if (collision.gameObject.name.Contains("Boulder") && collision.gameObject.GetComponent<Rigidbody>().velocity.magnitude > 0.3f)
                {
                    gameObject.SetActive(false);
                }
            }
        }
    }
}
