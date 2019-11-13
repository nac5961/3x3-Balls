﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadingPlatform : MonoBehaviour
{
    public float fadeSpeed;
    public float waitTime;
    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        timer = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneInfo.instance.GameStart && !SceneInfo.instance.Paused)
        {
            Fade();
        }
    }

    /// <summary>
    /// Fades in and out.
    /// </summary>
    private void Fade()
    {
        if (timer < waitTime)
        {
            timer += Time.deltaTime;
        }
        else
        {
            //Lower alpha
            Color color = GetComponent<Renderer>().material.color;
            color.a -= fadeSpeed * Time.deltaTime;
            color.a = Mathf.Clamp(color.a, 0.0f, 1.0f);

            //Set color
            GetComponent<Renderer>().material.color = color;

            //Enable collisions when it fades in
            if (GetComponent<Collider>().isTrigger && color.a > 0.2f)
            {
                GetComponent<Collider>().isTrigger = false;
            }

            //Disable collisions when it fades out
            else if (!GetComponent<Collider>().isTrigger && color.a < 0.2f)
            {
                GetComponent<Collider>().isTrigger = true;
            }

            //Reset
            if (color.a == 0.0f || color.a == 1.0f)
            {
                fadeSpeed = -fadeSpeed;
                timer = 0.0f;
            }
        }
    }
}
