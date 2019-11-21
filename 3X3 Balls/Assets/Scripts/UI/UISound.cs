using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISound : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Plays the sound for when a button is hovered over.
    /// </summary>
    public void PlayHoverSound()
    {
        AudioInfo.instance.PlayUIHoverSoundEffect();
    }

    /// <summary>
    /// Plays the sound for when a button is clicked.
    /// </summary>
    public void PlayClickSound()
    {
        AudioInfo.instance.PlayUIClickSoundEffect();
    }
}
