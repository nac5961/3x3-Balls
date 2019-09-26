using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EightBall : MonoBehaviour
{
    //MOVE THIS SCRIPT TO THE BALL SCRIPT
    private bool inBounds;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (SceneInfo.instance.GameStart && !SceneInfo.instance.Paused)
        {
            if (SceneInfo.instance.IsTurnOver)
            {
                ReEnterBounds();
            }
        }
    }

    /// <summary>
    /// Respawns the eight ball in-bounds at its previous position.
    /// </summary>
    private void ReEnterBounds()
    {
        if (!inBounds && !GetComponent<Ball>().IsScored && SceneInfo.instance.ActiveBall != SceneInfo.instance.TargetBall)
        {
            transform.position = GetComponent<Ball>().PrevPos;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Hole Area"))
        {
            inBounds = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Hole Area"))
        {
            inBounds = false;
        }
    }
}
