using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BumperGeneration : MonoBehaviour
{
    public GameObject bumperPrefab;
    public GameObject parent;
    public float radius;
    public int spacing;

    // Start is called before the first frame update
    void Start()
    {
        GenerateBumpers();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Generates bumpers in a circle based on the radius and spacing.
    /// </summary>
    private void GenerateBumpers()
    {
        for (int i = 0; i < 360; i += spacing)
        {
            //Note: (84.6, -10.76495, 17.2) is the center of the circle
            float x = 84.6f + radius * Mathf.Cos(Mathf.Deg2Rad * i);
            float z = 17.2f + radius * Mathf.Sin(Mathf.Deg2Rad * i);
            float y = -10.76495f + 12.0f;

            //Spawn bumper
            GameObject bumper = Instantiate(bumperPrefab, new Vector3(x, y, z), Quaternion.identity);
            bumper.transform.parent = parent.transform; //parent it, since we are rotating the parent to rotate the bumpers
        }
    }
}
