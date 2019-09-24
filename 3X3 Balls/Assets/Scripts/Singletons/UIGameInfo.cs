using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGameInfo : MonoBehaviour
{
    public static UIGameInfo instance;

    public GameObject aimUI;
    public GameObject shotUI;
    public GameObject turnUI;
    public GameObject levelOverUI;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Displays UI.
    /// </summary>
    /// <param name="ui">the UI to display</param>
    private void DisplayUI(GameObject ui)
    {
        if (!ui.activeSelf)
        {
            ui.SetActive(true);
        }
    }

    /// <summary>
    /// Hides UI.
    /// </summary>
    /// <param name="ui">the UI to hide</param>
    private void HideUI(GameObject ui)
    {
        if (ui.activeSelf)
        {
            ui.SetActive(false);
        }
    }

    /// <summary>
    /// Displays the UI for taking a shot.
    /// </summary>
    public void DisplayShotUI()
    {
        HideUI(aimUI);
        DisplayUI(shotUI);
    }

    /// <summary>
    /// Hides the UI for taking a shot.
    /// Will display the UI for aiming if the shot was cancelled.
    /// </summary>
    /// <param name="tookShot">if the player hit the ball</param>
    public void HideShotUI(bool tookShot)
    {
        HideUI(shotUI);

        if (!tookShot)
        {
            DisplayUI(aimUI);
        }
    }

    /// <summary>
    /// Displays the UI for showing player turns.
    /// </summary>
    public void DisplayTurnUI()
    {
        turnUI.GetComponent<TurnUI>().SetupUI();

        DisplayUI(turnUI);
    }

    /// <summary>
    /// Hides the UI for showing player turns.
    /// </summary>
    public void HideTurnUI()
    {
        HideUI(turnUI);
        DisplayUI(aimUI);
    }

    /// <summary>
    /// Displays the UI for when the level is complete.
    /// </summary>
    public void DisplayLevelOverUI()
    {
        levelOverUI.GetComponent<LevelOverUI>().SetupScores();

        DisplayUI(levelOverUI);
    }
}
