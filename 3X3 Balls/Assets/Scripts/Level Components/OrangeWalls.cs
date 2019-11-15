using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrangeWalls : MonoBehaviour
{
    public GameObject[] walls;
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

    public void ResetWalls()
    {
        for (int i = 0; i < walls.Length; i++)
        {
            walls[i].GetComponent<Renderer>().material = normalMaterial;
            walls[i].layer = 0;
        }
    }

    private void SetOrangeWalls()
    {
        List<GameObject> wallsCopy = new List<GameObject>(walls);

        for (int i = 0; i < 1; i++)
        {
            int rand = Random.Range(0, wallsCopy.Count);

            wallsCopy[rand].GetComponent<Renderer>().material = orangeMaterial;
            wallsCopy[rand].layer = orangeWallLayer;
            wallsCopy.RemoveAt(rand);
        }

        for (int i = 0; i < wallsCopy.Count; i++)
        {
            wallsCopy[i].GetComponent<Renderer>().material = normalMaterial;
            wallsCopy[i].layer = 0;
        }
    }
}
