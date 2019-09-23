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
        GameObject[] spawns = { p1Spawn, p2Spawn };

        for (int i = 0; i < GameInfo.instance.Players; i++)
        {
            GameObject playerBall = Instantiate(cueBallPrefab, spawns[i].transform.position, Quaternion.identity);
            MoveAboveSurface(playerBall, playerBall.GetComponent<SphereCollider>());

            SceneInfo.instance.Balls.Add(playerBall);
        }

        //Cue
        GameObject cue = Instantiate(cuePrefab);

        //Scene Manager
        SceneInfo.instance.Balls.Add(eightBall); //add last so we can index the player balls 
        SceneInfo.instance.Cue = cue;
        SceneInfo.instance.SetActiveBall();
        SceneInfo.instance.TargetBall = eightBall;

        //Canvas
        //GameObject.Find("Canvas").GetComponent<FadeScreen>().CueScript = poolCue.GetComponent<Cue>();

        //Camera
        Camera.main.gameObject.AddComponent<ThirdPersonCamera>();

        //TEMPORARY
        SceneInfo.instance.GameStart = true;
        SceneInfo.instance.IsAiming = true;
    }

    /// <summary>
    /// Moves a gameobject above a flat surface when spawned.
    /// * Make sure the spawn point is above the surface's collider *
    /// </summary>
    private void MoveAboveSurface(GameObject item, Collider collider)
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(item.transform.position, Vector3.down, out hitInfo))
        {
            item.transform.position = new Vector3(item.transform.position.x, hitInfo.point.y + collider.bounds.extents.y, item.transform.position.z);
        }
    }
}
