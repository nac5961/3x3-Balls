using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGameInfo : MonoBehaviour
{
    public static UIGameInfo instance;

    public GameObject generalUI;
    public GameObject aimUI;
    public GameObject shotUI;
    public GameObject hitUI;
    public GameObject turnUI;
    public GameObject levelOverUI;
    public GameObject pauseUI;

    public GameObject GeneralUI
    {
        get { return generalUI; }
    }
    public GameObject ShotUI
    {
        get { return shotUI; }
    }

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
        ui.SetActive(true);
    }

    /// <summary>
    /// Hides UI.
    /// </summary>
    /// <param name="ui">the UI to hide</param>
    private void HideUI(GameObject ui)
    {
        ui.SetActive(false);
    }

    /// <summary>
    /// Displays the UI for aiming.
    /// </summary>
    public void DisplayAimUI()
    {
        HideUI(hitUI);
        DisplayUI(aimUI);
    }

    /// <summary>
    /// Displays the UI for taking a shot.
    /// </summary>
    public void DisplayShotUI()
    {
        shotUI.GetComponent<ShotUI>().ResetPowerMeter();

        HideUI(aimUI);
        DisplayUI(shotUI);
    }

    /// <summary>
    /// Hides the UI for taking a shot.
    /// </summary>
    /// <param name="tookShot">if the player hit the ball</param>
    /// <param name="maxHit">if the player hit the ball with maximum force</param>
    public void HideShotUI(bool tookShot, bool maxHit = false)
    {
        HideUI(shotUI);

        if (tookShot)
        {
            if (maxHit)
            {
                hitUI.GetComponent<HitUI>().SetupMaxHitUI();
            }

            DisplayUI(hitUI);
        }
        else
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

        HideUI(generalUI);
        HideUI(hitUI);
        DisplayUI(turnUI);
    }

    /// <summary>
    /// Hides the UI for showing player turns.
    /// </summary>
    public void HideTurnUI()
    {
        HideUI(turnUI);
        DisplayUI(generalUI);
        DisplayUI(aimUI);
    }

    /// <summary>
    /// Displays the UI for when the level is complete.
    /// </summary>
    public void DisplayLevelOverUI()
    {
        levelOverUI.GetComponent<LevelOverUI>().SetupScores();

        HideUI(generalUI);
        HideUI(hitUI);
        DisplayUI(levelOverUI);
    }

    /// <summary>
    /// Displays the UI for when the game is paused.
    /// </summary>
    public void DisplayPauseUI()
    {
        DisplayUI(pauseUI);
    }

    /// <summary>
    /// Hides the UI for when the game is paused.
    /// </summary>
    public void HidePauseUI()
    {
        HideUI(pauseUI);
    }
}
