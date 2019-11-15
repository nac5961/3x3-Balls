using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrangeBallSpawner : MonoBehaviour
{
    public GameObject orangeBallPrefab;
    public GameObject spawnPos;

    private List<GameObject> enteredBalls;
    private List<GameObject> spawnedBalls;

    private bool checkBalls;

    // Start is called before the first frame update
    void Start()
    {
        enteredBalls = new List<GameObject>();
        spawnedBalls = new List<GameObject>();

        checkBalls = false;
    }

    // Update is called once per frame
    void Update()
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

        if (!availableBalls && enteredBalls.Count > 0)
        {
            SpawnBall();
        }
    }

    private void SpawnBall()
    {
        GameObject orangeBall = Instantiate(orangeBallPrefab, spawnPos.transform.position, Quaternion.identity);

        spawnedBalls.Add(orangeBall);
        SceneInfo.instance.Balls.Add(orangeBall);

        //Need to move the ball in order for it to fall down
        orangeBall.transform.position = orangeBall.transform.position + new Vector3(0.0f, -1.0f, 0.0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ball") && !other.name.Contains("Orange"))
        {
            enteredBalls.Add(other.gameObject);

            SpawnBall();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Ball") && !other.name.Contains("Orange"))
        {
            enteredBalls.Remove(other.gameObject);
        }
    }
}
