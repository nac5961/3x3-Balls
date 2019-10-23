﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleRotation : MonoBehaviour
{
    public float rotationSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneInfo.instance.GameStart && !SceneInfo.instance.Paused)
        {
            RotateObject();
        }
    }

    private void RotateObject()
    {
        transform.RotateAround(transform.position, Vector3.up, rotationSpeed * Time.deltaTime);
    }
}
