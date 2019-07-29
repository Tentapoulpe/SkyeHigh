using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Script_Game_Manager : MonoBehaviour
{
    public static Script_Game_Manager Instance { get; private set; }

    public string m_sceneToLoad;

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

    public void RestartGame()
    {
        Scene loadedLevel = SceneManager.GetActiveScene();
        SceneManager.LoadScene(loadedLevel.buildIndex);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(m_sceneToLoad);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
