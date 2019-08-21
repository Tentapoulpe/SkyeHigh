using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_Top_Border : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("DETECT");
            collision.GetComponent<PlayerController>().PlayerIsTopScreen();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            Debug.Log("!DETECT");
            collision.GetComponent<PlayerController>().PlayerIsNotTopScreen();
        }
    }
}
