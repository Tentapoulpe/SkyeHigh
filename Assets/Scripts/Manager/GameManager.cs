﻿using System.Collections;
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
        StartGame();
    }

    private void Update()
    {
        for (int i = 1; i < 4; i++)
        {
            if (Mathf.Abs(Input.GetAxis("Horizontal_P" + i)) > 0.2 ||
                Mathf.Abs(Input.GetAxis("Vertical_P" + i)) > 0.2)
            {
                Debug.Log(Input.GetJoystickNames()[i-1] + " is moved");
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
