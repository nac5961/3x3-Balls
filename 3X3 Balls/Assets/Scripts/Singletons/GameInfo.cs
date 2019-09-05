using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BallType
{
    Solid,
    Striped
}

public class GameInfo : MonoBehaviour
{
    public static GameInfo instance;

    private BallType p1Type;
    private BallType p2Type;

    private int currLevel;
    private int[] levels;
    private List<string> capturedBalls;

    private bool isFirstCourse = true;

    public BallType P1Type
    {
        get { return p1Type; }
    }

    public BallType P2Type
    {
        get { return p2Type; }
    }

    public int CurrLevel
    {
        get { return currLevel; }
        set { currLevel = value; }
    }

    public int[] Levels
    {
        get { return levels; }
    }

    public List<string> CapturedBalls
    {
        get { return capturedBalls; }
        set { capturedBalls = value; }
    }

    public bool IsFirstCourse
    {
        get { return isFirstCourse; }
        set { isFirstCourse = value; }
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
        currLevel = 0;
        levels = new int[] { 1, 2, 3 };
        capturedBalls = new List<string>();

        AssignBallType();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void AssignBallType()
    {
        float rand = Random.Range(0.0f, 1.0f);

        if (rand <= 0.5f)
        {
            p1Type = BallType.Solid;
            p2Type = BallType.Striped;
        }
        else
        {
            p1Type = BallType.Striped;
            p2Type = BallType.Solid;
        }
    }
}
