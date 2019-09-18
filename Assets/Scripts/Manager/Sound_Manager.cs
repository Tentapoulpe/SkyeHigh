using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound_Manager : MonoBehaviour
{
    public static Sound_Manager Instance {get; private set;}

    private AudioSource a_myCurrentAudio;
    private AudioClip a_myAudioClip;
    public AudioSource m_soundStage;
    public AudioSource m_soundEffect;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    public void PlayMusicLevel(AudioClip my_audioClip)
    {
        if(m_soundStage.isPlaying)
        {
            m_soundStage.Stop();
        }
        m_soundStage.PlayOneShot(my_audioClip);
    }

    public void StopMusicLevel()
    {
        m_soundStage.Stop();
    }

    private void CurrentAudio(AudioClip my_Audio)
    {
        a_myAudioClip = my_Audio;
        PlayOrStopAudio();
    }

    public void PlayOrStopAudio()
    {
        if(a_myCurrentAudio.isPlaying)
        {
            a_myCurrentAudio.Stop();
        }
        else
        {
            a_myCurrentAudio.PlayOneShot(a_myAudioClip);
        }
    }
}
