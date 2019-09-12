using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    public EventSystemTool m_EventSystem;
    public GameObject m_MainMenu;
    public GameObject m_CharacterSelection;
    public GameObject m_MapSelection;
    public GameObject m_EndGame;
    public GameObject m_EndRound;
    public GameObject m_LoadingScreen;
    public TextMeshProUGUI m_EndGameText;
    public TextMeshProUGUI m_EndRoundText;
    public List<GameObject> m_Players;
    public List<GameObject> m_PlayersDisplay;
    public List<Sprite> m_Characters;
    public List<TextMeshProUGUI> m_EndRoundScore;
    public List<TextMeshProUGUI> m_EndGameScore;
    private int i_mapIdx = 0;
    public List<GameObject> m_MapStage;
    public UnityEngine.UI.Image LoadingBar;

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
        m_MainMenu.SetActive(true);
    }

    public void ShowMapMenu()
    {
        m_CharacterSelection.SetActive(false);
        m_MapSelection.SetActive(true);
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

    void EnterCharacterSelection()
    {
        GameManager.Instance.GoToCharacterSelection();
    }

    public void ChangeMap(float f_direction)
    {
        if(f_direction >= 0 && i_mapIdx < 1)
        {
            i_mapIdx++;
            GameManager.Instance.FocusMap();
        }
        else if (i_mapIdx > 0)
        {
            i_mapIdx--;
            GameManager.Instance.FocusMap();
        }
    }

    public GameObject GetMapTransformation()
    {
        return m_MapStage[i_mapIdx];
    }

    public void GameScreen()
    {
        m_CharacterSelection.SetActive(false);
        m_MapSelection.SetActive(false);
    }

    public void DisplayEndGame(List<int> player, List<int> score)
    {
        if (player.Count == 1)
        {
            m_EndGameText.text = "Player " + player[0] + " wins";
        }
        else if (player.Count > 1)
        {
            m_EndGameText.text = "Draw ! ";
            for (int i = 0; i < player.Count; i++)
            {
                if (i != (player.Count - 1))
                {
                    m_EndGameText.text += "Player " + player[i] + ", ";
                }
                else
                {
                    m_EndGameText.text += "and Player " + player[i] + " win";
                }
            }
        }
        else if (player.Count == 0)
        {
            m_EndGameText.text = "Draw !! no one win";
        }

        for (int i = 0; i < score.Count; i++)
        {

            m_EndGameScore[i].gameObject.SetActive(true);
            m_EndGameScore[i].text = score[i].ToString();
        }
        m_EndGame.SetActive(true);
    }

    public void DisplayEndGame()
    {
        m_EndGameText.text = "Draw Debug";
        m_EndGame.SetActive(true);
    }

    public void DisplayEndRound(int player, List<int> score)
    {
        if (player != 0)
        {
            m_EndRoundText.text = "Player " + player + " wins";
        }
        else
        {

            m_EndRoundText.text = "Draw !!";
        }

        m_EndRound.SetActive(true);
        for (int i = 0; i < score.Count; i++)
        {
            m_EndRoundScore[i].text = score[i].ToString();
            m_EndRoundScore[i].gameObject.SetActive(true);
        }
    }

    public void RestartGame()
    {
        m_EndGame.SetActive(false);
        GameManager.Instance.RestartNewGame();
    }

    public void ReturnToMenu()
    {
        m_EndGame.SetActive(false);
        GameManager.Instance.ReturnMenu();
        m_MainMenu.SetActive(true);
    }

    public void RestartRound()
    {
        m_EndRound.SetActive(false);
    }

    public void UpdateLoadingScreen(float state)
    {
        if (!m_LoadingScreen.activeSelf)
        {
            m_LoadingScreen.SetActive(true);
            LoadingBar.fillAmount = 0f;
        }
        LoadingBar.fillAmount = state;
    }

    public void StopLoading()
    {
        m_LoadingScreen.SetActive(false);
    }
}
