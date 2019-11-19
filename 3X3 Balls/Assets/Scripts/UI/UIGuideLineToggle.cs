using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGuideLineToggle : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SetToggle();   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Sets the appropriate status of the toggle.
    /// </summary>
    private void SetToggle()
    {
        if (GameInfo.instance.GuideLines)
        {
            GetComponent<Toggle>().isOn = true;
            GameInfo.instance.GuideLines = true; //This is needed since the toggle automatically resets on start, which triggers ToggleGuideLines()
        }
        else
        {
            GetComponent<Toggle>().isOn = false;
            GameInfo.instance.GuideLines = false; //This is needed since the toggle automatically resets on start, which triggers ToggleGuideLines()
        }
    }

    /// <summary>
    /// Toggles the guide lines.
    /// </summary>
    public void ToggleGuideLines()
    {
        GameInfo.instance.GuideLines = !GameInfo.instance.GuideLines;
    }
}
