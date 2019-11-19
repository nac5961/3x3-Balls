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

    //Sound Effects file names

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
        PlaySoundEffect(mainMenu);//PlayMainMenuMusic();
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
        int removed = soundEffectInstances.RemoveAll(i => i.isPlaying == false);

        if (removed == 0)
        {
            Debug.Log("none removed");
        }
        else
        {
            Debug.Log("removed " + removed);
        }
    }

    private AudioSource FindAudio(string name)
    { 
        return audioSources.Find(i => i.clip.name == name);
    }

    private void PlaySoundEffect(string name)
    {
        AudioSource soundEffect = FindAudio(name);

        AudioSource soundEffectInstance = gameObject.AddComponent<AudioSource>();
        soundEffectInstance.clip = soundEffect.clip;
        soundEffectInstance.Play();

        soundEffectInstances.Add(soundEffectInstance);
    }

    private void PlayBackgroundMusic(string next, string prev = null)
    {
        AudioSource nextAudio = FindAudio(next);

        if (!string.IsNullOrEmpty(prev))
        {
            AudioSource prevAudio = FindAudio(prev);
            prevAudio.volume = 0.01f;
            prevAudio.Stop();
        }

        nextAudio.volume = 1.0f;
        nextAudio.Play();
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

        int rand = Random.Range(0, 2);
        string audio = rand == 0 ? inGame1 : inGame2;

        if (menu.isPlaying)
        {
            PlayBackgroundMusic(audio, mainMenu);
        }
        else if (game1.isPlaying)
        {
            PlayBackgroundMusic(audio, inGame1);
        }
        else if (game2.isPlaying)
        {
            PlayBackgroundMusic(audio, inGame2);
        }
    }
}
