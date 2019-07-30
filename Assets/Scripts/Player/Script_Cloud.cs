using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_Cloud : MonoBehaviour
{
    public Script_Player_Manager player;
    private void Awake()
    {
        transform.SetParent(null);
    }

    private void Update()
    {
        Vector2 CloudPos = new Vector2(player.transform.position.x, player.transform.position.y + 1.5f);
        if (transform.position != (Vector3)CloudPos)
        transform.position = Vector2.MoveTowards(transform.position, CloudPos, Time.deltaTime * (Vector2.Distance(transform.position, CloudPos) * 7f));
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && collision.transform.parent != player.transform)
        {
            player.GetComponent<Script_Player_Manager>().Death();
        }
    }
}
