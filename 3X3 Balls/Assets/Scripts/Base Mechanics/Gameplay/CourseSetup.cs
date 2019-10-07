using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CourseSetup : MonoBehaviour
{
    public int offset;
    public GameObject playerSpawn;
    private List<Vector3> spawnPoints;

    public GameObject cuePrefab;
    public GameObject cueBallPrefab;

    public GameObject eightBallSpawn;
    public GameObject eightBallPrefab;

    // Start is called before the first frame update
    void Start()
    {
        spawnPoints = new List<Vector3>();

        SetupCourse();

        //TEMPORARY
        SceneInfo.instance.StartGame();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Spawns the gameobjects necessary to play the game.
    /// Also hooks up scripts and references.
    /// </summary>
    private void SetupCourse()
    {
        SetupSpawnPoints();

        //Eight Ball
        GameObject eightBall = Instantiate(eightBallPrefab, eightBallSpawn.transform.position, Quaternion.identity);
        MoveAboveSurface(eightBall, eightBall.GetComponent<SphereCollider>());
        eightBall.GetComponent<Ball>().EightBallSpawn = eightBall.transform.position;

        //Cue Balls aka Players
        for (int i = 0; i < spawnPoints.Count; i++)
        {
            GameObject playerBall = Instantiate(cueBallPrefab, spawnPoints[i], Quaternion.identity);
            MoveAboveSurface(playerBall, playerBall.GetComponent<SphereCollider>());
            playerBall.GetComponent<Ball>().EightBallSpawn = eightBall.transform.position;

            SceneInfo.instance.Balls.Add(playerBall);
        }

        //Cue
        GameObject cue = Instantiate(cuePrefab);

        //Scene Manager
        SceneInfo.instance.Balls.Add(eightBall); //Add after player balls so we can index the Balls list by the player (player 1 is ball[0], player 2 is ball[1], etc.)
        SceneInfo.instance.Cue = cue;
        SceneInfo.instance.SetActiveBall();
        SceneInfo.instance.TargetBall = eightBall;
        SceneInfo.instance.TargetBallMaterial = eightBall.GetComponent<Renderer>().material;

        //Camera
        Camera.main.gameObject.AddComponent<ThirdPersonCamera>(); //Cannot attach in inspector because we need an active ball to be spawned
    }

    /// <summary>
    /// Calculates the spawn points for each player.
    /// </summary>
    private void SetupSpawnPoints()
    {
        if (GameInfo.instance.Players == 1)
        {
            spawnPoints.Add(playerSpawn.transform.position);
        }
        else
        {
            //Calculate the spacing inbetween each player
            float spacing = cueBallPrefab.GetComponent<SphereCollider>().radius * 2.0f + offset;

            //Get the direction to spawn players to the right and left of each other
            Vector3 left = -playerSpawn.transform.right;

            //Get the position of the first ball based on the number of players playing
            float playerNumOffset = GameInfo.instance.Players / 2.0f - 0.5f;
            Vector3 startPos = playerSpawn.transform.position + (left * spacing * playerNumOffset);

            //Save the position and calculate the position for the next ball
            for (int i = 0; i < GameInfo.instance.Players; i++)
            {
                spawnPoints.Add(startPos);

                startPos -= left * spacing;
            }
        }
    }

    /// <summary>
    /// Moves a gameobject above a flat surface when spawned.
    /// * Make sure the spawn point is above the surface's collider *
    /// </summary>
    /// <param name="item">gameobject to move above the flat surface</param>
    /// <param name="collider">collider attached to the gameobject</param>
    private void MoveAboveSurface(GameObject item, Collider collider)
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(item.transform.position, Vector3.down, out hitInfo))
        {
            item.transform.position = new Vector3(item.transform.position.x, hitInfo.point.y + collider.bounds.extents.y, item.transform.position.z);
        }
    }
}
