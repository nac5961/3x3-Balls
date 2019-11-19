using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerManager : MonoBehaviour
{
    public Toggle p1Toggle;
    public Toggle p2Toggle;
    public Toggle p3Toggle;
    public Toggle p4Toggle;

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
                toggle = p1Toggle;
                break;
            case 2:
                toggle = p2Toggle;
                break;
            case 3:
                toggle = p3Toggle;
                break;
            case 4:
                toggle = p4Toggle;
                break;
            default:
                toggle = p1Toggle;
                break;
        }

        toggle.isOn = true;
    }
}
