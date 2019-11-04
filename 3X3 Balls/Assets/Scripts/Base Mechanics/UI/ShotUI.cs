using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShotUI : MonoBehaviour
{
    public Image ball;

    public Image toggle;
    public TextMeshProUGUI toggleText;

    public Image cancel;
    public TextMeshProUGUI cancelText;
    public TextMeshProUGUI hitText;

    public Image border;
    public Sprite normal;
    public Sprite jump;
    public Sprite curveRight;
    public Sprite curveLeft;

    public Image powerMeter;
    public float fillSpeed;
    public float minFill;

    private bool powerSet;

    public bool PowerSet
    {
        get { return powerSet; }
        set { powerSet = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        powerMeter.fillAmount = minFill;
        powerSet = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneInfo.instance.GameStart && !SceneInfo.instance.Paused)
        {
            if (SceneInfo.instance.IsTakingShot)
            {
                AnimatePowerMeter();
            }
        }
    }

    /// <summary>
    /// Fills and unfills the power meter.
    /// </summary>
    private void AnimatePowerMeter()
    {
        if (!powerSet)
        {
            float percent = powerMeter.fillAmount;

            percent += fillSpeed * Time.deltaTime;
            percent = Mathf.Clamp(percent, minFill, 1.0f);
            powerMeter.fillAmount = percent;

            //Reverse
            if (percent == minFill || percent == 1.0f)
            {
                fillSpeed = -fillSpeed;
            }
        }
    }

    /// <summary>
    /// Sets up the images, text, and variables needed to display the UI.
    /// </summary>
    public void SetupUI()
    {
        //Make sure the ball image matches the one on the AimUI.
        ball.sprite = UIGameInfo.instance.AimUI.GetComponent<AimUI>().Ball.sprite;

        //Show the cancel image and text along with the correct text for hitting.
        cancel.gameObject.SetActive(true);
        cancelText.gameObject.SetActive(true);
        hitText.text = "Hold to\r\nset power";

        //Reset variables to their defaults
        powerSet = false;
        powerMeter.fillAmount = minFill;
        fillSpeed = Mathf.Abs(fillSpeed);
    }

    /// <summary>
    /// Displays the correct UI for releasing the button
    /// to hit the ball.
    /// </summary>
    public void ShowReleaseUI()
    {
        cancel.gameObject.SetActive(false);
        cancelText.gameObject.SetActive(false);
        hitText.text = "Release to\r\nhit ball";
    }

    /// <summary>
    /// Sets the correct border for the power meter based on the
    /// shot type.
    /// </summary>
    /// <param name="shot">shot type (jump, curve, normal)</param>
    /// <param name="right">curving right? Defaults to true</param>
    public void SetBorder(ShotType shot, bool right = true)
    {
        if (shot == ShotType.Normal)
        {
            border.sprite = normal;

            //Toggle is only for curving, so make sure it's disabled
            toggle.gameObject.SetActive(false);
            toggleText.gameObject.SetActive(false);
        }
        else if (shot == ShotType.Jump)
        {
            border.sprite = jump;

            //Toggle is only for curving, so make sure it's disabled
            toggle.gameObject.SetActive(false);
            toggleText.gameObject.SetActive(false);
        }
        else if (shot == ShotType.Curve)
        {
            SetCurveBorder(right);

            toggle.gameObject.SetActive(true);
            toggleText.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// Sets the correct border for the power meter based on the
    /// curve direction (left or right).
    /// </summary>
    /// <param name="right">curving right?</param>
    public void SetCurveBorder(bool right)
    {
        if (right)
        {
            border.sprite = curveRight;
        }
        else
        {
            border.sprite = curveLeft;
        }
    }

    /// <summary>
    /// Gets the fill amount for the power meter.
    /// </summary>
    /// <returns>fill amount</returns>
    public float GetFillAmount()
    {
        return powerMeter.fillAmount;
    }
}
