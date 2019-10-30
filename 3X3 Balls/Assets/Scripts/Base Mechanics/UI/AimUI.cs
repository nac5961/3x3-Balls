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

    public Image Ball
    {
        get { return ball; }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SetBall(Sprite sprite)
    {
        ball.sprite = sprite;
    }

    public void SetCenterHit()
    {
        SetBall(center);
    }

    public void SetTopHit()
    {
        SetBall(top);
    }

    public void SetBottomHit()
    {
        SetBall(bottom);
    }

    public void SetLeftHit()
    {
        SetBall(left);
    }

    public void SetRightHit()
    {
        SetBall(right);
    }
}
