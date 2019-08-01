using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Menu_Manager : MonoBehaviour
{
    public static UI_Menu_Manager Instance { get; private set; }

    public GameObject m_characterWindow;

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

    public void DisplayUI(GameObject current_go_to_display)
    {
        if(!current_go_to_display.activeSelf)
        current_go_to_display.SetActive(true);
    }

    public void HideUI(GameObject current_go_to_hide)
    {
        if(current_go_to_hide.activeSelf)
        current_go_to_hide.SetActive(false);
    }
}
