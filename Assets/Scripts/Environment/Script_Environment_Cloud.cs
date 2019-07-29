using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_Environment_Cloud : MonoBehaviour
{
    private Rigidbody2D my_rigidbody;

    private void Start()
    {
        my_rigidbody = GetComponent<Rigidbody2D>();    
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponentInParent<Script_Player_Manager>().ReduceVelocity();
            collision.GetComponentInParent<Script_Player_Manager>().IncreaseCloud();
        }
    }
}
