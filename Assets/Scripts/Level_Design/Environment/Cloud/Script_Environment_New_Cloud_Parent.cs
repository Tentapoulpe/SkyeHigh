using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_Environment_New_Cloud_Parent : MonoBehaviour
{
    private bool b_wantToBeDestroy = true;
    private bool b_goRight;
    private bool b_getDirection;
    private Rigidbody2D my_rigidbody;
    private float f_cloudVelocity;

    private void Start()
    {
        my_rigidbody = GetComponent<Rigidbody2D>();
        f_cloudVelocity = Random.Range(1f, 3f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!b_getDirection)
        {
            if (collision.CompareTag("LeftBorder"))
            {
                Debug.Log("goright");
                b_goRight = true;
            }
            else
            {
                Debug.Log("goleft");
                b_goRight = false;
            }

            b_getDirection = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("LeftBorder") && !b_goRight)
        {
            Destroy();
        }
        else if (collision.CompareTag("RightBorder") && b_goRight)
        {
            Destroy();
        }
    }

    private void Update()
    {
        if (b_goRight)
        {
            my_rigidbody.velocity = new Vector2(f_cloudVelocity, 0);
        }
        else
            my_rigidbody.velocity = new Vector2(-f_cloudVelocity, 0);
    }

    private void Destroy()
    {
        if (b_wantToBeDestroy)
        {
            GameManager.Instance.UnSpawnCloudParent(gameObject);
            Destroy(this.gameObject);
            b_wantToBeDestroy = false;
        }
    }
}
