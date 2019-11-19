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
    private List<GameObject> targetBalls;
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
    private bool dontEndTurn;

    //Rigidbody
    private float endTurnWaitTime; //Need to wait a few seconds before checking if a ball is moving after it is hit; Bug where it is flagged as not moving on hit even though ball is moving.
    private float nextTurnWaitTime;
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
    public List<GameObject> TargetBalls
    {
        get { return targetBalls; }
        set { targetBalls = value; }
    }
    public Material TargetBallMaterial
    {
        set { targetBallMaterial = value; }
    }
    public List<int> Scores
    {
        get { return scores; }
    }
    public bool GameStart
    {
        get { return gameStart; }
    }
    public bool Paused
    {
        get { return paused; }
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
    }

    public bool DisableControls
    {
        get { return disableControls; }
        set { disableControls = value; }
    }

    public bool DontEndTurn
    {
        set { dontEndTurn = value; }
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
        targetBalls = new List<GameObject>();

        finishedPlayers = new List<int>();
        scores = new List<int>();
        turns = new List<int>();
        currTurn = 0;

        gameStart = false;
        paused = false;

        isAiming = false;
        isTakingShot = false;
        isHit = false;
        isTurnOver = false;

        disableControls = false;
        dontEndTurn = false;

        endTurnWaitTime = 2.0f;
        nextTurnWaitTime = 0.0f;
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
        ManuallyEndLevel(); //DELETE ME
        if (gameStart)
        {
            if (!disableControls)
            {
                if (Input.GetButtonDown("Pause"))
                {
                    TogglePause();
                }
            }

            if (!paused)
            {
                if (isHit)
                {
                    if (timer < endTurnWaitTime)
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
                    if (timer < nextTurnWaitTime)
                    {
                        timer += Time.deltaTime;
                    }
                    else
                    {
                        StartNextTurn();
                    }
                }
            }
        }
    }

    /// <summary>
    /// DELETE ME
    /// </summary>
    private void ManuallyEndLevel()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            gameStart = false;
            UIGameInfo.instance.DisplayLevelOverUI();
        }
    }

    /// <summary>
    /// Assigns player turns based on their score.
    /// </summary>
    private void AssignTurns()
    {
        List<KeyValuePair<int, int>> players = new List<KeyValuePair<int, int>>(); //holds player (key) and their total (value)
        List<int> totals = new List<int>();

        for (int i = 0; i < GameInfo.instance.PlayerScores.Count; i++)
        {
            int total = 0;

            for (int j = 0; j < GameInfo.instance.PlayerScores[i].Count; j++)
            {
                //Only consider the scores up to the current level that we're at.
                // **Without this, we will be considering all of the 0 scores on levels that were not played.**
                if (j == GameInfo.instance.Level)
                    break;

                total += GameInfo.instance.PlayerScores[i][j];
            }

            players.Add(new KeyValuePair<int, int>(i, total));
            totals.Add(total);
        }

        //On first level so randomly choose player turn
        if (GameInfo.instance.Level == 1)
        {
            for (int i = 0; i < GameInfo.instance.Players; i++)
            {
                int rand = Random.Range(0, players.Count);

                //Add player and remove them to not count them again
                turns.Add(players[rand].Key);
                players.RemoveAt(rand);
            }
        }

        //Players have scores so order turns by scores
        else
        {
            //Order from least to greatest
            totals.Sort();

            //Make highest scores (players in last) go first and lowest scores (players in first) go last
            for (int i = totals.Count - 1; i >= 0; i--)
            {
                for (int j = 0; j < players.Count; j++)
                {
                    if (players[j].Value == totals[i])
                    {
                        //Add player and remove them to not count them again
                        turns.Add(players[j].Key);
                        players.RemoveAt(j);
                        break;
                    }
                }
            }
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

        //Show the player turn if more than one person is playing
        if (GameInfo.instance.Players > 1)
        {
            UIGameInfo.instance.DisplayTurnUI();
        }
        else
        {
            UIGameInfo.instance.DisplayAimUI();
        }
    }

    /// <summary>
    /// Pauses and resumes the game.
    /// </summary>
    public void TogglePause()
    {
        paused = !paused;

        if (paused)
        {
            UIGameInfo.instance.DisplayPauseUI();
        }
        else
        {
            //Only wait to check balls if a ball was moving beforehand.
            // **Need this check so the turn can end if players spam the pause button.**
            for (int i = 0; i < balls.Count; i++)
            {
                //Skip over balls that have been scored already (balls that have been hit into holes)
                if (balls[i].GetComponent<Ball>().IsScored)
                    continue;

                //Need to wait a few seconds again before checking if a ball is moving after it is resumed; Bug where it is flagged as not moving on hit even though ball is moving.
                if (balls[i].GetComponent<Ball>().WasMoving)
                {
                    timer = 0.0f;
                    break;
                }
            }

            UIGameInfo.instance.HidePauseUI();
        }
    }

    /// <summary>
    /// Switches the ball that players need to score to the player's ball who scored last.
    /// Marks the player who scored as finished.
    /// </summary>
    public void SwitchTargetBall()
    {
        //This check prevents duplicate entries when multiple balls
        //are scored during one turn.
        if (!finishedPlayers.Contains(GetCurrentPlayer()))
        {
            finishedPlayers.Add(GetCurrentPlayer());

            //Only make a new target ball if there are players still playing
            if (finishedPlayers.Count < GameInfo.instance.Players)
            {
                activeBall.GetComponent<Renderer>().material = targetBallMaterial;
                targetBalls.Add(activeBall);
            }
        }
    }

    /// <summary>
    /// Sets the active ball based on which player's turn it is.
    /// </summary>
    public void SetActiveBall()
    {
        activeBall = balls[GetCurrentPlayer()];
    }

    /// <summary>
    /// Increases the player's score.
    /// </summary>
    public void UpdatePlayerScore()
    {
        scores[GetCurrentPlayer()]++;
    }

    /// <summary>
    /// Gives players a bonus (bonus subtracts from the stroke count).
    /// </summary>
    /// <param name="bonus"></param>
    public void AddBonusScore(int bonus)
    {
        scores[GetCurrentPlayer()] -= bonus;

        if (scores[GetCurrentPlayer()] <= 0)
        {
            scores[GetCurrentPlayer()] = 1;
        }
    }

    /// <summary>
    /// Gets the player who is currently on their turn.
    /// </summary>
    /// <returns></returns>
    public int GetCurrentPlayer()
    {
        return turns[currTurn];
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
                continue;

            //Balls still moving
            if (!balls[i].GetComponent<Rigidbody>().IsSleeping())
            {
                stillMoving = true;
                break;
            }
        }

        if (!stillMoving && !dontEndTurn)
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
        timer = 0.0f;
        isTurnOver = false;
        isAiming = true;

        int prevPlayer = GetCurrentPlayer();

        //Set the next player
        for (int i = 0; i < GameInfo.instance.Players; i++)
        {
            currTurn++;

            if (currTurn >= GameInfo.instance.Players)
            {
                currTurn = 0;
            }

            //Only stay on the player if they did not already finish
            if (!finishedPlayers.Contains(GetCurrentPlayer()))
                break;
        }

        //Update all balls' previous positions
        for (int i = 0; i < balls.Count; i++)
        {
            Ball ball = balls[i].GetComponent<Ball>();

            if (!ball.IsScored)
            {
                //Respawn target ball if it was knocked out of the scoring area
                if (targetBalls.Contains(ball.gameObject) && !ball.InBounds)
                {
                    ball.Respawn();
                }

                ball.UpdatePrevPos();
                ball.ScoreCount = 0;
            }
        }

        //Set new active ball
        SetActiveBall();

        //Setup camera
        Camera.main.GetComponent<ThirdPersonCamera>().Target = activeBall.transform;
        Camera.main.GetComponent<ThirdPersonCamera>().UpdatePlayerRotations(prevPlayer, GetCurrentPlayer());

        //If more than one player is still playing, show the player's turn
        if (finishedPlayers.Count < GameInfo.instance.Players - 1)
        {
            //Set stroke count to next player's stroke count
            UIGameInfo.instance.GeneralUI.GetComponent<GeneralUI>().SetStrokeCount();

            UIGameInfo.instance.DisplayTurnUI();
        }

        //Otherwise skip the turn display
        else
        {
            UIGameInfo.instance.DisplayAimUI();
        }
    }
}
