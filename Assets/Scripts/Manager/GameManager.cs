using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public string m_sceneToLoad;

    [Header("Player")]
    public GameObject[] m_character;
    public GameObject[] m_mySpawn;
    public int playerNumber;
    private string[] s_playerNames;
    private string s_fireInput;
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
    }

    private void Start()
    {
        if(b_spawnPlayer)
        StartGame();
    }

    private void Update()
    {
        //for (int i = 1; i < 4; i++)
        //{
        //    if (Mathf.Abs(Input.GetAxis("Horizontal_P" + i)) > 0.2 || Mathf.Abs(Input.GetAxis("Vertical_P" + i)) > 0.2)
        //    {
        //        Debug.Log(Input.GetJoystickNames());
        //    }
        //}

        //s_playerNames = Input.GetJoystickNames();
        //if (s_playerNames[s_playerNames.Length].Length == 19)
        //{
        //    s_fireInput = "F_PS4_P" + playerNumber;
        //}
        //else if (s_playerNames[s_playerNames.Length].Length == 33)
        //{
        //    s_fireInput = "F_XBOX_P" + playerNumber;
        //}
        //else
        //{
        //    s_fireInput = "F_PC_P" + playerNumber;
        //}
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
}
