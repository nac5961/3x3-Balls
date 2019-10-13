using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILevelManager : MonoBehaviour
{
    public Button forestLevelButton;

    // Start is called before the first frame update
    void Start()
    {
        forestLevelButton.onClick.AddListener(() => LoadForestLevels());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Sets up the game and loads the forest courses.
    /// </summary>
    private void LoadForestLevels()
    {
        GameInfo.instance.SetupGame(4, Courses.Forest);
        GameInfo.instance.LoadLevel();
    }
}
