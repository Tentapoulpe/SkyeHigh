using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound_Manager : MonoBehaviour
{
    public static Sound_Manager Instance {get; private set;}

    private AudioSource a_myCurrentAudio;
    public AudioSource m_soundMainMenu;

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

    private void CurrentAudio(AudioSource my_Audio)
    {
        a_myCurrentAudio = my_Audio;
    }

    public void PlayOrStopAudio()
    {
        if(a_myCurrentAudio.isPlaying)
        {
            a_myCurrentAudio.Stop();
        }
        else
        {
            a_myCurrentAudio.Play();
        }
    }

    public void PlayMainMenuSound()
    {
        m_soundMainMenu.Play();
        CurrentAudio(m_soundMainMenu);
    }






    public void StopPlayCurrentAudio()
    {
        a_myCurrentAudio.Stop();
    }

    public void PlayCurrentAudio()
    {
        a_myCurrentAudio.Play();
    }
}
