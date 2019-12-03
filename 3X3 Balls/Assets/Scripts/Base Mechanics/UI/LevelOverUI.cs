using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelOverUI : MonoBehaviour
{
    public float spacing;
    public GameObject holePrefab;
    public GameObject totalPrefab;
    public GameObject playerNamePrefab;

    public RectTransform startingPos;

    public Button nextLevelButton;
    public Button mainMenuButton;

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
    /// Hides the next level button on the last level.
    /// </summary>
    private void SetupButtons()
    {
        //At the last level
        if (GameInfo.instance.Level == GameInfo.instance.TotalLevels)
        {
            //Hide the next level button
            nextLevelButton.gameObject.SetActive(false);

            //Center the main menu button
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
    /// Spawns the UI for the correct number of players and for the
    /// correct number of holes.
    /// </summary>
    public void SetupUI()
    {
        float holeUIWidth = holePrefab.GetComponent<RectTransform>().rect.width;
        Vector2 holeUIPos = new Vector2(startingPos.anchoredPosition.x + startingPos.rect.width / 2.0f, startingPos.anchoredPosition.y);

        //Holes
        for (int i = 0; i < GameInfo.instance.TotalLevels; i++)
        {
            holeUIPos = new Vector2(holeUIPos.x + holeUIWidth + spacing, holeUIPos.y);

            //Spawn hole UI
            GameObject holeUI = Instantiate(holePrefab, Vector3.zero, Quaternion.identity);
            holeUI.transform.SetParent(transform, false);
            holeUI.GetComponent<RectTransform>().anchoredPosition = holeUIPos;

            //Set hole number
            int number = i + 1;
            holeUI.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = number.ToString();
        }

        float totalUIWidth = totalPrefab.GetComponent<RectTransform>().rect.width;
        Vector2 totalUIPos = new Vector2(holeUIPos.x + totalUIWidth + spacing, holeUIPos.y);

        //Spawn total UI (for the label "Total")
        GameObject totalUI = Instantiate(totalPrefab, Vector3.zero, Quaternion.identity);
        totalUI.transform.SetParent(transform, false);
        totalUI.GetComponent<RectTransform>().anchoredPosition = totalUIPos;
        totalUI.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Total";

        //Save Scores (for current level)
        for (int i = 0; i < SceneInfo.instance.Scores.Count; i++)
        {
            GameInfo.instance.PlayerScores[i][GameInfo.instance.Level - 1] = SceneInfo.instance.Scores[i];
        }

        float xOffset = 25.0f; //This offset is needed in order to align the player name with the "Hole" label
        float playerNameUIWidth = playerNamePrefab.GetComponent<RectTransform>().rect.width;
        float playerNameUIHeight = playerNamePrefab.GetComponent<RectTransform>().rect.height;
        Vector2 playerNameUIPos = new Vector2(startingPos.anchoredPosition.x - xOffset, startingPos.anchoredPosition.y - startingPos.rect.height / 2.0f);

        //Players
        for (int i = 0; i < GameInfo.instance.Players; i++)
        {
            playerNameUIPos = new Vector2(playerNameUIPos.x, playerNameUIPos.y - playerNameUIHeight - spacing);
            holeUIPos = new Vector2(playerNameUIPos.x + playerNameUIWidth / 2.0f, playerNameUIPos.y);

            //Spawn player name UI
            GameObject playerNameUI = Instantiate(playerNamePrefab, Vector3.zero, Quaternion.identity);
            playerNameUI.transform.SetParent(transform, false);
            playerNameUI.GetComponent<RectTransform>().anchoredPosition = playerNameUIPos;

            //Set player number
            int number = i + 1;
            playerNameUI.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Player " + number;

            int total = 0;

            //Player Scores
            for (int j = 0; j < GameInfo.instance.TotalLevels; j++)
            {
                holeUIPos = new Vector2(holeUIPos.x + holeUIWidth + spacing, holeUIPos.y);

                //Spawn hole UI
                GameObject holeUI = Instantiate(holePrefab, Vector3.zero, Quaternion.identity);
                holeUI.transform.SetParent(transform, false);
                holeUI.GetComponent<RectTransform>().anchoredPosition = holeUIPos;

                //Set player score for hole
                int score = GameInfo.instance.PlayerScores[i][j];

                //Limit the score for display purposes
                if (score >= 100)
                {
                    string displayedScore = "99+";
                    holeUI.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = displayedScore;
                }
                else
                {
                    holeUI.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = score.ToString();
                }

                total += score;
            }

            totalUIPos = new Vector2(holeUIPos.x + totalUIWidth + spacing, holeUIPos.y);

            //Spawn total UI (for player)
            GameObject playerTotalUI = Instantiate(totalPrefab, Vector3.zero, Quaternion.identity);
            playerTotalUI.transform.SetParent(transform, false);
            playerTotalUI.GetComponent<RectTransform>().anchoredPosition = totalUIPos;

            //Limit the total for display purposes
            if (total >= 1000)
            {
                string displayedTotal = "999+";
                playerTotalUI.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = displayedTotal;
            }
            else
            {
                playerTotalUI.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = total.ToString();
            }
        }
    }
}
