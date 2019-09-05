using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AimDisplay : MonoBehaviour
{
    public GameObject panel;
    public GameObject powerMeter;
    public float animationSpeed;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (SceneInfo.instance.IsAiming)
        {
            DisplayUI();

            if (SceneInfo.instance.IsTakingShot)
            {
                DisplayAnimation();
                AnimatePowerMeter();
            }
            else
            {
                HideAnimation();
            }
        }
        else
        {
            HideUI();
        }
    }

    private void DisplayUI()
    {
        if (!panel.activeSelf)
        {
            panel.SetActive(true);
        }
    }

    private void HideUI()
    {
        if (panel.activeSelf)
        {
            panel.SetActive(false);
        }
    }

    private void DisplayAnimation()
    {
        if (!powerMeter.activeSelf)
        {
            powerMeter.SetActive(true);

            //Reset values to animate from beginning
            animationSpeed = Mathf.Abs(animationSpeed);
            powerMeter.GetComponent<Image>().fillAmount = 0.0f;
        }
    }

    private void HideAnimation()
    {
        if (powerMeter.activeSelf)
        {
            powerMeter.SetActive(false);
        }
    }

    private void AnimatePowerMeter()
    {
        Image img = powerMeter.GetComponent<Image>();

        float percent = img.fillAmount;
        percent += animationSpeed * Time.deltaTime;
        percent = Mathf.Clamp(percent, 0.0f, 1.0f);

        img.fillAmount = percent;

        if (percent >= 1.0f || percent <= 0.0f)
        {
            animationSpeed = -animationSpeed;
        }
    }

    public float GetFillAmount()
    {
        return powerMeter.GetComponent<Image>().fillAmount;
    }
}
