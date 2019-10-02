using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseUI : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Resumes the game.
    /// </summary>
    public void ResumeGame()
    {
        SceneInfo.instance.TogglePause();
    }

    /// <summary>
    /// Goes back to the main menu.
    /// </summary>
    public void QuitGame()
    {
        GameInfo.instance.LoadMainMenu();
    }
}
