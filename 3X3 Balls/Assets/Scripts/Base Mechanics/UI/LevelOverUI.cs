using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelOverUI : MonoBehaviour
{
    public Button nextLevelButton;
    public Button mainMenuButton;

    public GameObject p1Score;
    public GameObject p2Score;
    public GameObject p3Score;
    public GameObject p4Score;

    // Start is called before the first frame update
    void Start()
    {
        SetupButtons();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Configures the buttons based on the level.
    /// </summary>
    private void SetupButtons()
    {
        if (GameInfo.instance.Level == GameInfo.instance.TotalLevels)
        {
            nextLevelButton.gameObject.SetActive(false);

            RectTransform mmButonRect = mainMenuButton.GetComponent<RectTransform>();
            mmButonRect.anchoredPosition = new Vector2(0.0f, mmButonRect.anchoredPosition.y);
        }
    }

    /// <summary>
    /// Loads the main menu.
    /// </summary>
    public void GoToMainMenu()
    {
        GameInfo.instance.LoadMainMenu();
    }

    /// <summary>
    /// Loads the next level.
    /// </summary>
    public void GoToNextLevel()
    {
        GameInfo.instance.Level++;
        GameInfo.instance.LoadLevel();
    }

    /// <summary>
    /// REDO
    /// Sets the scores shown at the end of each level.
    /// </summary>
    public void SetupScores()
    {
        List<KeyValuePair<string, string>> players = new List<KeyValuePair<string, string>>(); //key = all individual scores; value = total score

        //Store scores
        for (int i = 0; i < SceneInfo.instance.Scores.Count; i++)
        {
            GameInfo.instance.PlayerScores[i][GameInfo.instance.Level - 1] = SceneInfo.instance.Scores[i];
        }

        //Get score info
        for (int i = 0; i < GameInfo.instance.PlayerScores.Count; i++)
        {
            int total = 0;
            string info = "Player " + (i + 1) + ": ";

            for (int j = 0; j < GameInfo.instance.PlayerScores[i].Count; j++)
            {
                total += GameInfo.instance.PlayerScores[i][j];
                info += GameInfo.instance.PlayerScores[i][j] + " ";
            }

            players.Add(new KeyValuePair<string, string>(info, "Total: " + total.ToString()));
        }

        //Display score info
        for (int i = 0; i < players.Count; i++)
        {
            //Player 1
            if (i == 0)
                p1Score.GetComponent<TextMeshProUGUI>().text = players[i].Key + players[i].Value;
            //Player 2
            else if (i == 1)
                p2Score.GetComponent<TextMeshProUGUI>().text = players[i].Key + players[i].Value;
            //Player 3
            else if (i == 2)
                p3Score.GetComponent<TextMeshProUGUI>().text = players[i].Key + players[i].Value;
            //Player 4
            else if (i == 3)
                p4Score.GetComponent<TextMeshProUGUI>().text = players[i].Key + players[i].Value;
        }
    }
}
