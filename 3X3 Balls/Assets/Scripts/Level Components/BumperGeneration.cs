﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BumperGeneration : MonoBehaviour
{
    public GameObject bumperPrefab;
    public GameObject parent;
    public float radius;

    // Start is called before the first frame update
    void Start()
    {
        GenerateBumpers();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void GenerateBumpers()
    {
        for (float i = 0.0f; i < 360.0f; i += 7.0f)
        {
            float x = 84.6f + radius * Mathf.Cos(Mathf.Deg2Rad * i);
            float z = 17.2f + radius * Mathf.Sin(Mathf.Deg2Rad * i);
            float y = -10.76495f + 12.0f;

            GameObject bumper = Instantiate(bumperPrefab, new Vector3(x, y, z), Quaternion.identity);
            bumper.transform.parent = parent.transform;
        }

        for (float i = 0.0f; i < 360.0f; i += 14.0f)
        {
            float x = 84.6f + (radius + 10.0f) * Mathf.Cos(Mathf.Deg2Rad * i);
            float z = 17.2f + (radius + 10.0f) * Mathf.Sin(Mathf.Deg2Rad * i);
            float y = -10.76495f + 12.0f;

            GameObject bumper = Instantiate(bumperPrefab, new Vector3(x, y, z), Quaternion.identity);
            bumper.transform.parent = parent.transform;
        }
    }
}
