using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GeneralUI : MonoBehaviour
{
    public TextMeshProUGUI strokeCount;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Sets the stroke count shown in the UI.
    /// </summary>
    public void SetStrokeCount()
    {
        strokeCount.text = SceneInfo.instance.Scores[SceneInfo.instance.GetCurrentPlayer()].ToString();
    }
}
