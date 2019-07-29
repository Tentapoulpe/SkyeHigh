using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_Cloud : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            GetComponentInParent<Script_Player_Manager>().Death();
        }
    }
}
