using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_Top_Border : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController player = collision.GetComponentInParent<PlayerController>();
        if (player)
        {
            Debug.Log("DETECT");
            player.PlayerIsTopScreen();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        PlayerController player = collision.GetComponentInParent<PlayerController>();
        if (player)
        {
            Debug.Log("!DETECT");
            player.PlayerIsNotTopScreen();
        }
    }
}
