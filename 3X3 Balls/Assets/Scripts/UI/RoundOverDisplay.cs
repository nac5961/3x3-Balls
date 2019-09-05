using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class RoundOverDisplay : MonoBehaviour
{
    public GameObject panel;
    public GameObject text;
    public float displayDuration;

    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        timer = displayDuration;
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneInfo.instance.IsTurnOver && SceneInfo.instance.IsRoundOver)
        {
            if (timer > 0.0f)
            {
                if (timer == displayDuration)
                {
                    SetRoundResults();
                    DisplayUI();
                }

                timer -= Time.deltaTime;

                if (timer <= 0.0f)
                {
                    GetComponent<FadeToNextLevel>().ReadyToFade = true;
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

    private void SetRoundResults()
    {
        text.GetComponent<TextMeshProUGUI>().text = GameInfo.instance.CapturedBalls[GameInfo.instance.CapturedBalls.Count - 1] + " was scored first";
    }
}
