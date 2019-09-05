using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hole : MonoBehaviour
{
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
        if (!SceneInfo.instance.IsRoundOver)
        {
            if (collision.gameObject.CompareTag("Solid") || collision.gameObject.CompareTag("Striped"))
            {
                SceneInfo.instance.IsRoundOver = true;
                GameInfo.instance.CapturedBalls.Add(collision.gameObject.name);
            }
            else if (collision.gameObject.CompareTag("Cue Ball"))
            {
                collision.gameObject.GetComponent<CueBall>().Respawn(true);
            }
        }
    }
}
