using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnTransition : MonoBehaviour
{
    public GameObject uiCover;
    public float transitionSpeed;

    private bool isTransitioning;

    // Start is called before the first frame update
    void Start()
    {
        isTransitioning = false;   
    }

    // Update is called once per frame
    void Update()
    {
        Transition();
    }

    public void Transition()
    {
        if (GameInfo.instance.IsTurnOver && !isTransitioning)
        {
            isTransitioning = true;
        }

        //Fade out and in screen
        if (isTransitioning)
        {
            Color color = uiCover.GetComponent<Image>().color;
            uiCover.GetComponent<Image>().color = new Color(color.r, color.g, color.b, Mathf.Clamp(color.a + transitionSpeed * Time.deltaTime, 0.0f, 1.0f));

            //Screen is faded out, set up next turn
            if (uiCover.GetComponent<Image>().color.a >= 1.0f)
            {
                GameInfo.instance.IsTurnOver = false;
                GameInfo.instance.Cue.GetComponent<PoolCue>().AlignWithBall();

                transitionSpeed = -transitionSpeed; //Reverse to fade out
            }

            //Finished transitioning
            else if (uiCover.GetComponent<Image>().color.a <= 0.0f)
            {
                isTransitioning = false;
                transitionSpeed = -transitionSpeed; //Reverse so it can fade in again
            }
        }
    }
}
