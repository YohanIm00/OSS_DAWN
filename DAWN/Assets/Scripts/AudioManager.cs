using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("#BGM")]
    public AudioClip[] bgmClips;
    public float bgmVolume;
    private AudioSource bgmPlayer;

    [Header("#SFX")]
    public AudioClip[] sfxClips;
    public float sfxVolume;
    public int channels;
    private AudioSource[] sfxPlayers;
    [SerializeField] private int channelIndex;
    
    public enum BGM { MainMenu, Prologue, SortGame, Dialogue, MainGame, GameClear, GameOver }
    public enum SFX
    {
        // Main UI
        ButtonSelect, ButtonClick,
        // Prologue
        Earthquake, MeowKitten, GoodIdea, WandAppear,
        // Dialogue : Add later...
        // SortGame
        SortStart, SortFinish, CorrectSort, WrongSort,
        // MainGame
        MainStart, MainFinish, OrderMeow0, OrderMeow1, OrderMeow2,  OrderMeow3, OrderMeow4, Baking3s, Baking5s, Serving, BalloonPop, 
        // GameOver
        Spark, Spotlight
    }

    private void Awake()
    {
        instance = this;
        Init();
    }

    private void Start()
    {
        StopBgm();

        if (SceneManager.GetActiveScene().name == "MainMenu")
            PlayBgm(BGM.MainMenu);
        else if (SceneManager.GetActiveScene().name == "Prologue")
            PlayBgm(BGM.Prologue);
        else if (SceneManager.GetActiveScene().name == "SortingGame")
            PlayBgm(BGM.SortGame);
        else if (SceneManager.GetActiveScene().name == "Dialogue")
            PlayBgm(BGM.Dialogue);
        else if (SceneManager.GetActiveScene().name == "MainGame")
            PlayBgm(BGM.MainGame);
        else if (SceneManager.GetActiveScene().name == "GameClear")
            PlayBgm(BGM.GameClear);
        else if (SceneManager.GetActiveScene().name == "GameOver")
            PlayBgm(BGM.GameOver);
    }

    private void Init()
    {
        // Initialize bgmPlayer
        GameObject bgmObject = new GameObject("BGM Player");
        bgmObject.transform.parent = transform;
        bgmPlayer = new AudioSource();

        bgmPlayer = bgmObject.AddComponent<AudioSource>();
        bgmPlayer.playOnAwake = false;
        bgmPlayer.loop = true;
        bgmPlayer.volume = bgmVolume;

        // Initialize sfxPlayer
        GameObject sfxObject = new GameObject("SFX Player");
        sfxObject.transform.parent = transform;
        sfxPlayers = new AudioSource[channels];

        for (int index = 0; index < sfxPlayers.Length; ++index)
        {
            sfxPlayers[index] = sfxObject.AddComponent<AudioSource>();
            sfxPlayers[index].playOnAwake = false;
            sfxPlayers[index].volume = sfxVolume;
        }
    }

    public void PlayBgm(BGM bgm)
    {
        bgmPlayer.clip = bgmClips[(int)bgm];
        if (bgm == BGM.GameClear || bgm == BGM.GameOver)
            bgmPlayer.loop = false;
        bgmPlayer.Play();
    }

    public void StopBgm()
    {
        bgmPlayer.Stop();
    }

    public void VolumeController(float volume)
    {
        bgmVolume = volume;
        bgmPlayer.volume = bgmVolume;
    }

    public void PlaySfx(SFX sfx)
    {
        if (sfx == SFX.Baking3s || sfx == SFX.Baking5s)
            sfxVolume = 0.1f;
        
        for (int index = 0; index < sfxPlayers.Length; ++index)
        {
            int loopIndex = (index + channelIndex) % sfxPlayers.Length;

            if(sfxPlayers[loopIndex].isPlaying)
                continue;

            channelIndex = loopIndex;

            sfxPlayers[loopIndex].clip = sfxClips[(int)sfx];
            sfxPlayers[loopIndex].Play();
        }

        sfxVolume = 0.3f;
    }

    public void StopSfx(SFX sfx)
    {
        for (int index = 0; index < sfxPlayers.Length; ++index)
        {
            int loopIndex = (index + channelIndex) % sfxPlayers.Length;

            if (sfxPlayers[loopIndex].isPlaying && sfxPlayers[loopIndex].clip == sfxClips[(int)sfx])
            {
                sfxPlayers[loopIndex].Stop();
                break;
            }
        }
    }

    public bool isPlaying(SFX sfx)
    {
        for (int index = 0; index < sfxPlayers.Length; ++index)
        {
            int loopIndex = (index + channelIndex) % sfxPlayers.Length;

            if (sfxPlayers[loopIndex].isPlaying && sfxPlayers[loopIndex].clip == sfxClips[(int)sfx])
                return false;
        }
        return true;
    }
}
