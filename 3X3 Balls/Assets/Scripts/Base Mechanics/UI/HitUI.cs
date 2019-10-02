using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitUI : MonoBehaviour
{
    public GameObject maxHitText;

    public float duration;
    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneInfo.instance.GameStart && !SceneInfo.instance.Paused)
        {
            if (maxHitText.activeSelf)
            {
                DisplayMaxHitUI();
            }
        }
    }

    /// <summary>
    /// Configures the UI before it is displayed.
    /// </summary>
    public void SetupMaxHitUI()
    {
        timer = 0.0f;
        maxHitText.SetActive(true);
    }

    /// <summary>
    /// Displays the UI for a set duration.
    /// </summary>
    private void DisplayMaxHitUI()
    {
        if (timer < duration)
        {
            timer += Time.deltaTime;

            if (timer >= duration)
            {
                maxHitText.SetActive(false);
            }
        }
    }
}
