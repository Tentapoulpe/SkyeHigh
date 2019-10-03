using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public bool m_debugMode;

    [Space(20f)]
    private Camera m_Camera;
    bool winState = false;
    Vector3 v_winerPos = new Vector3();

    int[] l_Players = new int[4];
    int playerConnected = 0;
    int playerAlive = 0;
    bool b_isInCharacterSelection = false;
    bool b_isInMapSelection = false;
    bool b_canChangeMap = true;
    List<PlayerController> l_playersPlaying = new List<PlayerController>();

    [Header("Player")]
    public GameObject[] m_character;
    private int[] m_PlayersCharacter = new int[4];
    public GameObject[] m_mySpawn;
    public bool b_spawnPlayer;
    float[] axisCD = new float[4];
    bool[] lockedPlayer = new bool[4];

    [Header("Cloud")]
    public List<GameObject> m_prefabCloudParent;
    private List<GameObject> g_cloudParentIdx = new List<GameObject>();
    public List<GameObject> m_spawnPointCloud = new List<GameObject>();
    private bool b_spawnIdx;
    private int i_numberOfCloud;
    public int i_numberOfCloudMax;

    private int roundState = 1;
    public int m_MaxRound;
    private List<int> Scores = new List<int>();
    public List<GameObject> m_Levels;
    private int i_mapIdx = 0;
    private bool b_isPause = false;

    public AudioClip m_mainMenuMusic;

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

    private void Start()
    {
        

    }

    private void Update()
    {
        if (winState)
        {
            m_Camera.orthographicSize -= Time.deltaTime * 40;
            m_Camera.transform.position = Vector3.MoveTowards(m_Camera.transform.position, v_winerPos, Time.deltaTime * 80 );
            if (m_Camera.orthographicSize <= 4f)
            {
                m_Camera.orthographicSize = 4f;
                if (roundState < m_MaxRound)
                {
                    if (m_Camera.transform.position == v_winerPos)
                    {
                        
                        if (m_debugMode)
                        {
                            UIManager.Instance.DisplayEndRound(0, Scores);
                        }
                        else if (v_winerPos != Vector3.zero)
                        {
                            Scores[l_playersPlaying[0].playerNumber - 1]++;
                            UIManager.Instance.DisplayEndRound(l_playersPlaying[0].playerNumber, Scores);
                        }
                        else
                            UIManager.Instance.DisplayEndRound(0, Scores);

                        Invoke("RestartRound", 2f);
                        winState = false;
                        CameraManager.Instance.canMove = false;
                    }
                }
                else
                {
                    if (m_Camera.transform.position == v_winerPos)
                    {
                        if (m_debugMode)
                        {
                            UIManager.Instance.DisplayEndGame();
                        }
                        else if (v_winerPos != Vector3.zero)
                        {
                            Scores[l_playersPlaying[0].playerNumber - 1]++;
                            List<int> winnerNumber = new List<int>();
                            int bestScore = 0;
                            for (int i = 0; i < Scores.Count; i++)
                            {
                                    if (bestScore < Scores[i])
                                    {
                                        bestScore = Scores[i];
                                        winnerNumber.Clear();
                                        winnerNumber.Add(i+1);
                                    }
                                    else if (bestScore == Scores[i])
                                    {
                                        winnerNumber.Add(i + 1);
                                    }
                            }
                            UIManager.Instance.DisplayEndGame(winnerNumber, Scores);
                        }
                        else
                        {
                            List<int> winnerNumber = new List<int>();
                            int bestScore = 0;
                            for (int i = 0; i < Scores.Count; i++)
                            {
                                if (bestScore < Scores[i])
                                {
                                    bestScore = Scores[i];
                                    winnerNumber.Clear();
                                    winnerNumber.Add(i + 1);
                                }
                                else if (bestScore == Scores[i])
                                {
                                    winnerNumber.Add(i + 1);
                                }
                            }
                            UIManager.Instance.DisplayEndGame(winnerNumber, Scores);

                        }

                        winState = false;
                        CameraManager.Instance.canMove = false;
                    }
                }
            }
        }

        for (int i = 0; i < axisCD.Length; i++)
        {
            if (axisCD[i] > 0)
            {
                axisCD[i] -= Time.deltaTime;
                if (axisCD[i] < 0)
                {
                    axisCD[i] = 0;
                }
            }
        }

        if (b_isInCharacterSelection)
        {
            string[] names = Input.GetJoystickNames();
            if (((Input.GetButtonDown("F1_PS4_P1") && names[0].Length == 19) || (Input.GetButtonDown("F1_XBOX_P1") && names[0].Length == 33)) && l_Players[0] == 0)
            {
                AddPlayer(0);
            }
            else if (((Input.GetButtonDown("F1_PS4_P1") && names[0].Length == 19) || (Input.GetButtonDown("F1_XBOX_P1") && names[0].Length == 33)) && l_Players[0] != 0)
            {
                LockPlayer(0);
            }

            if (((Input.GetButtonDown("F1_PS4_P2") && names[1].Length == 19) || (Input.GetButtonDown("F1_XBOX_P2") && names[1].Length == 33)) && l_Players[1] == 0)
            {
                AddPlayer(1);
            }
            else if (((Input.GetButtonDown("F1_PS4_P2") && names[1].Length == 19) || (Input.GetButtonDown("F1_XBOX_P2") && names[1].Length == 33)) && l_Players[1] != 0)
            {
                LockPlayer(1);
            }

            if (((Input.GetButtonDown("F1_PS4_P3") && names[2].Length == 19) || (Input.GetButtonDown("F1_XBOX_P3") && names[2].Length == 33)) && l_Players[2] == 0)
            {
                AddPlayer(2);
            }
            else if (((Input.GetButtonDown("F1_PS4_P3") && names[2].Length == 19) || (Input.GetButtonDown("F1_XBOX_P3") && names[2].Length == 33)) && l_Players[2] != 0)
            {
                LockPlayer(2);
            }

            if (((Input.GetButtonDown("F1_PS4_P4") && names[3].Length == 19) || (Input.GetButtonDown("F1_XBOX_P4") && names[3].Length == 33)) && l_Players[3] == 0)
            {
                AddPlayer(3);
            }
            else if (((Input.GetButtonDown("F1_PS4_P4") && names[3].Length == 19) || (Input.GetButtonDown("F1_XBOX_P4") && names[3].Length == 33)) && l_Players[3] != 0)
            {
                LockPlayer(3);
            }

            // BACK BUTTON

            if (((Input.GetButtonDown("F2_PS4_P1") && names[0].Length == 19) || (Input.GetButtonDown("F2_XBOX_P1") && names[0].Length == 33)))
            {
                if (l_Players[0] != 0)
                {
                    if (!lockedPlayer[0])
                    {
                        RemovePlayer(0);
                    }
                    else
                        UnlockPlayer(0);
                }
                else
                {
                    if (playerConnected == 0)
                    {
                        AudioManager.Instance.PlaySFX(SFXToPlay.Menu_Cancel);
                        UIManager.Instance.CharacterSelectionToStart();
                        b_isInCharacterSelection = false;
                    }
                }
            }


            if (((Input.GetButtonDown("F2_PS4_P2") && names[1].Length == 19) || (Input.GetButtonDown("F2_XBOX_P2") && names[1].Length == 33)))
            {
                if (l_Players[1] != 0)
                {
                    if (!lockedPlayer[1])
                    {
                        RemovePlayer(1);
                    }
                    else
                        UnlockPlayer(1);
                }
                else
                {
                    if (playerConnected == 0)
                    {
                        AudioManager.Instance.PlaySFX(SFXToPlay.Menu_Cancel);
                        UIManager.Instance.CharacterSelectionToStart();
                        b_isInCharacterSelection = false;
                    }
                }
            }


            if (((Input.GetButtonDown("F2_PS4_P3") && names[2].Length == 19) || (Input.GetButtonDown("F2_XBOX_P3") && names[2].Length == 33)))
            {
                if (l_Players[2] != 0)
                {
                    if (!lockedPlayer[2])
                    {
                        RemovePlayer(2);
                    }
                    else
                        UnlockPlayer(2);
                }
                else
                {
                    if (playerConnected == 0)
                    {
                        AudioManager.Instance.PlaySFX(SFXToPlay.Menu_Cancel);
                        UIManager.Instance.CharacterSelectionToStart();
                        b_isInCharacterSelection = false;
                    }
                }
            }



            if (((Input.GetButtonDown("F2_PS4_P4") && names[3].Length == 19) || (Input.GetButtonDown("F2_XBOX_P4") && names[3].Length == 33)))
            {
                if (l_Players[3] != 0)
                {
                    if (!lockedPlayer[3])
                    {
                        RemovePlayer(3);
                    }
                    else
                        UnlockPlayer(3);
                }
                else
                {
                    if (playerConnected == 0)
                    {
                        AudioManager.Instance.PlaySFX(SFXToPlay.Menu_Cancel);
                        UIManager.Instance.CharacterSelectionToStart();
                        b_isInCharacterSelection = false;
                    }
                }
            }


            if (!lockedPlayer[0] && axisCD[0] == 0 && l_Players[0] != 0)
            {
                if (Input.GetAxis("Horizontal_P1") == 1)
                {
                    ChangeCharacter(0, 1);
                    axisCD[0] = 0.2f;
                }
                else if (Input.GetAxis("Horizontal_P1") == -1)
                {
                    ChangeCharacter(0, -1);
                    axisCD[0] = 0.2f;
                }
            }
            if (!lockedPlayer[1] && axisCD[1] == 0 && l_Players[1] != 0)
            {
                if (Input.GetAxis("Horizontal_P2") == 1)
                {
                    ChangeCharacter(1, 1);
                    axisCD[1] = 0.2f;
                }
                else if (Input.GetAxis("Horizontal_P2") == -1)
                {
                    ChangeCharacter(1, -1);
                    axisCD[1] = 0.2f;
                }
            }
            if (!lockedPlayer[2] && axisCD[2] == 0 && l_Players[2] != 0)
            {
                if (Input.GetAxis("Horizontal_P3") == 1)
                {
                    ChangeCharacter(2, 1);
                    axisCD[2] = 0.2f;
                }
                else if (Input.GetAxis("Horizontal_P3") == -1)
                {
                    ChangeCharacter(2, -1);
                    axisCD[2] = 0.2f;
                }
            }
            if (!lockedPlayer[3] && axisCD[3] == 0 && l_Players[3] != 0)
            {
                if (Input.GetAxis("Horizontal_P4") == 1)
                {
                    ChangeCharacter(3, 1);
                    axisCD[3] = 0.2f;
                }
                else if (Input.GetAxis("Horizontal_P4") == -1)
                {
                    ChangeCharacter(3, -1);
                    axisCD[3] = 0.2f;
                }
            }
        }
        else if (b_isInMapSelection)
        {
            FocusMap();
            string[] names = Input.GetJoystickNames();
            float f_controllerSensibility = Input.GetAxis("Vertical_P1");
            if (f_controllerSensibility == 0)
            {
                b_canChangeMap = true;
            }

            if (b_canChangeMap && (f_controllerSensibility == 1 || f_controllerSensibility == -1))
            {
                AudioManager.Instance.PlaySFX(SFXToPlay.Menu_Selec);
                Debug.Log("CHANGE");
                b_canChangeMap = false;
                ChangeMap(f_controllerSensibility);
            }

            if (Input.GetButtonDown("F1_PS4_P1") || Input.GetButtonDown("F1_XBOX_P1"))
            {
                AudioManager.Instance.PlaySFX(SFXToPlay.Menu_Valid);
                LockMap();
            }
        }
        else if (Scores.Count > 0 && winState == false && CameraManager.Instance.canMove)
        {
            if (Input.GetButtonDown("START_PS4") || Input.GetButtonDown("START_XBOX"))
            {
                if (b_isPause)
                {
                    AudioManager.Instance.PlaySFX(SFXToPlay.Menu_Selec);
                    UnPauseGame();
                }
                else
                {
                    AudioManager.Instance.PlaySFX(SFXToPlay.Menu_Selec);
                    PauseGame();
                }
            }
        }
    }

    public void ChangeMap(float f_direction)
    {
        FocusMap();
        m_Levels[i_mapIdx].GetComponent<Script_Level_Manager>().StopAnimation();
        if (!b_canChangeMap)
        {
            if (f_direction >= 0 && i_mapIdx < m_Levels.Count - 1)
            {
                Debug.Log("Haut");
                i_mapIdx++;
                FocusMap();
            }
            else if (f_direction < 0 && i_mapIdx > 0)
            {
                Debug.Log("BAS");
                i_mapIdx--;
                FocusMap();
            }
        }
    }

    public void LockMap()
    {
        b_isInMapSelection = false;
        StartCoroutine(ChangeScene(m_Levels[i_mapIdx].GetComponent<Script_Level_Manager>().m_myLevel.m_levelName));
        Sound_Manager.Instance.PlayMusicLevel(m_Levels[i_mapIdx].GetComponent<Script_Level_Manager>().m_myLevel.m_levelmusic);
        UIManager.Instance.GameScreen();
    }

    public void FocusMap()
    {
        m_Camera = Camera.main;
        m_Levels[i_mapIdx].GetComponent<Script_Level_Manager>().PlayAnimation();
        m_Camera.transform.position = Vector3.MoveTowards(m_Camera.transform.position, m_Levels[i_mapIdx].transform.position, Time.deltaTime * 80);
    }

    public void SpawnCloudParent()
    {
        int i_numerCloudParentToSpawn = UnityEngine.Random.Range(0, m_prefabCloudParent.Count);
        if(g_cloudParentIdx.Count < i_numberOfCloudMax)
        {
            int i_mySpawnIdx = Convert.ToInt32(b_spawnIdx);
            if(i_mySpawnIdx == 0)
            {
                GameObject my_cloud = Instantiate(m_prefabCloudParent[i_numerCloudParentToSpawn], m_spawnPointCloud[i_mySpawnIdx].transform);
                g_cloudParentIdx.Add(my_cloud);
                b_spawnIdx = true;
            }
            else
            {
                GameObject my_cloud = Instantiate(m_prefabCloudParent[i_numerCloudParentToSpawn], m_spawnPointCloud[i_mySpawnIdx].transform);
                g_cloudParentIdx.Add(my_cloud);
                b_spawnIdx = false;
            }
        }
    }

    public void UnSpawnCloudParent(GameObject g_myGameobject)
    {
        if(g_cloudParentIdx.Count > 0)
        {
            g_cloudParentIdx.Remove(g_myGameobject);
            SpawnCloudParent();
        }
    }

    public void RestartGame()
    {
        winState = false;
        Scene loadedLevel = SceneManager.GetActiveScene();
        SceneManager.LoadScene(loadedLevel.buildIndex);
    }

    public void RestartNewGame()
    {
        winState = false;
        Scene loadedLevel = SceneManager.GetActiveScene();
        l_playersPlaying.Clear();
        g_cloudParentIdx.Clear();
        m_spawnPointCloud.Clear();
        roundState = 1;
        Scores.Clear();
        SceneManager.LoadScene(loadedLevel.buildIndex);
    }

    public void StartGame()
    {
        m_Camera = Camera.main;
        l_playersPlaying.Clear();
        for (int i = 0; i < l_Players.Length; i++)
        {
            if (l_Players[i] !=0)
            {
                GameObject player = Instantiate(m_character[m_PlayersCharacter[i]], m_mySpawn[l_Players[i]-1].transform.position, Quaternion.identity);
                player.GetComponent<PlayerController>().SetPlayerNumber(i + 1);
                l_playersPlaying.Add(player.GetComponent<PlayerController>());
                CameraManager.Instance.m_Targets.Add(player.transform);
                if (roundState == 1)
                {
                    Scores.Add(0);
                }
            }
        }
        playerAlive = playerConnected;
        for (int i = 0; i < i_numberOfCloudMax; i++)
        {
            SpawnCloudParent();
        }
    }

    public void RestartRound()
    {
        UIManager.Instance.RestartRound();
        l_playersPlaying.Clear();
        g_cloudParentIdx.Clear();
        m_spawnPointCloud.Clear();
        roundState++;
        RestartGame();
    }

    public void Quit()
    {
        Application.Quit();
    }


    public void AddPlayer(int player)
    {
        AudioManager.Instance.PlaySFX(SFXToPlay.Menu_Selec);
        playerConnected++;
        l_Players[player] = playerConnected;
        UIManager.Instance.ActivatePlayer(playerConnected, player);
    }

    public void RemovePlayer(int player)
    {
        AudioManager.Instance.PlaySFX(SFXToPlay.Menu_Cancel);
        int playerPlace = l_Players[player];
        UIManager.Instance.DeactivatePlayer(playerPlace);
        l_Players[player] = 0;
        for (int i = 0; i < l_Players.Length; i++)
        {
            if (l_Players[i] > playerPlace)
            {
                UIManager.Instance.DeactivatePlayer(l_Players[i]);
                l_Players[i]--;
                UIManager.Instance.ActivatePlayer(l_Players[i],i);
            }
        }
        playerConnected--;
    }

    public void ChangeCharacter(int player,int changing)
    {
        AudioManager.Instance.PlaySFX(SFXToPlay.Menu_Selec);
        m_PlayersCharacter[player] += changing;
        if (m_PlayersCharacter[player] < 0)
        {
            m_PlayersCharacter[player] = m_character.Length - 1;
        }
        else if (m_PlayersCharacter[player] >= m_character.Length)
        {
            m_PlayersCharacter[player] = 0;
        }
        UIManager.Instance.UpdatePlayerCharacter(l_Players[player], m_PlayersCharacter[player]);
    }

    public void LockPlayer(int player)
    {
        switch (m_PlayersCharacter[player])
        {
            case 0:
                AudioManager.Instance.PlaySFX(SFXToPlay.Menu_Skye);
                break;
            case 1:
                AudioManager.Instance.PlaySFX(SFXToPlay.Menu_Dawn);
                break;
            case 2:
                AudioManager.Instance.PlaySFX(SFXToPlay.Menu_King);
                break;
            default:
                break;
        }
        lockedPlayer[player] = true;
        UIManager.Instance.LockPlayerCharacter(l_Players[player]);
        if ((playerConnected >= 2 && !m_debugMode) || (playerConnected >= 1 && m_debugMode))
        {
            int playerLocked = 0;
            for (int i = 0; i < lockedPlayer.Length; i++)
            {
                if (lockedPlayer[i] == true)
                {
                    playerLocked++;
                }
            }
            if (playerLocked == playerConnected)
            {
                b_isInCharacterSelection = false;
                UIManager.Instance.ShowMapMenu();
                b_isInMapSelection = true;
            }
        }
    }

    public void UnlockPlayer(int player)
    {
        AudioManager.Instance.PlaySFX(SFXToPlay.Menu_Cancel);
        lockedPlayer[player] = false;
        UIManager.Instance.UnlockPlayerCharacter(l_Players[player]);
    }

    public void GoToCharacterSelection()
    {
        b_isInCharacterSelection = true;
    }

    public void PlayerDied(int player)
    {
        playerAlive--;
        if (m_debugMode)
        {
            WinScreen(Vector3.zero);
            return;
        }
        CameraManager.Instance.m_Targets.Remove(l_playersPlaying[player - 1].transform);
        l_playersPlaying.Remove(l_playersPlaying[player-1]);
        if (playerAlive == 1)
        {
            l_playersPlaying[0].LockControlsEG();
            WinScreen(l_playersPlaying[0].transform.position);
        }
        if (playerAlive <= 0)
        {
            WinScreen(Vector3.zero);
        }
    }

    public void ReturnMenu()
    {
        winState = false;
        if (Time.timeScale != 1)
            Time.timeScale = 1f;
        StartCoroutine(ChangeScene(0));
        Sound_Manager.Instance.PlayMusicLevel(m_mainMenuMusic);
        Destroy(UIManager.Instance.gameObject);
        Destroy(gameObject);
    }

    public void WinScreen(Vector3 WinPos)
    {
        v_winerPos = WinPos;
        winState = true;
    }

    IEnumerator ChangeScene(int scene)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene);
        while (!asyncLoad.isDone)
        {
            UIManager.Instance.UpdateLoadingScreen(asyncLoad.progress);
            yield return null;
        }
        UIManager.Instance.StopLoading();
    }

    IEnumerator ChangeScene(string scene)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene);
        while (!asyncLoad.isDone)
        {
            UIManager.Instance.UpdateLoadingScreen(asyncLoad.progress);
            yield return null;
        }
        UIManager.Instance.StopLoading();
    }

    public void PauseGame()
    {
        foreach(PlayerController player in l_playersPlaying)
        {
            player.LockControls();
        }
        b_isPause = true;
        Time.timeScale = 0;
        UIManager.Instance.DisplayPause();
    }

    public void UnPauseGame()
    {
        Time.timeScale = 1f;
        Invoke("RestartAfterPause", 0.1f);
    }

    public void RestartAfterPause()
    {
        foreach (PlayerController player in l_playersPlaying)
        {
            player.UnlockControls();
        }
        b_isPause = false;
        UIManager.Instance.HidePause();
    }
}
