using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInfo : MonoBehaviour
{
    public static GameInfo instance;

    private GameObject[] balls;

    private bool gameStart;
    private bool paused;
    private bool gameover;

    private bool firstHit;
    private bool preHit;
    private bool postHit;
    private bool reset;

    public GameObject[] Balls
    {
        get { return balls; }
        set { balls = value; }
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

    public bool GameOver
    {
        get { return gameover; }
        set { gameover = value; }
    }

    public bool FirstHit
    {
        get { return firstHit; }
        set { firstHit = value; }
    }

    public bool PreHit
    {
        get { return preHit; }
        set { preHit = value; }
    }

    public bool PostHit
    {
        get { return postHit; }
        set { postHit = value; }
    }

    public bool Reset
    {
        get { return reset; }
        set { reset = value; }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        gameStart = true;
        paused = false;
        gameover = false;

        firstHit = true;
        preHit = true;
        postHit = false;
        reset = false;
    }

    // Update is called once per frame
    void Update()
    {
        EndTurn();
    }

    private void EndTurn()
    {
        if (postHit)
        {
            for (int i = 0; i < balls.Length; i++)
            {
                //A ball is still moving
                if (!balls[i].GetComponent<Rigidbody>().IsSleeping())
                {
                    break;
                }

                //All balls have stopped moving
                else if (i == balls.Length - 1)
                {
                    reset = true;
                    preHit = true;
                    postHit = false;
                    Debug.Log("All Stopped");
                }
            }
        }
    }
}
