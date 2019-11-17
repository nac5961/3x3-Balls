using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrangeWalls : MonoBehaviour
{
    public GameObject[] rightWalls;
    public GameObject[] leftWalls;
    public Material normalMaterial;
    public Material orangeMaterial;

    private int orangeWallLayer;
    private bool switchWalls;

    // Start is called before the first frame update
    void Start()
    {
        orangeWallLayer = 10;
        switchWalls = false;

        //Set initial orange walls
        SetOrangeWalls();
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneInfo.instance.IsAiming)
        {
            if (!switchWalls)
            {
                switchWalls = true;
            }
        }
        else if (SceneInfo.instance.IsTurnOver)
        {
            if (switchWalls)
            {
                switchWalls = false;
                SetOrangeWalls();
            }
        }
    }

    public void ResetWallLayers()
    {
        //Right walls
        for (int i = 0; i < rightWalls.Length; i++)
        {
            rightWalls[i].layer = 0;
        }

        //Left walls
        for (int i = 0; i < leftWalls.Length; i++)
        {
            leftWalls[i].layer = 0;
        }
    }

    private void SetOrangeWalls()
    {
        int rightRand = Random.Range(0, rightWalls.Length);
        int leftRand = Random.Range(0, leftWalls.Length);

        //Right walls
        for (int i = 0; i < rightWalls.Length; i++)
        {
            if (i == rightRand)
            {
                rightWalls[i].GetComponent<Renderer>().material = orangeMaterial;
                rightWalls[i].layer = orangeWallLayer;
            }
            else
            {
                rightWalls[i].GetComponent<Renderer>().material = normalMaterial;
                rightWalls[i].layer = 0;
            }
        }

        //Left walls
        for (int i = 0; i < leftWalls.Length; i++)
        {
            if (i == leftRand)
            {
                leftWalls[i].GetComponent<Renderer>().material = orangeMaterial;
                leftWalls[i].layer = orangeWallLayer;
            }
            else
            {
                leftWalls[i].GetComponent<Renderer>().material = normalMaterial;
                leftWalls[i].layer = 0;
            }
        }
    }
}
