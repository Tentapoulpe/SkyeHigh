using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_Level_Manager : MonoBehaviour
{
    public Script_Scriptable_Level m_myLevel;

    public void PlayAnimation()
    {
        gameObject.transform.GetComponentInParent<Animator>().enabled = true;
    }

    public void StopAnimation()
    {
        gameObject.transform.GetComponentInParent<Animator>().enabled = false;
    }
}
