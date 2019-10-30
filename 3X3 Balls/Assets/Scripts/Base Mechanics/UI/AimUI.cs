using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AimUI : MonoBehaviour
{
    public Image ball;
    public Sprite center;
    public Sprite top;
    public Sprite bottom;
    public Sprite left;
    public Sprite right;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetCenterHit()
    {
        ball.sprite = center;
    }

    public void SetTopHit()
    {
        ball.sprite = top;
    }

    public void SetBottomHit()
    {
        ball.sprite = bottom;
    }

    public void SetLeftHit()
    {
        ball.sprite = left;
    }

    public void SetRightHit()
    {
        ball.sprite = right;
    }
}
