using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelOverUI : MonoBehaviour
{
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
    /// Configures the buttons based on the level.
    /// </summary>
    private void SetupButtons()
    {
        if (GameInfo.instance.Level == GameInfo.instance.TotalLevels)
        {
            nextLevelButton.gameObject.SetActive(false);

            mainMenuButton.transform.position = new Vector3(0.0f, mainMenuButton.transform.position.y, mainMenuButton.transform.position.z);
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
        for (int i = 0; i < GameInfo.instance.PlayerScores.Count; i++)
        {
            
        }
    }
}
