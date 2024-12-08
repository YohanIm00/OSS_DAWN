using System.Collections;
using System.Collections.Generic;
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
    private int channelIndex;
    
    public enum BGM { MainMenu, Prologue, Dialogue, SortGame, MainGame, GameClear, GameOver }
    public enum SFX
    {
        // Main UI
        // [Header("#Main UI")]
        ButtonClick0,
        // Prologue
        Earthquake, MeowKitten, Lightbulb,
        // Dialogue
        ButtonClick1,
        // SortGame
        CorrectSort, WrongSort,
        // MainGame
        OrderMeow0, OrderMeow1, OrderMeow2,
        // GameOver
        Spark, TurnLightOn
    }

    private void Awake()
    {
        instance = this;
        Init();
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
        bgmPlayer.Play();
    }

    public void StopBgm(BGM bgm)
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
        for (int index = 0; index < sfxPlayers.Length; ++index)
        {
            int loopIndex = (index + channelIndex) % sfxPlayers.Length;

            if(sfxPlayers[loopIndex].isPlaying)
                continue;

            channelIndex = loopIndex;

            sfxPlayers[loopIndex].clip = sfxClips[(int)sfx];
            sfxPlayers[loopIndex].Play();
        }
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
