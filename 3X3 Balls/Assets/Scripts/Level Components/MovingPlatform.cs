using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public GameObject startPos;
    public GameObject endPos;

    public float moveSpeed;
    public float startWaitTime;
    public float endWaitTime;
    private float timer;
    private float movePercent;
    private bool reverse;


    // Start is called before the first frame update
    void Start()
    {
        timer = 0.0f;
        movePercent = 0.0f;
        reverse = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneInfo.instance.GameStart && !SceneInfo.instance.Paused)
        {
            MoveBetweenPoints();
        }
    }

    /// <summary>
    /// Moves the platform back and forth.
    /// </summary>
    private void MoveBetweenPoints()
    {
        float waitTime = reverse ? endWaitTime : startWaitTime;

        if (timer < waitTime)
        {
            timer += Time.deltaTime;
        }
        else
        {
            Vector3 start = reverse ? endPos.transform.position : startPos.transform.position;
            Vector3 end = reverse ? startPos.transform.position : endPos.transform.position;

            movePercent += moveSpeed * Time.deltaTime;
            movePercent = Mathf.Clamp(movePercent, 0.0f, 1.0f);

            transform.position = Vector3.Lerp(start, end, movePercent);

            if (movePercent >= 1.0f)
            {
                movePercent = 0.0f;
                timer = 0.0f;
                reverse = !reverse;
            }
        }
    }
}
