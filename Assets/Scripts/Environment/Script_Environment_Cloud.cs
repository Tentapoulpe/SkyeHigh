using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_Environment_Cloud : MonoBehaviour
{
    private Rigidbody2D my_rigidbody;

    //public static event 

    private void Start()
    {
        my_rigidbody = GetComponent<Rigidbody2D>();    
    }

    private void OnTriggeEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("blblbl");
            collision.gameObject.GetComponentInParent<PlayerController>().CloudSlow();
            //StartCoroutine(WaitToRegenerate(collision.gameObject));
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            collision.gameObject.GetComponentInParent<PlayerController>().CloudUnSlow();
        }
    }

    IEnumerator WaitToRegenerate(GameObject g_player)
    {
        yield return new WaitForSeconds(g_player.GetComponentInParent<PlayerController>().ReturnTimeBeforeRegenerate());
        g_player.GetComponentInParent<PlayerController>().IncreaseCloud();
    }

}
