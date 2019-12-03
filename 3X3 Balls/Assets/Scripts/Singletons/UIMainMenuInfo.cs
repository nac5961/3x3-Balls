using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMainMenuInfo : MonoBehaviour
{
    public GameObject instructionsUI;
    public GameObject optionsUI;
    public GameObject creditsUI;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Quits  the game.
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
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
    /// Displays the instructions UI.
    /// </summary>
    public void DisplayInstructions()
    {
        HideAllUI();
        DisplayUI(instructionsUI);
    }

    /// <summary>
    /// Displays the options UI.
    /// </summary>
    public void DisplayOptions()
    {
        HideAllUI();
        DisplayUI(optionsUI);
    }

    /// <summary>
    /// Displays the credits UI.
    /// </summary>
    public void DisplayCredits()
    {
        HideAllUI();
        DisplayUI(creditsUI);
    }

    /// <summary>
    /// Hides all UI.
    /// </summary>
    private void HideAllUI()
    {
        HideUI(instructionsUI);
        HideUI(optionsUI);
        HideUI(creditsUI);
    }
}
