using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneInfo : MonoBehaviour
{
    public static SceneInfo instance;

    private GameObject cue;
    private List<GameObject> balls;
    private GameObject activeBall;
    private GameObject targetBall;
    private Material targetBallMaterial;

    private List<int> scores;
    private List<int> turns;
    private int currTurn;

    private bool gameStart;
    private bool paused;

    private bool isAiming;
    private bool isTakingShot;
    private bool isHit;
    private bool isTurnOver;
    private bool isLevelOver;
    private bool isGameOver;

    private bool disableControls;

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

    public bool IsLevelOver
    {
        get { return isLevelOver; }
        set { isLevelOver = value; }
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

        gameStart = false;
        paused = false;

        isAiming = false;
        isTakingShot = false;
        isHit = false;
        isTurnOver = false;
        isLevelOver = false;
        isGameOver = false;

        disableControls = false;

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
                EndTurn();
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
    /// Switches the ball that players need to score to the player's ball who scored last.
    /// </summary>
    public void SwitchTargetBall()
    {
        GameObject oldTarget = targetBall;
        targetBall = activeBall;
        targetBall.GetComponent<Renderer>().material = targetBallMaterial;

        //Remove the player who scored since they already finished the course
        turns.RemoveAt(currTurn);

        //Remove the ball scored
        balls.Remove(oldTarget);
    }

    /// <summary>
    /// Sets the active ball based on which player's turn it is.
    /// </summary>
    public void SetActiveBall()
    {
        activeBall = balls[turns[currTurn]];
    }

    /// <summary>
    /// Gets the player who is currently on their turn.
    /// </summary>
    /// <returns></returns>
    public int GetCurrentPlayer()
    {
        return turns[currTurn];
    }

    private void EndTurn()
    {
        for (int i = 0; i < balls.Count; i++)
        {
            //Balls still moving
            if (!balls[i].GetComponent<Rigidbody>().IsSleeping())
            {
                break;
            }

            //All balls stopped
            if (i == balls.Count - 1)
            {
                isHit = false;
                isTurnOver = true;

                //TEMPORARY
                //scores[turns[currTurn]]++;
                currTurn++;

                if (currTurn >= turns.Count)
                {
                    currTurn = 0;
                }

                isTurnOver = false;
                isAiming = true;
                SetActiveBall();
                Camera.main.GetComponent<ThirdPersonCamera>().Target = activeBall.transform;
                GameObject.Find("Canvas").GetComponent<TurnDisplay>().StartDisplay();
            }
        }
    }
}
