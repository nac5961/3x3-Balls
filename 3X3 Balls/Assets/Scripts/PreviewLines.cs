using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewLines : MonoBehaviour
{
    public Material[] materials;
    public float firstPreviewDistance;
    public float secondPreviewDistance;

    private GameObject cue;
    private GameObject cueBall;

    public GameObject Cue
    {
        get { return cue; }
        set { cue = value; }
    }

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
        if (!GameInfo.instance.PostHit)
        {
            Vector3 toCueBall = cueBall.transform.position - cue.transform.position;
            toCueBall = new Vector3(toCueBall.x, 0.0f, toCueBall.z).normalized;

            //Check if the preview line will hit something to limit its length
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
                    if (Physics.Raycast(hitInfo.point, toOtherBall, out hitInfo2, secondPreviewDistance))
                    {
                        //Limit the length
                        //secondDistance = hitInfo2.distance;
                    }

                    //Draw second preview line
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
