using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EventSystemTool : MonoBehaviour
{
    private StandaloneInputModule s_Module;
    private EventSystem s_System;
    private GameObject StartButton;
    public GameObject RestartButton;

    private void Awake()
    {
        s_Module = GetComponent<StandaloneInputModule>();
        s_System = GetComponent<EventSystem>();
        StartButton = s_System.firstSelectedGameObject;
        string[] names = Input.GetJoystickNames();
        if (names.Length > 0)
        {
            if (names[0].Length == 33)
            {
                s_Module.submitButton = "F1_XBOX_P1";
                s_Module.cancelButton = "F2_XBOX_P1";
            }
        }
    }



}
