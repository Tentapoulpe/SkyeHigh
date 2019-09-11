using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCloud : MonoBehaviour
{
    public PlayerController player;
    private void Awake()
    {
        transform.SetParent(null);
        player.SetCloud(this);
    }

    private void Update()
    {
        if (player == null)
        {
            return;
        }
        Vector2 CloudPos = new Vector2(player.transform.position.x, player.transform.position.y + 1.5f);
        if (transform.position != (Vector3)CloudPos)
        transform.position = Vector2.MoveTowards(transform.position, CloudPos, Time.deltaTime * (Vector2.Distance(transform.position, CloudPos) * 10f));
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (player != null)
        {
            if (collision.CompareTag("Player") && collision.gameObject != player.transform.gameObject && collision.gameObject.GetComponent<PlayerController>().ReturnDashState())
            {
                player.Fall();
                collision.gameObject.GetComponent<PlayerController>().StopDash(true);
                if (collision.gameObject.GetComponent<PlayerController>().GetCloudHealth() <= 0)
                    collision.gameObject.GetComponent<PlayerController>().SetCloudHealth(1f);

                DestroyCloud();
            }

        }
        
    }

    public void DestroyCloud()
    {
        Destroy(gameObject);
    }
}
