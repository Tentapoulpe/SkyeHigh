using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;
using System;

public enum SFXToPlay {
    Menu_Cancel,
    Menu_Skye,
    Menu_Dawn,
    Menu_Bolt,
    Menu_King,
    Menu_Valid,
    Menu_Selec,
    Bolt_Hit,
    Bolt_Skill,
    Dash,Last_Dash,
    Hit_Bord,
    Hit_Bord_Dash,
    Hit_Cloud,
    Hit_Dash,
    Hit_Dash_vs_Dash,
    Fall,
    Crash,
    Absorb_Cloud
}

public class AudioManager : MonoBehaviour
{

    public static AudioManager Instance { get; private set; }

    [Serializable]
    public struct SFX
    {
        public string Name;
        [EventRef]
        public string Event;
    }

    public List<SFX> SFXList;
    [Space(10f)]
    [Header("Musics")]
    [EventRef]
    public string MenuMusic;
    [EventRef]
    public string SkyeMusic, DawnMusic, BoltMusic, KingMusic;
    EventInstance musicInstance;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
            Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        musicInstance = RuntimeManager.CreateInstance(MenuMusic);
        musicInstance.start();
    }

    public void ChangeMusic(int musicIdx)
    {
        musicInstance.release();
        switch(musicIdx)
        {
            case 0:
                musicInstance = RuntimeManager.CreateInstance(MenuMusic);
                break;
            case 1:
                musicInstance = RuntimeManager.CreateInstance(SkyeMusic);
                break;
            case 2:
                musicInstance = RuntimeManager.CreateInstance(DawnMusic);
                break;
            case 3:
                musicInstance = RuntimeManager.CreateInstance(BoltMusic);
                break;
            case 4:
                musicInstance = RuntimeManager.CreateInstance(KingMusic);
                break;
        }
    }

    public void PlaySFX(SFXToPlay sfx)
    {
        RuntimeManager.PlayOneShot(SFXList[(int)sfx].Event);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
