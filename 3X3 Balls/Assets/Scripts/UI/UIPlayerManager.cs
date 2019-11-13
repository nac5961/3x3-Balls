using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerManager : MonoBehaviour
{
    public GameObject p1Toggle;
    public GameObject p2Toggle;
    public GameObject p3Toggle;
    public GameObject p4Toggle;

    // Start is called before the first frame update
    void Start()
    {
        SetActiveToggle();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Sets the number of players.
    /// Called in the UI.
    /// </summary>
    /// <param name="num">number of players</param>
    public void SetNumPlayers(int num)
    {
        GameInfo.instance.SetPlayers(num);
    }

    /// <summary>
    /// Enables the toggle based on the number of players.
    /// </summary>
    private void SetActiveToggle()
    {
        Toggle toggle;

        switch (GameInfo.instance.Players)
        {
            case 1:
                toggle = p1Toggle.GetComponent<Toggle>();
                break;
            case 2:
                toggle = p2Toggle.GetComponent<Toggle>();
                break;
            case 3:
                toggle = p3Toggle.GetComponent<Toggle>();
                break;
            case 4:
                toggle = p4Toggle.GetComponent<Toggle>();
                break;
            default:
                toggle = p1Toggle.GetComponent<Toggle>();
                break;
        }

        toggle.isOn = true;
    }
}
