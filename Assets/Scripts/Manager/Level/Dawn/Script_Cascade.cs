using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_Cascade : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<PlayerController>())
        {
            Debug.Log("Touch");
            other.GetComponent<Rigidbody2D>().gravityScale = 1.2f;
        }
    }
}
