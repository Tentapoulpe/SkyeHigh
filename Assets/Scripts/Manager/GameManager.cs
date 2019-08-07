﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public string m_sceneToLoad;



    int[] l_Players = new int[4];
    int playerConnected = 0;
    bool b_isInCharacterSelection = false;

    [Header("Player")]
    public GameObject[] m_character;
    private int[] m_PlayersCharacter = new int[4];
    public GameObject[] m_mySpawn;
    public bool b_spawnPlayer;
    float[] axisCD = new float[4];
    bool[] lockedPlayer = new bool[4];

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
        if(b_spawnPlayer)
        StartGame();
    }

    private void Update()
    {
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
            // ADD BUTTON

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
                        UI_Menu_Manager.Instance.CharacterSelectionToStart();
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
                        UI_Menu_Manager.Instance.CharacterSelectionToStart();
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
                        UI_Menu_Manager.Instance.CharacterSelectionToStart();
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
                        UI_Menu_Manager.Instance.CharacterSelectionToStart();
                        b_isInCharacterSelection = false;
                    }
                }
            }

            if (!lockedPlayer[0] && axisCD[0] == 0 && l_Players[0] != 0)
            {
                if (Input.GetAxis("Horizontal_P1") == 1)
                {
                    ChangeCharacter(0, 1);
                    axisCD[0] = 0.5f;
                }
                else if (Input.GetAxis("Horizontal_P1") == -1)
                {
                    ChangeCharacter(0, -1);
                    axisCD[0] = 0.5f;
                }
            }
            if (!lockedPlayer[1] && axisCD[1] == 0 && l_Players[1] != 0)
            {
                if (Input.GetAxis("Horizontal_P2") == 1)
                {
                    ChangeCharacter(1, 1);
                    axisCD[1] = 0.5f;
                }
                else if (Input.GetAxis("Horizontal_P2") == -1)
                {
                    ChangeCharacter(1, -1);
                    axisCD[1] = 0.5f;
                }
            }
            if (!lockedPlayer[2] && axisCD[2] == 0 && l_Players[2] != 0)
            {
                if (Input.GetAxis("Horizontal_P3") == 1)
                {
                    ChangeCharacter(2, 1);
                    axisCD[2] = 0.5f;
                }
                else if (Input.GetAxis("Horizontal_P3") == -1)
                {
                    ChangeCharacter(2, -1);
                    axisCD[2] = 0.5f;
                }
            }
            if (!lockedPlayer[3] && axisCD[3] == 0 && l_Players[3] != 0)
            {
                if (Input.GetAxis("Horizontal_P4") == 1)
                {
                    ChangeCharacter(3, 1);
                    axisCD[3] = 0.5f;
                }
                else if (Input.GetAxis("Horizontal_P4") == -1)
                {
                    ChangeCharacter(3, -1);
                    axisCD[3] = 0.5f;
                }
            }
        }
    }

    public void RestartGame()
    {
        Scene loadedLevel = SceneManager.GetActiveScene();
        SceneManager.LoadScene(loadedLevel.buildIndex);
    }

    public void StartGame()
    {
        for (int i = 0; i < l_Players.Length; i++)
        {
            if (l_Players[i] !=0)
            {
                GameObject player = Instantiate(m_character[m_PlayersCharacter[i]], m_mySpawn[l_Players[i]-1].transform.position, Quaternion.identity);
                player.GetComponent<PlayerController>().SetPlayerNumber(i + 1);
            }
        }
    }

    public void Quit()
    {
        Application.Quit();
    }


    public void AddPlayer(int player)
    {
        playerConnected++;
        l_Players[player] = playerConnected;
        UI_Menu_Manager.Instance.ActivatePlayer(playerConnected, player);
    }

    public void RemovePlayer(int player)
    {
        int playerPlace = l_Players[player];
        UI_Menu_Manager.Instance.DeactivatePlayer(playerPlace);
        l_Players[player] = 0;
        for (int i = 0; i < l_Players.Length; i++)
        {
            if (l_Players[i] > playerPlace)
            {
                UI_Menu_Manager.Instance.DeactivatePlayer(l_Players[i]);
                l_Players[i]--;
                UI_Menu_Manager.Instance.ActivatePlayer(l_Players[i],i);
            }
        }
        playerConnected--;
    }

    public void ChangeCharacter(int player,int changing)
    {
        m_PlayersCharacter[player] += changing;
        if (m_PlayersCharacter[player] < 0)
        {
            m_PlayersCharacter[player] = m_character.Length - 1;
        }
        else if (m_PlayersCharacter[player] >= m_character.Length)
        {
            m_PlayersCharacter[player] = 0;
        }
        UI_Menu_Manager.Instance.UpdatePlayerCharacter(l_Players[player], m_PlayersCharacter[player]);
    }

    public void LockPlayer(int player)
    {
        lockedPlayer[player] = true;
        UI_Menu_Manager.Instance.LockPlayerCharacter(l_Players[player]);
        if (playerConnected >= 2)
        {
            int playerLocked = 0;
            for (int i = 0; i < lockedPlayer.Length; i++)
            {
                if (lockedPlayer[i] == true)
                {
                    playerLocked++;
                }
                if (playerLocked == playerConnected)
                {
                    b_isInCharacterSelection = false;
                    SceneManager.LoadScene(1);
                    UI_Menu_Manager.Instance.GameScreen();
                }
            }
        }
    }

    public void UnlockPlayer(int player)
    {
        lockedPlayer[player] = false;
        UI_Menu_Manager.Instance.UnlockPlayerCharacter(l_Players[player]);
    }

    public void GoToCharacterSelection()
    {
        b_isInCharacterSelection = true;
    }



}
