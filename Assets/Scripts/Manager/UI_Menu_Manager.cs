using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UI_Menu_Manager : MonoBehaviour
{
    public static UI_Menu_Manager Instance { get; private set; }

    public EventSystemTool m_EventSystem;
    public GameObject m_MainMenu;
    public GameObject m_CharacterSelection;
    public GameObject m_EndGame;
    public TextMeshProUGUI m_EndGameText;
    public List<GameObject> m_Players;
    public List<GameObject> m_PlayersDisplay;
    public List<Sprite> m_Characters;

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

    public void StartToCharacterSelection()
    {
        m_MainMenu.SetActive(false);
        m_CharacterSelection.SetActive(true);
        Invoke("EnterCharacterSelection", 0.1f);
    }

    public void CharacterSelectionToStart()
    {
        m_CharacterSelection.SetActive(false);
        m_EventSystem.SetStartPriority();
        m_MainMenu.SetActive(true);
    }

    public void QuitGame()
    {
        GameManager.Instance.Quit();
    }

    public void ActivatePlayer(int player,int playername)
    {
        m_PlayersDisplay[player-1].SetActive(false);
        m_Players[player - 1].GetComponent<SpriteRenderer>().sprite = m_Characters[0];
        m_Players[player-1].SetActive(true);
        m_Players[player-1].transform.GetChild(0).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = "P" + (playername + 1);
    }


    public void DeactivatePlayer(int player)
    {
        m_Players[player-1].SetActive(false);
        m_PlayersDisplay[player-1].SetActive(true);
    }

    public bool IsCharacterSelect()
    {
        return m_CharacterSelection.activeSelf;
    }

    public void UpdatePlayerCharacter(int player, int idx)
    {
        m_Players[player - 1].GetComponent<SpriteRenderer>().sprite = m_Characters[idx];
    }

    public void LockPlayerCharacter(int player)
    {
        m_Players[player - 1].GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0.5f);
    }

    public void UnlockPlayerCharacter(int player)
    {
        m_Players[player - 1].GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f);
    }

    public void EnterCharacterSelection()
    {
        GameManager.Instance.GoToCharacterSelection();
    }

    public void GameScreen()
    {
        m_CharacterSelection.SetActive(false);
    }

    public void DisplayEndGame(int player)
    {
        m_EndGameText.text = "Player " + player + " wins";
        m_EventSystem.SetRestartPriority();
        m_EndGame.SetActive(true);
    }
}
