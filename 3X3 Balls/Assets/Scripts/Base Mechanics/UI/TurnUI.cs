using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TurnUI : MonoBehaviour
{
    public TextMeshProUGUI turn;

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
        SceneInfo.instance.DisableControls = true;

        timer = 0.0f;

        int player = SceneInfo.instance.GetCurrentPlayer() + 1;
        turn.text = "Player " + player + "'s Turn";
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
