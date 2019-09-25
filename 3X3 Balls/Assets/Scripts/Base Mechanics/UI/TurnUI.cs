using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TurnUI : MonoBehaviour
{
    public GameObject text;

    public float duration;
    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneInfo.instance.GameStart && !SceneInfo.instance.Paused)
        {
            DisplayUI();
        }
    }

    /// <summary>
    /// Configures the UI before it is displayed.
    /// </summary>
    public void SetupUI()
    {
        timer = 0.0f;
        SceneInfo.instance.DisableControls = true;

        text.GetComponent<TextMeshProUGUI>().text = "Player " + SceneInfo.instance.GetCurrentPlayer() + "'s Turn";
    }

    /// <summary>
    /// Displays the UI for a set duration.
    /// </summary>
    private void DisplayUI()
    {
        if (timer < duration)
        {
            timer += Time.deltaTime;

            if (timer >= duration)
            {
                SceneInfo.instance.DisableControls = false;
                UIGameInfo.instance.HideTurnUI();
            }
        }
    }
}
