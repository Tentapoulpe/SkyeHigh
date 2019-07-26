using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_Player : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponentInParent<Script_Player_Manager>().dash_state == Script_Player_Manager.DashState.Dashing && collision.CompareTag("Player"))
        {
            GetComponentInParent<Script_Player_Manager>().Death();
        }
    }
}
