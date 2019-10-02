using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShotUI : MonoBehaviour
{
    public GameObject powerMeter;
    public float fillSpeed;
    public float minFill;

    // Start is called before the first frame update
    void Start()
    {
        powerMeter.GetComponent<Image>().fillAmount = minFill;
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
        Image img = powerMeter.GetComponent<Image>();

        float percent = img.fillAmount;

        percent += fillSpeed * Time.deltaTime;
        percent = Mathf.Clamp(percent, minFill, 1.0f);
        img.fillAmount = percent;

        //Reverse
        if (percent == minFill || percent == 1.0f)
        {
            fillSpeed = -fillSpeed;
        }
    }

    /// <summary>
    /// Resets the properties in the power meter animation.
    /// </summary>
    public void ResetPowerMeter()
    {
        powerMeter.GetComponent<Image>().fillAmount = minFill;
        fillSpeed = Mathf.Abs(fillSpeed);
    }

    /// <summary>
    /// Gets the fill amount for the power meter.
    /// </summary>
    /// <returns>fill amount</returns>
    public float GetFillAmount()
    {
        return powerMeter.GetComponent<Image>().fillAmount;
    }
}
