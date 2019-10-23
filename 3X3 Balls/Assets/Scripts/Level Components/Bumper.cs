using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bumper : MonoBehaviour
{
    public bool isStationary;
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
        removalHeight = startHeight - gameObject.GetComponent<CapsuleCollider>().bounds.extents.y * 2.5f;
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

    private void Disappear()
    {
        if (percent < 1.0f)
        {
            percent += Time.deltaTime * removalSpeed;
            percent = Mathf.Clamp(percent, 0.0f, 1.0f);

            Vector3 start = new Vector3(transform.position.x, startHeight, transform.position.z);
            Vector3 end = new Vector3(transform.position.x, removalHeight, transform.position.z);

            transform.position = Vector3.Lerp(start, end, percent);

            if (percent >= 1.0f)
            {
                remove = false;
                gameObject.GetComponent<CapsuleCollider>().isTrigger = true;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            if (!isStationary && !remove && !collision.gameObject.GetComponent<Rigidbody>().IsSleeping() && collision.gameObject.GetComponent<Rigidbody>().velocity.magnitude > 0.01f)
            {
                remove = true;
            }
        }
    }
}
