using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Courses
{
    Forest
}

public class GameInfo : MonoBehaviour
{
    public static GameInfo instance;

    private List<List<int>> playerScores;

    private string course;
    private int level;
    private int totalLevels;
    private int players;

    public List<List<int>> PlayerScores
    {
        get { return playerScores; }
        set { playerScores = value; }
    }
    public int Level
    {
        get { return level; }
        set { level = value; }
    }
    public int TotalLevels
    {
        get { return totalLevels; }
    }
    public int Players
    {
        get { return players; }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //Default players
        SetPlayers(2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Sets the number of players.
    /// </summary>
    /// <param name="num">number of players</param>
    public void SetPlayers(int num)
    {
        players = num;
    }

    /// <summary>
    /// Sets the variables to their default values to start the game.
    /// </summary>
    /// <param name="numLevels">number of levels being played</param>
    /// <param name="course">series of courses to play</param>
    public void SetupGame(int numLevels, Courses course)
    {
        this.course = course.ToString();
        level = 1;
        totalLevels = numLevels;

        playerScores = new List<List<int>>();

        for (int i = 0; i < players; i++)
        {
            playerScores.Add(new List<int>());

            for (int j = 0; j < totalLevels; j++)
            {
                playerScores[i].Add(0);
            }
        }
    }

    /// <summary>
    /// Loads the specified level.
    /// </summary>
    public void LoadLevel()
    {
        string levelName = course + " " + level;

        SceneManager.LoadScene(levelName);
    }

    /// <summary>
    /// Loads the main menu.
    /// </summary>
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
