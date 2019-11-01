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
            EnableJumpHit();
            jumpText.text = "Start Jump\r\nShot (x1)";
        }
        else
        {
            DisableJumpHit();
            jumpText.text = "Start Jump\r\nShot (x0)";
        }

        if (SceneInfo.instance.ActiveBall.GetComponent<Ball>().CanCurveShot)
        {
            EnableCurveHit();
            curveText.text = "Start Curve\r\nShot (x1)";
        }
        else
        {
            DisableCurveHit();
            curveText.text = "Start Curve\r\nShot (x0)";
        }
    }

    private void DisableOption(Image img, TextMeshProUGUI textUI)
    {
        img.color = new Color32(85, 85, 85, 255);

        textUI.color = new Color32(85, 85, 85, 255);
    }

    private void EnableOption(Image img, TextMeshProUGUI textUI)
    {
        img.color = new Color32(255, 255, 255, 255);

        textUI.color = new Color32(255, 255, 255, 255);
    }

    public void DisableJumpHit()
    {
        DisableOption(jump, jumpText);
    }

    private void EnableJumpHit()
    {
        EnableOption(jump, jumpText);
    }

    public void DisableCurveHit()
    {
        DisableOption(curve, curveText);
    }

    private void EnableCurveHit()
    {
        EnableOption(curve, curveText);
    }

    private void SetBall(Sprite sprite)
    {
        ball.sprite = sprite;

        if (sprite == center)
        {
            if (SceneInfo.instance.ActiveBall.GetComponent<Ball>().CanJumpShot)
            {
                EnableJumpHit();
            }

            if (SceneInfo.instance.ActiveBall.GetComponent<Ball>().CanCurveShot)
            {
                EnableCurveHit();
            }
        }
        else
        {
            if (SceneInfo.instance.ActiveBall.GetComponent<Ball>().CanJumpShot)
            {
                DisableJumpHit();
            }

            if (SceneInfo.instance.ActiveBall.GetComponent<Ball>().CanCurveShot)
            {
                DisableCurveHit();
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
