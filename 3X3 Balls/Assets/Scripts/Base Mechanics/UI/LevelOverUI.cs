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
    /// Sets the scores shown at the end of each level.
    /// </summary>
    public void SetupScores()
    {
        string p1 = "Player 1 - ";
        string p2 = "Player 2 - ";
        int p1Total = 0;
        int p2Total = 0;

        //Store scores
        for (int i = 0; i < SceneInfo.instance.Scores.Count; i++)
        {
            GameInfo.instance.PlayerScores[i][GameInfo.instance.Level - 1] = SceneInfo.instance.Scores[i];
        }

        //Display scores
        for (int i = 0; i < GameInfo.instance.PlayerScores.Count; i++)
        {
            for (int j = 0; j < GameInfo.instance.PlayerScores[i].Count; j++)
            {
                //Player 1
                if (i == 0)
                {
                    p1 += GameInfo.instance.PlayerScores[i][j] + " ";
                    p1Total += GameInfo.instance.PlayerScores[i][j];
                }

                //Player 2
                else if (i == 1)
                {
                    p2 += GameInfo.instance.PlayerScores[i][j] + " ";
                    p2Total += GameInfo.instance.PlayerScores[i][j];
                }
            }
        }

        p1Score.GetComponent<TextMeshProUGUI>().text = p1 + "Total: " + p1Total.ToString();
        p2Score.GetComponent<TextMeshProUGUI>().text = p2 + "Total: " + p2Total.ToString();
    }
}
