using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInfo : MonoBehaviour
{
    public static GameInfo instance;

    public Material targetMaterial;

    private int level;
    private int players;
    private int fastestPlayer;

    public Material TargetMaterial
    {
        get { return targetMaterial; }
    }
    public int Players
    {
        get { return players; }
    }
    public int FastestPlayer
    {
        get { return fastestPlayer; }
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
    public void SetupGame()
    {
        level = 1;
        fastestPlayer = -1;
    }

    /// <summary>
    /// TEMPORARY METHOD for loading the first level 
    /// </summary>
    public void LoadLevel()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("NewMockup");
    }
}
