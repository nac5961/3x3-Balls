using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILevelManager : MonoBehaviour
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
    /// Sets up the game and loads the forest courses.
    /// Called in the UI.
    /// </summary>
    public void LoadForestLevels()
    {
        GameInfo.instance.SetupGame(7, Courses.Forest);
        GameInfo.instance.LoadLevel();
    }
}
