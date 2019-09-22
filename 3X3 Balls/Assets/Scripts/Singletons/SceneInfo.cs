using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneInfo : MonoBehaviour
{
    public static SceneInfo instance;

    private GameObject cue;
    private List<GameObject> balls;
    private GameObject activeBall;

    private List<int> scores;
    private List<int> turns;
    private int currTurn;

    private bool isAiming;
    private bool isTakingShot;
    private bool isTurnOver;
    private bool isRoundOver;

    private bool disableControls;

    public GameObject Cue
    {
        get { return cue; }
        set { cue = value; }
    }
    public List<GameObject> Balls
    {
        set { balls = value; }
    }
    public GameObject ActiveBall
    {
        get { return activeBall; }
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

    public bool IsTurnOver
    {
        get { return isTurnOver; }
        set { isTurnOver = value; }
    }

    public bool IsRoundOver
    {
        get { return isRoundOver; }
        set { isRoundOver = value; }
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
    }

    // Start is called before the first frame update
    void Start()
    {
        scores = new List<int>();
        turns = new List<int>();
        currTurn = 0;

        isAiming = true; //make false
        isTakingShot = false;
        isTurnOver = false;
        isRoundOver = false;

        disableControls = true;

        InitializeScores();
        AssignTurns();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAiming)
        {
            EndTurn();
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
                isTurnOver = true;

                //TEMPORARY
                currTurn++;

                if (currTurn >= GameInfo.instance.Players)
                {
                    currTurn = 0;
                }

                isTurnOver = false;
                SetActiveBall();
                Camera.main.GetComponent<ThirdPersonCamera>().Target = activeBall.transform;
                GameObject.Find("Canvas").GetComponent<TurnDisplay>().StartDisplay();
            }
        }
    }
}
