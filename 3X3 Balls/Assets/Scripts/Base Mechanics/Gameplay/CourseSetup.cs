using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CourseSetup : MonoBehaviour
{
    public GameObject levelOverview;
    public GameObject scoreAreaOverview;

    public GameObject playerSpawn;

    public GameObject cuePrefab;
    public GameObject cueBallPrefab;

    public Material[] cueBallMaterials;

    public GameObject[] targetBalls;
    public GameObject[] otherBalls;

    // Start is called before the first frame update
    void Start()
    {
        SetupCourse();

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
        //Eight Ball
        for (int i = 0; i < targetBalls.Length; i++)
        {
            MoveAboveSurface(targetBalls[i], targetBalls[i].GetComponent<Collider>());
            targetBalls[i].GetComponent<Ball>().TargetBallSpawn = targetBalls[i].transform.position;
        }

        //Other Balls
        for (int i = 0; i < otherBalls.Length; i++)
        {
            MoveAboveSurface(otherBalls[i], otherBalls[i].GetComponent<Collider>());
        }

        //Cue Balls aka Players
        for (int i = 0; i < GameInfo.instance.Players; i++)
        {
            GameObject playerBall = Instantiate(cueBallPrefab, playerSpawn.transform.position, Quaternion.identity);
            MoveAboveSurface(playerBall, playerBall.GetComponent<Collider>());
            playerBall.GetComponent<Ball>().TargetBallSpawn = targetBalls[0].transform.position;

            //Set appropriate material
            playerBall.GetComponent<MeshRenderer>().material = cueBallMaterials[i];

            //Prevent collisions until first hit so balls can spawn inside each other
            playerBall.GetComponent<Collider>().isTrigger = true;
            playerBall.GetComponent<Rigidbody>().useGravity = false;

            SceneInfo.instance.Balls.Add(playerBall);
        }

        //Cue
        GameObject cue = Instantiate(cuePrefab);

        //Scene Manager
        for (int i = 0; i < targetBalls.Length; i++)
        {
            SceneInfo.instance.Balls.Add(targetBalls[i]); //Add after player balls so we can index the Balls list by the player (player 1 is ball[0], player 2 is ball[1], etc.)
            SceneInfo.instance.TargetBalls.Add(targetBalls[i]);
        }
        for (int i = 0; i < otherBalls.Length; i++)
        {
            SceneInfo.instance.Balls.Add(otherBalls[i]); //Add after player balls so we can index the Balls list by the player (player 1 is ball[0], player 2 is ball[1], etc.)
        }
        SceneInfo.instance.Cue = cue;
        SceneInfo.instance.SetActiveBall();
        SceneInfo.instance.TargetBallMaterial = targetBalls[0].GetComponent<Renderer>().material;

        //Camera
        Camera.main.gameObject.AddComponent<ThirdPersonCamera>(); //Cannot attach in inspector because we need an active ball to be spawned
        Camera.main.GetComponent<ThirdPersonCamera>().LevelOverview = levelOverview;
        Camera.main.GetComponent<ThirdPersonCamera>().ScoreAreaOverview = scoreAreaOverview;
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
