using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewLines : MonoBehaviour
{
    public Material[] materials;
    public float firstPreviewDistance;
    public float secondPreviewDistance;

    private GameObject cueBall;

    public GameObject CueBall
    {
        get { return cueBall; }
        set { cueBall = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    private void OnRenderObject()
    {
        float firstDistance = firstPreviewDistance;
        float secondDistance = secondPreviewDistance;

        //Get aim direction
        if (GameInfo.instance.IsAiming)
        {
            Vector3 toCueBall = cueBall.transform.position - GameInfo.instance.Cue.transform.position;
            toCueBall = new Vector3(toCueBall.x, 0.0f, toCueBall.z).normalized;

            //Check if the first preview line will hit something to limit its length
            RaycastHit hitInfo;
            if (Physics.SphereCast(cueBall.transform.position, cueBall.GetComponent<SphereCollider>().radius, toCueBall, out hitInfo, firstPreviewDistance))
            {
                //Limit the length
                firstDistance = hitInfo.distance;

                //Another ball is hit, show second preview line
                if (hitInfo.transform.CompareTag("Solid") || hitInfo.transform.CompareTag("Striped") || hitInfo.transform.CompareTag("Ball"))
                {
                    Vector3 toOtherBall = hitInfo.transform.position - hitInfo.point;
                    toOtherBall = new Vector3(toOtherBall.x, 0.0f, toOtherBall.z).normalized;

                    RaycastHit hitInfo2;
                    if (Physics.SphereCast(hitInfo.point, hitInfo.transform.GetComponent<SphereCollider>().radius, toOtherBall, out hitInfo2, secondPreviewDistance))
                    {
                        //Limit the length
                        //Make sure the raycast is not hitting the ball that is being raycasted from
                        //This may happen since hitInfo.point is the origin and not the ball itself (hitInfo.transform.position)
                        if (hitInfo2.transform.gameObject != hitInfo.transform.gameObject)
                        {
                            secondDistance = hitInfo2.distance;
                        }
                    }

                    //Draw second preview line
                    //hitInfo.point may be more accurate than hitInfo.transform.position
                    materials[1].SetPass(0);
                    GL.Begin(GL.LINES);
                    GL.Vertex(hitInfo.point);
                    GL.Vertex(hitInfo.point + (toOtherBall * secondDistance));
                    GL.End();
                }
            }

            //Draw first preview line
            materials[0].SetPass(0);
            GL.Begin(GL.LINES);
            GL.Vertex(cueBall.transform.position);
            GL.Vertex(cueBall.transform.position + (toCueBall * firstDistance));
            GL.End();
        }
    }
}
