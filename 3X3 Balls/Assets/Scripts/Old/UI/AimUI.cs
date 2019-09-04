using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AimUI : MonoBehaviour
{
    public GameObject ui;
    public GameObject powerMeter;
    public float animationSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameInfo.instance.IsAiming)
        {
            DisplayUI();

            if (GameInfo.instance.IsTakingShot)
            {
                AnimatePowerMeter();
            }
        }
        else
        {
            HideUI();
        }
    }

    private void DisplayUI()
    {
        if (!ui.activeSelf)
        {
            ui.SetActive(true);
        }
    }

    private void HideUI()
    {
        if (ui.activeSelf)
        {
            ui.SetActive(false);
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


}
