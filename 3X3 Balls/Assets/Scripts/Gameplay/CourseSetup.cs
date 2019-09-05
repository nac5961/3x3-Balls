using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CourseSetup : MonoBehaviour
{
    public GameObject p1Spawn;
    public GameObject p2Spawn;

    public GameObject cue;
    public GameObject cueBall;

    public GameObject solidBallsSpawn;
    public GameObject stripedBallsSpawn;
    public GameObject solidBalls;
    public GameObject stripedBalls;

    // Start is called before the first frame update
    void Start()
    {
        SetupCourse();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SetupCourse()
    {
        //Solid Balls
        GameObject solids = Instantiate(solidBalls, solidBallsSpawn.transform.position, Quaternion.identity);
        for (int i = 0; i < solids.transform.childCount; i++)
        {
            if (GameInfo.instance.CapturedBalls.Contains(solids.transform.GetChild(i).name))
            {
                Destroy(solids.transform.GetChild(i).gameObject);
            }
            else
            {
                solids.transform.GetChild(i).GetComponent<Ball>().RespawnPos = solidBallsSpawn.transform.position;
            }
        }

        //Striped Balls
        GameObject stripes = Instantiate(stripedBalls, stripedBallsSpawn.transform.position, Quaternion.identity);
        for (int i = 0; i < stripes.transform.childCount; i++)
        {
            if (GameInfo.instance.CapturedBalls.Contains(stripes.transform.GetChild(i).name))
            {
                Destroy(stripes.transform.GetChild(i).gameObject);
            }
            else
            {
                stripes.transform.GetChild(i).GetComponent<Ball>().RespawnPos = stripedBallsSpawn.transform.position;
            }
        }

        //Cue Balls
        GameObject p1Ball = Instantiate(cueBall, p1Spawn.transform.position, Quaternion.identity);
        GameObject p2Ball = Instantiate(cueBall, p2Spawn.transform.position, Quaternion.identity);
        MoveAboveSurface(p1Ball, p1Ball.GetComponent<SphereCollider>());
        MoveAboveSurface(p2Ball, p2Ball.GetComponent<SphereCollider>());

        //Cue
        GameObject poolCue = Instantiate(cue);
        poolCue.GetComponent<Cue>().P1CueBall = p1Ball;
        poolCue.GetComponent<Cue>().P2CueBall = p2Ball;

        //Cue Balls (Continued)
        p1Ball.GetComponent<PreviewLine>().Cue = poolCue;
        p1Ball.GetComponent<PreviewLine>().Player = GameInfo.instance.P1Type;
        p1Ball.GetComponent<CueBall>().RespawnPos = p1Spawn.transform.position;
        p2Ball.GetComponent<PreviewLine>().Cue = poolCue;
        p2Ball.GetComponent<PreviewLine>().Player = GameInfo.instance.P2Type;
        p2Ball.GetComponent<CueBall>().RespawnPos = p2Spawn.transform.position;

        //Camera
        Camera.main.GetComponent<CameraMovement>().CueScript = poolCue.GetComponent<Cue>();

        //Canvas
        GameObject.Find("Canvas").GetComponent<FadeScreen>().CueScript = poolCue.GetComponent<Cue>();

        //Scene Manager
        List<GameObject> balls = new List<GameObject>();
        balls.Add(p1Ball);
        balls.Add(p2Ball);
        SceneInfo.instance.Balls = balls.ToArray();
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
