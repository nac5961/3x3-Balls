using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FadeToNextLevel : MonoBehaviour
{
    public GameObject panel;
    public float fadeSpeed;

    private bool readyToFade;

    public bool ReadyToFade
    {
        set { readyToFade = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        readyToFade = false;
    }

    // Update is called once per frame
    void Update()
    {
        FadeOut();
    }

    public void FadeOut()
    {
        if (SceneInfo.instance.IsTurnOver && SceneInfo.instance.IsRoundOver && readyToFade)
        {
            Color color = panel.GetComponent<Image>().color;
            panel.GetComponent<Image>().color = new Color(color.r, color.g, color.b, Mathf.Clamp(color.a + fadeSpeed * Time.deltaTime, 0.0f, 1.0f));

            if (panel.GetComponent<Image>().color.a >= 1.0f)
            {
                GameInfo.instance.CurrLevel++;

                if (GameInfo.instance.CurrLevel >= GameInfo.instance.Levels.Length)
                {
                    GameInfo.instance.CurrLevel = 0;
                }

                if (GameInfo.instance.IsFirstCourse)
                {
                    GameInfo.instance.IsFirstCourse = false;
                }

                SceneManager.LoadScene("Course " + GameInfo.instance.Levels[GameInfo.instance.CurrLevel]);
            }
        }
    }
}
