using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneInfo : MonoBehaviour
{
    public static SceneInfo instance;

    //GameObjects
    private GameObject cue;
    private List<GameObject> balls;
    private GameObject activeBall;
    private GameObject targetBall;
    private Material targetBallMaterial;

    //Turns and Scores
    private List<int> finishedPlayers;
    private List<int> scores;
    private List<int> turns;
    private int currTurn;

    //Gameplay
    private bool gameStart;
    private bool paused;

    private bool isAiming;
    private bool isTakingShot;
    private bool isHit;
    private bool isTurnOver;

    private bool disableControls;

    //Rigidbody
    private float waitTime; //Need to wait a few seconds before checking a balls velocity after it is hit; Bug where velocity might be 0 on hit even though ball is moving.
    private float timer;

    public GameObject Cue
    {
        get { return cue; }
        set { cue = value; }
    }
    public List<GameObject> Balls
    {
        get { return balls; }
        set { balls = value; }
    }
    public GameObject ActiveBall
    {
        get { return activeBall; }
    }
    public GameObject TargetBall
    {
        get { return targetBall; }
        set { targetBall = value; }
    }
    public Material TargetBallMaterial
    {
        set { targetBallMaterial = value; }
    }
    public bool GameStart
    {
        get { return gameStart; }
        set { gameStart = value; }
    }
    public bool Paused
    {
        get { return paused; }
        set { paused = value; }
    }
    public bool IsAiming
    {
        get { return isAiming; }
        set { isAiming = value; }
    }

    public bool IsTakingShot
    {
        get { return isTakingShot; }
        set { isTakingShot = value; }
    }

    public bool IsHit
    {
        get { return isHit; }
        set { isHit = value; }
    }

    public bool IsTurnOver
    {
        get { return isTurnOver; }
        set { isTurnOver = value; }
    }

    public bool DisableControls
    {
        get { return disableControls; }
        set { disableControls = value; }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        //Initialize in Awake() so that other scripts can access these variables on Start()
        balls = new List<GameObject>();

        scores = new List<int>();
        turns = new List<int>();
        currTurn = 0;
        finishedPlayers = new List<int>();

        gameStart = false;
        paused = false;

        isAiming = false;
        isTakingShot = false;
        isHit = false;
        isTurnOver = false;

        disableControls = false;

        waitTime = 2.0f;
        timer = 0.0f;

        InitializeScores();
        AssignTurns();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (gameStart && !paused)
        {
            if (isHit)
            {
                if (timer < waitTime)
                {
                    timer += Time.deltaTime;
                }
                else
                {
                    EndTurn();
                }
            }
            else if (isTurnOver)
            {
                StartNextTurn();
            }
        }
    }

    /// <summary>
    /// Assigns player turns randomly while making the player who won the previous
    /// round go last.
    /// </summary>
    private void AssignTurns()
    {
        int fastestPlayer = GameInfo.instance.FastestPlayer;

        //Get list of players
        List<int> players = new List<int>();
        for (int i = 0; i < GameInfo.instance.Players; i++)
        {
            players.Add(i);
        }

        //Assign turns randomly
        for (int i = 0; i < GameInfo.instance.Players; i++)
        {
            int rand = Random.Range(0, players.Count);

            turns.Add(players[rand]);
            players.RemoveAt(rand);
        }

        //Make the fastest player go last
        if (turns.Remove(fastestPlayer))
        {
            turns.Add(fastestPlayer);
        }
    }

    /// <summary>
    /// Sets all of the player's scores to 0.
    /// </summary>
    private void InitializeScores()
    {
        for (int i = 0; i < GameInfo.instance.Players; i++)
        {
            scores.Add(0);
        }
    }

    /// <summary>
    /// Starts the game.
    /// </summary>
    public void StartGame()
    {
        gameStart = true;
        isAiming = true;
    }

    /// <summary>
    /// Switches the ball that players need to score to the player's ball who scored last.
    /// Marks the player who scored as finished.
    /// </summary>
    public void SwitchTargetBall()
    {
        if (finishedPlayers.Count == GameInfo.instance.Players - 1)
        {
            finishedPlayers.Add(turns[currTurn]);
        }
        else

        targetBall = activeBall;
        targetBall.GetComponent<Renderer>().material = targetBallMaterial;

        finishedPlayers.Add(turns[currTurn]);
    }

    /// <summary>
    /// Sets the active ball based on which player's turn it is.
    /// </summary>
    public void SetActiveBall()
    {
        activeBall = balls[turns[currTurn]];
    }

    /// <summary>
    /// Increases the player's score.
    /// </summary>
    public void UpdatePlayerScore()
    {
        scores[turns[currTurn]]++;
    }

    /// <summary>
    /// Gets the player who is currently on their turn.
    /// </summary>
    /// <returns></returns>
    public int GetCurrentPlayer()
    {
        return turns[currTurn] + 1;
    }

    /// <summary>
    /// Checks if all the balls have stopped moving so the player's turn can end.
    /// </summary>
    private void EndTurn()
    {
        bool stillMoving = false;

        for (int i = 0; i < balls.Count; i++)
        {
            //Skip over balls that have been scored already (balls that have been hit into holes)
            if (balls[i].GetComponent<Ball>().IsScored)
            {
                continue;
            }

            //Balls still moving
            if (!balls[i].GetComponent<Rigidbody>().IsSleeping())
            {
                stillMoving = true;
                break;
            }
        }

        if (!stillMoving)
        {
            timer = 0.0f;
            isHit = false;
            isTurnOver = true;

            //End level
            if (finishedPlayers.Count == GameInfo.instance.Players)
            {
                gameStart = false;
                UIGameInfo.instance.DisplayLevelOverUI();
            }
        }
    }

    /// <summary>
    /// Starts the next player's turn.
    /// </summary>
    private void StartNextTurn()
    {
        isTurnOver = false;
        isAiming = true;

        //Set the current player
        for (int i = 0; i < GameInfo.instance.Players; i++)
        {
            currTurn++;

            if (currTurn >= GameInfo.instance.Players)
            {
                currTurn = 0;
            }

            //Only stay on the current player if the player did not already finish
            if (!finishedPlayers.Contains(turns[currTurn]))
            {
                break;
            }
        }

        SetActiveBall();
        Camera.main.GetComponent<ThirdPersonCamera>().Target = activeBall.transform;

        UIGameInfo.instance.DisplayTurnUI();
    }
}
