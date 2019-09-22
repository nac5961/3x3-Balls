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

            powerMeter.GetComponent<Image>().fillAmount = 0.0f;
        }
    }

    private void HideAnimation()
    {
        if (powerMeter.activeSelf)
        {
            powerMeter.SetActive(false);

            powerMeter.GetComponent<Image>().fillAmount = 0.0f;
        }
    }

    private void AnimatePowerMeter()
    {
        Image img = powerMeter.GetComponent<Image>();

        float percent = img.fillAmount;

        if (Input.GetButton("Increase Power"))
        {
            percent += animationSpeed * Time.deltaTime;
        }
        else if (Input.GetButton("Decrease Power"))
        {
            percent -= animationSpeed * Time.deltaTime;
        }

        percent = Mathf.Clamp(percent, 0.0f, 1.0f);

        img.fillAmount = percent;
    }

    public float GetFillAmount()
    {
        return powerMeter.GetComponent<Image>().fillAmount;
    }
}
