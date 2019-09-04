using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CourseSetup : MonoBehaviour
{
    public GameObject p1Spawn;
    public GameObject p2Spawn;

    public GameObject cue;
    public GameObject cueBall;

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
        //Cue Balls
        GameObject p1Ball = Instantiate(cueBall, p1Spawn.transform.position, Quaternion.identity);
        GameObject p2Ball = Instantiate(cueBall, p2Spawn.transform.position, Quaternion.identity);
        MoveAboveSurface(p1Ball, p1Ball.GetComponent<SphereCollider>());
        MoveAboveSurface(p2Ball, p2Ball.GetComponent<SphereCollider>());

        //Cue
        GameObject poolCue = Instantiate(cue);
        poolCue.SetActive(false); //Make invisible since we don't know what player will be going first/what player to attach the cue to first
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
