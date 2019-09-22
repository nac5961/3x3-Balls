//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;

//public class FadeScreen : MonoBehaviour
//{
//    public GameObject panel;
//    public float fadeSpeed;

//    private Cue cueScript;
//    private bool isFading;

//    public Cue CueScript
//    {
//        set { cueScript = value; }
//    }

//    // Start is called before the first frame update
//    void Start()
//    {
//        isFading = false;
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        FadeOutAndIn();
//    }

//    public void FadeOutAndIn()
//    {
//        if (SceneInfo.instance.IsTurnOver && !SceneInfo.instance.IsRoundOver && !isFading)
//        {
//            isFading = true;
//        }

//        //Fade out and in screen
//        if (isFading)
//        {
//            Color color = panel.GetComponent<Image>().color;
//            panel.GetComponent<Image>().color = new Color(color.r, color.g, color.b, Mathf.Clamp(color.a + fadeSpeed * Time.deltaTime, 0.0f, 1.0f));

//            //Screen is faded out, set up next turn
//            if (panel.GetComponent<Image>().color.a >= 1.0f)
//            {
//                SceneInfo.instance.IsTurnOver = false;
//                SceneInfo.instance.Turn = SceneInfo.instance.Turn == GameInfo.instance.P1Type ? GameInfo.instance.P2Type : GameInfo.instance.P1Type;
//                fadeSpeed = -fadeSpeed; //Reverse to fade out

//                GetComponent<TurnDisplay>().StartDisplay();

//                cueScript.AlignWithBall();
//            }

//            //Finished fading
//            else if (panel.GetComponent<Image>().color.a <= 0.0f)
//            {
//                isFading = false;
//                fadeSpeed = -fadeSpeed; //Reverse so it can fade in again
//            }
//        }
//    }
//}
