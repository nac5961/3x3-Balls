using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialEightBall : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneInfo.instance.GameStart && !SceneInfo.instance.Paused)
        {
            if (GetComponent<Ball>().IsScored && GetComponent<ParticleSystem>().isPlaying)
            {
                GetComponent<ParticleSystem>().Stop();
            }
        }
    }
}
