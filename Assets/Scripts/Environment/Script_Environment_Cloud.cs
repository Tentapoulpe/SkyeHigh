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
            Debug.Log("InCloud");
            collision.gameObject.GetComponentInParent<PlayerController>().CloudSlow();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            if(collision.GetComponentInParent<PlayerController>().CanRegenerate() == true)
            {
                collision.GetComponentInParent<PlayerController>().IncreaseCloud(f_healthToRegenerate);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            collision.gameObject.GetComponentInParent<PlayerController>().CloudUnSlow();
        }
    }
}
