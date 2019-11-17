using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrangeBallSpawner : MonoBehaviour
{
    public GameObject orangeBallPrefab;
    public GameObject spawnPos;

    private List<GameObject> spawnedBalls;
    private int enteredBalls;

    private bool checkBalls;

    // Start is called before the first frame update
    void Start()
    {
        spawnedBalls = new List<GameObject>();
        enteredBalls = 0;

        checkBalls = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneInfo.instance.GameStart && !SceneInfo.instance.Paused)
        {
            if (SceneInfo.instance.IsAiming)
            {
                if (!checkBalls)
                {
                    checkBalls = true;
                }
            }
            else if (SceneInfo.instance.IsTurnOver)
            {
                if (checkBalls)
                {
                    checkBalls = false;

                    SpawnExtraBall();
                }
            }
        }
    }

    /// <summary>
    /// Spawns an extra ball if for some reason they are no
    /// available orange balls.
    /// ** This could happen if one player scores 2 orange balls **
    /// </summary>
    private void SpawnExtraBall()
    {
        bool availableBalls = false;

        for (int i = 0; i < spawnedBalls.Count; i++)
        {
            if (!spawnedBalls[i].GetComponent<Ball>().IsScored)
            {
                availableBalls = true;
                break;
            }
        }

        //There are no available balls but there are players
        //still in the area.
        if (!availableBalls && enteredBalls > 0)
        {
            SpawnBall();
        }
    }

    /// <summary>
    /// Spawns an orange ball.
    /// </summary>
    private void SpawnBall()
    {
        //Spawn
        GameObject orangeBall = Instantiate(orangeBallPrefab, spawnPos.transform.position, Quaternion.identity);

        //Add to list of balls to keep track of it and properly end turns
        spawnedBalls.Add(orangeBall);
        SceneInfo.instance.Balls.Add(orangeBall);

        //Need to move the ball in order for it to fall down
        orangeBall.transform.position = orangeBall.transform.position + new Vector3(0.0f, -1.0f, 0.0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        //Spawn balls when a cue ball enters
        if (other.gameObject.CompareTag("Ball") && other.name.Contains("Cue Ball"))
        {
            enteredBalls++;
            SpawnBall();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Ball") && other.name.Contains("Cue Ball"))
        {
            enteredBalls--;
        }
    }
}
