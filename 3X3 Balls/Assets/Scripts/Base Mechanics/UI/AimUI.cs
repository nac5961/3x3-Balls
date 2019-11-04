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

    /// <summary>
    /// Lightens or Darkens the UI for jumping and curving based on the
    /// player's ability to jump and curve. Also sets the text appropriately.
    /// </summary>
    public void SetupUI()
    {
        Ball player = SceneInfo.instance.ActiveBall.GetComponent<Ball>();

        if (player.CanJumpShot)
        {
            LightenJumpHit();
            jumpText.text = "Start Jump\r\nShot (x1)";
        }
        else
        {
            DarkenJumpHit();
            jumpText.text = "Start Jump\r\nShot (x0)";
        }

        if (player.CanCurveShot)
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

    /// <summary>
    /// Changes the color of a UI and its associated text.
    /// </summary>
    /// <param name="img">UI to change</param>
    /// <param name="text">text to change</param>
    /// <param name="color">color to change to</param>
    private void ChangeColor(Image img, TextMeshProUGUI text, Color32 color)
    {
        img.color = color;
        text.color = color;
    }

    /// <summary>
    /// Darkens the UI and text for jumping.
    /// </summary>
    private void DarkenJumpHit()
    {
        ChangeColor(jump, jumpText, new Color32(85, 85, 85, 255));
    }

    /// <summary>
    /// Lightens the UI and text for jumping.
    /// </summary>
    private void LightenJumpHit()
    {
        ChangeColor(jump, jumpText, new Color32(255, 255, 255, 255));
    }

    /// <summary>
    /// Darkens the UI and text for curving.
    /// </summary>
    private void DarkenCurveHit()
    {
        ChangeColor(curve, curveText, new Color32(85, 85, 85, 255));
    }

    /// <summary>
    /// Lightens the UI and text for curving.
    /// </summary>
    private void LightenCurveHit()
    {
        ChangeColor(curve, curveText, new Color32(255, 255, 255, 255));
    }

    /// <summary>
    /// Sets the image of the ball display.
    /// </summary>
    /// <param name="sprite">image to change to</param>
    private void SetBall(Sprite sprite)
    {
        Ball player = SceneInfo.instance.ActiveBall.GetComponent<Ball>();

        ball.sprite = sprite;

        //Center is the only spot that jumping and curving are valid.
        //So check to see if we need to lighten it.
        if (sprite == center)
        {
            if (player.CanJumpShot)
            {
                LightenJumpHit();
            }

            if (player.CanCurveShot)
            {
                LightenCurveHit();
            }
        }

        //All other spots we do not want the player to jump and curve.
        //So disable the UI if it was enabled.
        else
        {
            if (player.CanJumpShot)
            {
                DarkenJumpHit();
            }

            if (player.CanCurveShot)
            {
                DarkenCurveHit();
            }
        }
    }

    /// <summary>
    /// Sets the ball image to a center hit.
    /// </summary>
    public void SetCenterHit()
    {
        SetBall(center);
    }

    /// <summary>
    /// Sets the ball image to a top hit.
    /// </summary>
    public void SetTopHit()
    {
        SetBall(top);
    }

    /// <summary>
    /// Sets the ball image to a bottom hit.
    /// </summary>
    public void SetBottomHit()
    {
        SetBall(bottom);
    }

    /// <summary>
    /// Sets the ball image to a left hit.
    /// </summary>
    public void SetLeftHit()
    {
        SetBall(left);
    }

    /// <summary>
    /// Sets the ball image to a right hit.
    /// </summary>
    public void SetRightHit()
    {
        SetBall(right);
    }
}
