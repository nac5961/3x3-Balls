using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeRoam : MonoBehaviour
{
    private float speed = 20.0f;
    private float rotSpeed = 60.0f;
    private bool enteredFreeRoam = false;
    private GameObject inGameCanvas;

    // Start is called before the first frame update
    void Start()
    {
        inGameCanvas = GameObject.Find("In-Game Canvas");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftAlt) && Input.GetKeyDown(KeyCode.V))
        {
            enteredFreeRoam = !enteredFreeRoam;

            if (enteredFreeRoam)
            {
                Camera.main.GetComponent<ThirdPersonCamera>().enabled = false;
                SceneInfo.instance.ActiveBall.GetComponent<PreviewLine>().enabled = false;
                SceneInfo.instance.Cue.SetActive(false);
                inGameCanvas.SetActive(false);
            }
            else
            {
                Camera.main.GetComponent<ThirdPersonCamera>().enabled = true;
                SceneInfo.instance.ActiveBall.GetComponent<PreviewLine>().enabled = true;
                SceneInfo.instance.Cue.SetActive(true);
                inGameCanvas.SetActive(true);
            }
        }

        if (enteredFreeRoam)
        {
            Move();
            Rotate();
        }
    }

    private void Move()
    {
        if (Input.GetKey(KeyCode.W))
        {
            Camera.main.transform.position += Camera.main.transform.forward * speed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.A))
        {
            Camera.main.transform.position += -Camera.main.transform.right * speed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.S))
        {
            Camera.main.transform.position += -Camera.main.transform.forward * speed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.D))
        {
            Camera.main.transform.position += Camera.main.transform.right * speed * Time.deltaTime;
        }
    }

    private void Rotate()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            Camera.main.transform.RotateAround(Camera.main.transform.position, Vector3.up, -rotSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            Camera.main.transform.RotateAround(Camera.main.transform.position, Vector3.up, rotSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            Camera.main.transform.RotateAround(Camera.main.transform.position, Camera.main.transform.right, -rotSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            Camera.main.transform.RotateAround(Camera.main.transform.position, Camera.main.transform.right, rotSpeed * Time.deltaTime);
        }
    }
}
