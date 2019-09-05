using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BallTypeDisplay : MonoBehaviour
{
    public GameObject panel;
    public GameObject p1Text;
    public GameObject p2Text;
    public float displayDuration;

    private bool beginDisplay;

    // Start is called before the first frame update
    void Start()
    {
        if (GameInfo.instance.IsFirstCourse)
        {
            SceneInfo.instance.DisableControls = true;
            beginDisplay = true;

            DisplayUI();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GameInfo.instance.IsFirstCourse)
        {
            if (beginDisplay)
            {
                beginDisplay = false;
                SetBallType();
            }
            if (displayDuration > 0.0f)
            {
                displayDuration -= Time.deltaTime;

                if (displayDuration <= 0.0f)
                {
                    HideUI();
                    GetComponent<TurnDisplay>().StartDisplay();
                }
            }
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

    private void SetBallType()
    {
        p1Text.GetComponent<TextMeshProUGUI>().text = GameInfo.instance.P1Type.ToString();
        p2Text.GetComponent<TextMeshProUGUI>().text = GameInfo.instance.P2Type.ToString();
    }
}
