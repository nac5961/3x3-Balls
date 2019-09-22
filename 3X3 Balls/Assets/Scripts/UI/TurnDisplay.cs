using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TurnDisplay : MonoBehaviour
{
    public GameObject panel;
    public GameObject text;
    public float displayDuration;

    private float timer;
    private bool beginDisplay;

    // Start is called before the first frame update
    void Start()
    {
        //TEMPORARY
        StartDisplay();
    }

    // Update is called once per frame
    void Update()
    {
        if (beginDisplay)
        {
            beginDisplay = false;

            SetTurn();
            DisplayUI();
        }

        if (timer > 0.0f)
        {
            timer -= Time.deltaTime;

            if (timer <= 0.0f)
            {
                SceneInfo.instance.DisableControls = false;
                HideUI();
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

    public void StartDisplay()
    {
        timer = displayDuration;
        beginDisplay = true;

        SceneInfo.instance.DisableControls = true;
    }

    private void SetTurn()
    {
        int player = SceneInfo.instance.GetCurrentPlayer() + 1;
        text.GetComponent<TextMeshProUGUI>().text = "Player " + player + "'s Turn";
    }
}
