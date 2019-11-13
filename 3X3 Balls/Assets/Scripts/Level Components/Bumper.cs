using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bumper : MonoBehaviour
{
    public float removalSpeed;
    private bool remove;

    private float percent;
    private float startHeight;
    private float removalHeight;

    // Start is called before the first frame update
    void Start()
    {
        remove = false;

        percent = 0.0f;
        startHeight = transform.position.y;
        removalHeight = startHeight - GetComponent<Collider>().bounds.extents.y * 2.5f;
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneInfo.instance.GameStart && !SceneInfo.instance.Paused)
        {
            if (remove)
            {
                Disappear();
            }
        }
    }

    /// <summary>
    /// Moves the bumper below ground, then destroys it.
    /// </summary>
    private void Disappear()
    {
        percent += removalSpeed * Time.deltaTime;
        percent = Mathf.Clamp(percent, 0.0f, 1.0f);

        Vector3 start = new Vector3(transform.position.x, startHeight, transform.position.z);
        Vector3 end = new Vector3(transform.position.x, removalHeight, transform.position.z);

        transform.position = Vector3.Lerp(start, end, percent);

        if (percent >= 1.0f)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            //Remove only if the ball is moving fast enough.
            //This prevents bumpers falling down if the ball is standing still.
            if (!remove && !collision.gameObject.GetComponent<Rigidbody>().IsSleeping() && collision.gameObject.GetComponent<Rigidbody>().velocity.magnitude > 0.2f)
            {
                remove = true;
            }
        }
    }
}
