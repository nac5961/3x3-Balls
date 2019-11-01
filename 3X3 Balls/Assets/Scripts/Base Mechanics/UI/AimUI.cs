using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AimUI : MonoBehaviour
{
    public TextMeshProUGUI jumpText;
    public TextMeshProUGUI curveText;
    public Image jump;
    public Image curve;
    public Image ball;
    public Sprite center;
    public Sprite top;
    public Sprite bottom;
    public Sprite left;
    public Sprite right;

    public Image Ball
    {
        get { return ball; }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetupUI()
    {
        if (SceneInfo.instance.ActiveBall.GetComponent<Ball>().CanJumpShot)
        {
            LightenJumpHit();
            jumpText.text = "Start Jump\r\nShot (x1)";
        }
        else
        {
            DarkenJumpHit();
            jumpText.text = "Start Jump\r\nShot (x0)";
        }

        if (SceneInfo.instance.ActiveBall.GetComponent<Ball>().CanCurveShot)
        {
            LightenCurveHit();
            curveText.text = "Start Curve\r\nShot (x1)";
        }
        else
        {
            DarkenCurveHit();
            curveText.text = "Start Curve\r\nShot (x0)";
        }
    }

    private void ChangeColor(Image img, TextMeshProUGUI text, Color32 color)
    {
        img.color = color;
        text.color = color;
    }

    private void DarkenJumpHit()
    {
        ChangeColor(jump, jumpText, new Color32(85, 85, 85, 255));
    }

    private void LightenJumpHit()
    {
        ChangeColor(jump, jumpText, new Color32(255, 255, 255, 255));
    }

    private void DarkenCurveHit()
    {
        ChangeColor(curve, curveText, new Color32(85, 85, 85, 255));
    }

    private void LightenCurveHit()
    {
        ChangeColor(curve, curveText, new Color32(255, 255, 255, 255));
    }

    private void SetBall(Sprite sprite)
    {
        ball.sprite = sprite;

        if (sprite == center)
        {
            if (SceneInfo.instance.ActiveBall.GetComponent<Ball>().CanJumpShot)
            {
                LightenJumpHit();
            }

            if (SceneInfo.instance.ActiveBall.GetComponent<Ball>().CanCurveShot)
            {
                LightenCurveHit();
            }
        }
        else
        {
            if (SceneInfo.instance.ActiveBall.GetComponent<Ball>().CanJumpShot)
            {
                DarkenJumpHit();
            }

            if (SceneInfo.instance.ActiveBall.GetComponent<Ball>().CanCurveShot)
            {
                DarkenCurveHit();
            }
        }
    }

    public void SetCenterHit()
    {
        SetBall(center);
    }

    public void SetTopHit()
    {
        SetBall(top);
    }

    public void SetBottomHit()
    {
        SetBall(bottom);
    }

    public void SetLeftHit()
    {
        SetBall(left);
    }

    public void SetRightHit()
    {
        SetBall(right);
    }
}
