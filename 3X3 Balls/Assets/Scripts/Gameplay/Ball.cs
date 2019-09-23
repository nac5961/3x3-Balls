using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public Vector3 prevPos;

    private bool updatePos;
    protected bool needsToRespawn;

    // Start is called before the first frame update
    void Start()
    {
        prevPos = transform.position;
        needsToRespawn = false;
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
            //if (SceneInfo.instance.IsTurnOver)
            //{
            //    if (needsToRespawn)
            //    {
            //        Respawn();
            //    }

            //    UpdatePrevPos();
            //}
        }
    }

    private void UpdatePrevPos()
    {
        prevPos = transform.position;
    }

    private void Respawn()
    {
        transform.position = prevPos;

        needsToRespawn = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") || (collision.gameObject.CompareTag("Hole") && gameObject != SceneInfo.instance.TargetBall))
        {
            Respawn();
            //needsToRespawn = true;
        }
        else if (collision.gameObject.CompareTag("Hole") && gameObject == SceneInfo.instance.TargetBall)
        {
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
