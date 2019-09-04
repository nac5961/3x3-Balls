using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Setup : MonoBehaviour
{
    public GameObject rackBalls;
    public GameObject cueBall;
    public GameObject cue;

    public GameObject rackBallSpawn;
    public GameObject cueBallSpawn;

    public GameObject firstHitLeftBorder;
    public GameObject firstHitRightBorder;

    // Start is called before the first frame update
    void Start()
    {
        SetupGame();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Spawns balls and cue to setup table
    /// </summary>
    private void SetupGame()
    {
        //Rack Balls
        GameObject rackBallsInstance = Instantiate(rackBalls, rackBallSpawn.transform.position, Quaternion.identity);
        MoveAboveSurface(rackBallsInstance, rackBallsInstance.transform.GetChild(0).GetComponent<SphereCollider>());

        //Cue Ball
        GameObject cueBallInstance = Instantiate(cueBall, cueBallSpawn.transform.position, Quaternion.identity);
        MoveAboveSurface(cueBallInstance, cueBallInstance.GetComponent<SphereCollider>());
        cueBallInstance.GetComponent<CueBall>().FirstHitLeftPos = firstHitLeftBorder.transform.position;
        cueBallInstance.GetComponent<CueBall>().FirstHitRightPos = firstHitRightBorder.transform.position;

        //Temporarily disable gravity until first hit
        //to avoid collision problems with the sides of the table
        //when moving the ball before hitting it
        cueBallInstance.GetComponent<Rigidbody>().useGravity = false;

        //Cue
        GameObject cueInstance = Instantiate(cue);
        cueInstance.GetComponent<PoolCue>().CueBall = cueBallInstance;

        //Camera
        Camera.main.GetComponent<CameraMovement>().CueBall = cueBallInstance;

        //Game Info Script
        List<GameObject> balls = new List<GameObject>();
        for (int i = 0; i < rackBallsInstance.transform.childCount; i++)
        {
            balls.Add(rackBallsInstance.transform.GetChild(i).gameObject);
        }
        balls.Add(cueBallInstance);
        GameInfo.instance.Balls = balls.ToArray();
        GameInfo.instance.Cue = cueInstance;

        //Preview Lines Script
        GetComponent<PreviewLines>().CueBall = cueBallInstance;
    }

    /// <summary>
    /// Moves a gameobject above a flat surface when spawned.
    /// * Make sure the gameobject is spawned above the surface's collider *
    /// </summary>
    private void MoveAboveSurface(GameObject item, Collider collider)
    {
        RaycastHit hitInfo;
        Physics.Raycast(item.transform.position, Vector3.down, out hitInfo);

        item.transform.position = new Vector3(item.transform.position.x, hitInfo.point.y + collider.bounds.extents.y, item.transform.position.z);
    }
}
