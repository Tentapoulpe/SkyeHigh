using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public string m_sceneToLoad;



    int[] l_ControllerUsed = new int[4];
    int playerConnected = 0;

    [Header("Player")]
    public GameObject[] m_character;
    public GameObject[] m_mySpawn;
    public bool b_spawnPlayer;

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
        string[] names = Input.GetJoystickNames();
        if (((Input.GetButtonDown("F1_PS4_P1") && names[0].Length == 19) || (Input.GetButtonDown("F1_XBOX_P1") && names[0].Length == 33)) && l_ControllerUsed[0] == 0)
        {
            AddPlayer(0);
        }
        else if (((Input.GetButtonDown("F1_PS4_P2") && names[1].Length == 19) || (Input.GetButtonDown("F1_XBOX_P2") && names[1].Length == 33)) && l_ControllerUsed[1] == 0)
        {
            AddPlayer(1);
        }
        else if (((Input.GetButtonDown("F1_PS4_P3") && names[2].Length == 19) || (Input.GetButtonDown("F1_XBOX_P3") && names[2].Length == 33)) && l_ControllerUsed[2] == 0)
        {
            AddPlayer(2);
        }
        else if (((Input.GetButtonDown("F1_PS4_P4") && names[3].Length == 19) || (Input.GetButtonDown("F1_XBOX_P4") && names[3].Length == 33)) && l_ControllerUsed[3] == 0)
        {
            AddPlayer(3);
        }

        if (((Input.GetButtonDown("F2_PS4_P1") && names[0].Length == 19) || (Input.GetButtonDown("F2_XBOX_P1") && names[0].Length == 33)) && l_ControllerUsed[0] != 0)
        {
            RemovePlayer(0);
        }
        else if (((Input.GetButtonDown("F2_PS4_P2") && names[1].Length == 19) || (Input.GetButtonDown("F2_XBOX_P2") && names[1].Length == 33)) && l_ControllerUsed[1] != 0)
        {
            RemovePlayer(1);
        }
        else if (((Input.GetButtonDown("F2_PS4_P3") && names[2].Length == 19) || (Input.GetButtonDown("F2_XBOX_P3") && names[2].Length == 33)) && l_ControllerUsed[2] != 0)
        {
            RemovePlayer(2);
        }
        else if (((Input.GetButtonDown("F2_PS4_P4") && names[3].Length == 19) || (Input.GetButtonDown("F2_XBOX_P4") && names[3].Length == 33)) && l_ControllerUsed[3] != 0)
        {
            RemovePlayer(3);
        }
    }

    public void RestartGame()
    {
        Scene loadedLevel = SceneManager.GetActiveScene();
        SceneManager.LoadScene(loadedLevel.buildIndex);
    }

    public void StartGame()
    {
        for (int i = 0; i < m_mySpawn.Length; i++)
        {
            Instantiate(m_character[0], m_mySpawn[i].transform.position, Quaternion.identity);
        }
    }

    public void Quit()
    {
        Application.Quit();
    }


    public void AddPlayer(int player)
    {
        playerConnected++;
        l_ControllerUsed[player] = playerConnected;
    }

    public void RemovePlayer(int player)
    {
        int playerPlace = l_ControllerUsed[player];
        l_ControllerUsed[player] = 0;
        for (int i = 0; i < l_ControllerUsed.Length; i++)
        {
            if (l_ControllerUsed[i] > playerPlace)
            {
                l_ControllerUsed[i]--;
            }
        }
        playerConnected--;
    }
    
}
