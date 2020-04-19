using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    
    public const float DEFAULT_MUSIC_VOLUME = .65f;
    public const float DUCK_MUSIC_VOLUME = .3f;

    public static MusicManager Instance { private set; get; }

    public bool PlayMenuMusicOnStart = false;

    public AudioClip MainMenuMusic;
    public AudioClip GameMusic;
    public AudioClip GunSound;


    private AudioSource mMusicSource;
    private AudioSource mGunShotSource;

    private AudioSource mDefaultSource;

    private void Awake() 
    {
        if(Instance != null) Destroy(gameObject);
        Instance = this;
        DontDestroyOnLoad(gameObject);
        mMusicSource = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if(PlayMenuMusicOnStart) PlayMusic(true);
    }

    public void GunEffect(bool startPlaying)
    {
        if(startPlaying) mGunShotSource.Play();
        else mGunShotSource.Pause();
    }

    public void PlayMusic(bool menuMusic)
    {
        if(menuMusic)
        {
            mMusicSource.clip = MainMenuMusic;

        }
        else
        {
            mMusicSource.clip = GameMusic;
        }
        mMusicSource.Play();
    }

    public void DuckMusic(bool duckTheMusic)
    {
        if(duckTheMusic)
        {
            mMusicSource.volume = DUCK_MUSIC_VOLUME;
        }
        else
        {
            mMusicSource.volume = DEFAULT_MUSIC_VOLUME;
        }
    }

    public void StopMusic()
    {
        //mMusicSource.pitch = 0.5f;
        mMusicSource.Stop();
    }
}
