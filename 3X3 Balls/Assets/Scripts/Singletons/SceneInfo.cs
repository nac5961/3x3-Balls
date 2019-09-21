using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneInfo : MonoBehaviour
{
    public static SceneInfo instance;

    private BallType turn;
    private GameObject[] balls;

    private bool isAiming = true;
    private bool isTakingShot = false;
    private bool isTurnOver = false;

    private bool isRoundOver = false;
    private bool disableControls = false;

    public BallType Turn
    {
        get { return turn; }
        set { turn = value; }
    }

    public GameObject[] Balls
    {
        set { balls = value; }
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
        AssignFirstTurn();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAiming)
        {
            EndTurn();
        }
    }

    public void AssignFirstTurn()
    {
        float rand = Random.Range(0.0f, 1.0f);

        if (rand <= 0.5f)
        {
            turn = BallType.Solid;
        }
        else
        {
            //turn = BallType.Striped;
            turn = BallType.Solid;
        }
    }

    private void EndTurn()
    {
        for (int i = 0; i < balls.Length; i++)
        {
            //Balls still moving
            if (!balls[i].GetComponent<Rigidbody>().IsSleeping())
            {
                break;
            }

            //All balls stopped
            if (i == balls.Length - 1)
            {
                isTurnOver = true;
            }
        }
    }
}
