using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShotUI : MonoBehaviour
{
    public GameObject powerMeter;
    public float fillSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneInfo.instance.GameStart && !SceneInfo.instance.Paused)
        {
            if (SceneInfo.instance.IsTakingShot)
            {
                ProcessPowerInput();
            }
        }
    }

    /// <summary>
    /// Processes input for moving the power meter.
    /// </summary>
    private void ProcessPowerInput()
    {
        Image img = powerMeter.GetComponent<Image>();

        float percent = img.fillAmount;

        if (Input.GetButton("Increase Power"))
        {
            percent += fillSpeed * Time.deltaTime;
        }
        else if (Input.GetButton("Decrease Power"))
        {
            percent -= fillSpeed * Time.deltaTime;
        }

        percent = Mathf.Clamp(percent, 0.0f, 1.0f);

        img.fillAmount = percent;
    }

    /// <summary>
    /// Sets the fill amount back to 0.
    /// </summary>
    public void ResetPowerMeter()
    {
        powerMeter.GetComponent<Image>().fillAmount = 0.0f;
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
