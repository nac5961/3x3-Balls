using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AudioInfo : MonoBehaviour
{
    public static AudioInfo instance;

    public AudioClip[] sounds;
    private List<AudioSource> audioSources = new List<AudioSource>();
    private List<AudioSource> soundEffectInstances = new List<AudioSource>();

    private float waitTime = 2.0f;
    private float timer = 0.0f;

    //Background Music file names
    private string mainMenu = "BG_MainMenu";
    private string inGame1 = "BG_InGame1";
    private string inGame2 = "BG_InGame2";

    //Sound Effect file names (Game)
    private string score = "SFX_Score";
    private string cueHit = "SFX_CueHit";
    private string fade = "SFX_Fade";
    private string ballHit = "SFX_BallHit";
    private string wallHit = "SFX_WallHit";
    private string bumperHit = "SFX_BumperHit";

    //Sound Effect files names (UI)
    private string uiHover = "SFX_UIHover";
    private string uiClick = "SFX_UIClick";

    public string BallHit
    {
        get { return ballHit; }
    }
    public string WallHit
    {
        get { return wallHit; }
    }
    public string BumperHit
    {
        get { return bumperHit; }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        SetupSound();
    }
    // Start is called before the first frame update
    void Start()
    {
        PlayMainMenuMusic();
    }

    // Update is called once per frame
    void Update()
    {
        if (timer < waitTime)
        {
            timer += Time.deltaTime;
        }
        else
        {
            timer = 0.0f;
            RemoveSoundEffectInstances();
        }
    }

    private void SetupSound()
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            AudioSource audio = gameObject.AddComponent<AudioSource>();

            audio.clip = sounds[i];
            audio.playOnAwake = false;
            audio.loop = audio.clip.name.Contains("BG") ? true : false;

            audioSources.Add(audio);
        }
    }

    private void RemoveSoundEffectInstances()
    {
        List<AudioSource> finishedSoundEffects = soundEffectInstances.FindAll(i => i.isPlaying == false);

        for (int i = finishedSoundEffects.Count - 1; i >= 0; i--)
        {
            AudioSource soundEffect = finishedSoundEffects[i];

            soundEffectInstances.Remove(soundEffect);
            Destroy(soundEffect);
        }
    }

    private AudioSource FindAudio(string name)
    { 
        return audioSources.Find(i => i.clip.name == name);
    }

    private void PlaySoundEffect(string name, float volume = 1.0f)
    {
        AudioSource soundEffect = FindAudio(name);
        soundEffect.volume = volume;
        soundEffect.Play();
    }

    private void PlayInstancedSoundEffect(string name, float volume = 1.0f)
    {
        AudioSource soundEffect = FindAudio(name);

        AudioSource soundEffectInstance = gameObject.AddComponent<AudioSource>();
        soundEffectInstance.clip = soundEffect.clip;
        soundEffectInstance.volume = volume;
        soundEffectInstance.Play();

        soundEffectInstances.Add(soundEffectInstance);
    }

    /// <summary>
    /// Fades out the specified object's audio by decreasing volume over time
    /// </summary>
    /// <param name="src"> The AudioSource being manipulated </param>
    /// <returns>Coroutine to decrease sources volume</returns>
    private IEnumerator FadeOut(AudioSource src)
    {
        if (src.volume >= 0.9f)
        {
            src.volume = 1.0f;
        }
        //decrease volume over time
        for (float x = src.volume; x >= 0; x -= 0.8f)
        {
            src.volume = x;
            yield return new WaitForSeconds(.1f);
        }
        src.Stop();
    }

    /// <summary>
    /// Fades in the specified object's audio by increasing volume over time
    /// </summary>
    /// <param name="src"> The AudioSource being manipulated </param>
    /// <returns> Coroutine to increase sources volume </returns>
    private IEnumerator FadeIn(AudioSource src)
    {
        if (!src.isPlaying)
        {
            src.volume = 0;
            src.Play();
        }

        //increase volume over time
        for (float x = src.volume; x <= 1f; x += 0.8f)
        {
            src.volume = x;
            yield return new WaitForSeconds(.1f);
        }
    }

    private void PlayBackgroundMusic(string next, string prev = null)
    {
        AudioSource nextAudio = FindAudio(next);

        //the holy grail of code in this one line. Stops all current fading before switching tracks
        StopAllCoroutines();

        //Fade out prev audio
        if (!string.IsNullOrEmpty(prev))
        {
            AudioSource prevAudio = FindAudio(prev);
            StartCoroutine(FadeOut(prevAudio));
        }

        //Fade in next audio
        StartCoroutine(FadeIn(nextAudio));
    }

    public void PlayMainMenuMusic()
    {
        AudioSource game1 = FindAudio(inGame1);
        AudioSource game2 = FindAudio(inGame2);

        if (game1.isPlaying)
        {
            PlayBackgroundMusic(mainMenu, inGame1);
        }
        else if (game2.isPlaying)
        {
            PlayBackgroundMusic(mainMenu, inGame2);
        }
        else
        {
            PlayBackgroundMusic(mainMenu);
        }
    }

    public void PlayInGameMusic()
    {
        AudioSource menu = FindAudio(mainMenu);
        AudioSource game1 = FindAudio(inGame1);
        AudioSource game2 = FindAudio(inGame2);

        //Play InGame1 on the first level (first level is played from main menu)
        if (GameInfo.instance.Level == 1)
        {
            PlayBackgroundMusic(inGame1, mainMenu);
        }

        //Play InGame2 after half of the levels are finished
        else if (GameInfo.instance.Level == GameInfo.instance.TotalLevels / 2)
        {
            PlayBackgroundMusic(inGame2, inGame1);
        }
    }

    #region Game SFX Calls
    public void PlayScoreSoundEffect()
    {
        PlayInstancedSoundEffect(score);
    }

    public void PlayCueHitSoundEffect(float volume)
    {
        PlaySoundEffect(cueHit, volume);
    }

    public void PlayFadeSoundEffect()
    {
        PlaySoundEffect(fade);
    }
    #endregion

    #region UI SFX Calls
    public void PlayUIHoverSoundEffect()
    {
        PlaySoundEffect(uiHover);
    }

    public void PlayUIClickSoundEffect()
    {
        PlaySoundEffect(uiClick);
    }
    #endregion
}
