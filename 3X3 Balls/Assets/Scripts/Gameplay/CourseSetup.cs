using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CourseSetup : MonoBehaviour
{
    public GameObject p1Spawn;
    public GameObject p2Spawn;

    public GameObject cuePrefab;
    public GameObject cueBallPrefab;

    public GameObject eightBallSpawn;
    public GameObject eightBallPrefab;

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
        //Eight Ball
        GameObject eightBall = Instantiate(eightBallPrefab, eightBallSpawn.transform.position, Quaternion.identity);

        //Cue Balls aka Players
        GameObject p1Ball = Instantiate(cueBallPrefab, p1Spawn.transform.position, Quaternion.identity);
        GameObject p2Ball = Instantiate(cueBallPrefab, p2Spawn.transform.position, Quaternion.identity);
        MoveAboveSurface(p1Ball, p1Ball.GetComponent<SphereCollider>());
        MoveAboveSurface(p2Ball, p2Ball.GetComponent<SphereCollider>());

        //Cue
        GameObject cue = Instantiate(cuePrefab);

        //Camera
        Camera.main.gameObject.AddComponent<ThirdPersonCamera>();

        //Canvas
        //GameObject.Find("Canvas").GetComponent<FadeScreen>().CueScript = poolCue.GetComponent<Cue>();

        //Scene Manager
        List<GameObject> balls = new List<GameObject>();
        balls.Add(p1Ball);
        balls.Add(p2Ball);
        balls.Add(eightBall);
        SceneInfo.instance.Balls = balls;
        SceneInfo.instance.Cue = cue;
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
