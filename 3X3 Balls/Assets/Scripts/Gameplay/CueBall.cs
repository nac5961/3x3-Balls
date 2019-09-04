using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CueBall : MonoBehaviour
{
    public float moveSpeed;

    private Vector3 firstHitLeftPos;
    private Vector3 firstHitRightPos;
    private float percent;

    public Vector3 FirstHitLeftPos
    {
        get { return firstHitLeftPos; }
        set { firstHitLeftPos = value; }
    }

    public Vector3 FirstHitRightPos
    {
        get { return firstHitRightPos; }
        set { firstHitRightPos = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        percent = 0.5f;

        //Ensure the ball starts at the initialized percent
        // * This will override the spawn position *
        Vector3 newPosition = Vector3.Lerp(firstHitLeftPos, firstHitRightPos, percent);
        transform.position = new Vector3(newPosition.x, transform.position.y, newPosition.z);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameInfo.instance.FirstHit)
        {
            FirstHitMovement();
        }
    }

    /// <summary>
    /// Allows movement of the cue ball before it is hit
    /// </summary>
    private void FirstHitMovement()
    {
        //Move left
        if (Input.GetKey(KeyCode.A))
        {
            percent -= moveSpeed * Time.deltaTime;
        }

        //Move right
        else if (Input.GetKey(KeyCode.D))
        {
            percent += moveSpeed * Time.deltaTime;
        }

        percent = Mathf.Clamp(percent, 0.0f, 1.0f);

        Vector3 newPosition = Vector3.Lerp(firstHitLeftPos, firstHitRightPos, percent);
        transform.position = new Vector3(newPosition.x, transform.position.y, newPosition.z);
        Camera.main.GetComponent<CameraMovement>().Aim();
    }
}
