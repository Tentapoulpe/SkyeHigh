using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_Environment_Cloud : MonoBehaviour
{
    private Rigidbody2D my_rigidbody;
    public float f_healthToRegenerate;

    private void Start()
    {
        my_rigidbody = GetComponent<Rigidbody2D>();    
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && collision.gameObject.GetComponentInParent<PlayerController>().CanRegenerate())
        {
            collision.gameObject.GetComponentInParent<PlayerController>().CloudSlow();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            collision.GetComponentInParent<PlayerController>().IncreaseCloud(f_healthToRegenerate);
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

    //IEnumerator WaitToRegenerate(GameObject g_player)
    //{
    //    yield return new WaitForSeconds(g_player.GetComponentInParent<PlayerController>().ReturnTimeBeforeRegenerate());
    //    g_player.GetComponentInParent<PlayerController>().IncreaseCloud();
    //}

}
