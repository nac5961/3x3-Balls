//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class GameInfo : MonoBehaviour
//{
//    public static GameInfo instance;

//    private GameObject[] balls;
//    private GameObject cue;

//    private bool paused;

//    private bool firstHit;
//    private bool isAiming;
//    private bool isTakingShot;
//    private bool isTurnOver;

//    public GameObject[] Balls
//    {
//        get { return balls; }
//        set { balls = value; }
//    }

//    public GameObject Cue
//    {
//        get { return cue; }
//        set { cue = value; }
//    }

//    public bool Paused
//    {
//        get { return paused; }
//        set { paused = value; }
//    }

//    public bool FirstHit
//    {
//        get { return firstHit; }
//        set { firstHit = value; }
//    }

//    public bool IsAiming
//    {
//        get { return isAiming; }
//        set { isAiming = value; }
//    }

//    public bool IsTakingShot
//    {
//        get { return isTakingShot; }
//        set { isTakingShot = value; }
//    }

//    public bool IsTurnOver
//    {
//        get { return isTurnOver; }
//        set { isTurnOver = value; }
//    }

//    private void Awake()
//    {
//        if (instance == null)
//        {
//            instance = this;
//        }
//        else
//        {
//            Destroy(gameObject);
//        }
//    }

//    // Start is called before the first frame update
//    void Start()
//    {
//        paused = false;

//        firstHit = true;
//        isAiming = true;
//        isTakingShot = false;
//        isTurnOver = false;
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        if (!isAiming)
//        {
//            EndTurn();
//        }
//    }

//    private void EndTurn()
//    {
//        for (int i = 0; i < balls.Length; i++)
//        {
//            //A ball is still moving
//            if (!balls[i].GetComponent<Rigidbody>().IsSleeping())
//            {
//                break;
//            }

//            //End Turn - All balls have stopped moving
//            //In UIManager, the next turn will begin
//            else if (i == balls.Length - 1)
//            {
//                isTurnOver = true;
//                Debug.Log("All Stopped");
//            }
//        }
//    }
//}
