using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShotUI : MonoBehaviour
{
    public Image ballImage;

    public Image cancelImage;
    public TextMeshProUGUI cancelText;
    public TextMeshProUGUI hitText;

    public Image border;
    public Sprite normal;
    public Sprite jump;
    public Sprite curve;

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
    /// Resets the properties in the power meter animation.
    /// </summary>
    public void ResetPowerMeter()
    {
        ballImage.sprite = UIGameInfo.instance.AimUI.GetComponent<AimUI>().Ball.sprite;

        cancelImage.gameObject.SetActive(true);
        cancelText.gameObject.SetActive(true);
        hitText.text = "Hold to\r\nset power";

        powerSet = false;
        powerMeter.fillAmount = minFill;
        fillSpeed = Mathf.Abs(fillSpeed);
    }

    public void ShowReleaseText()
    {
        cancelImage.gameObject.SetActive(false);
        cancelText.gameObject.SetActive(false);
        hitText.text = "Release to\r\nhit ball";
    }

    public void SetBorder(ShotType shot)
    {
        if (shot == ShotType.Normal)
        {
            border.sprite = normal;
        }
        else if (shot == ShotType.Jump)
        {
            border.sprite = jump;
        }
        else if (shot == ShotType.Curve)
        {
            border.sprite = curve;
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
