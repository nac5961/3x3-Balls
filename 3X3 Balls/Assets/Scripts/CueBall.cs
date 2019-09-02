using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CueBall : MonoBehaviour
{
    public float moveSpeed;

    private Vector3 farLeft;
    private Vector3 farRight;
    private float percent;

    public Vector3 FarLeft
    {
        get { return farLeft; }
        set { farLeft = value; }
    }

    public Vector3 FarRight
    {
        get { return farRight; }
        set { farRight = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        percent = 0.5f;

        //Ensure the ball starts at the initialized percent
        // * This will override the spawn position *
        Vector3 newPosition = Vector3.Lerp(farLeft, farRight, percent);
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

        Vector3 newPosition = Vector3.Lerp(farLeft, farRight, percent);
        transform.position = new Vector3(newPosition.x, transform.position.y, newPosition.z);
    }
}
