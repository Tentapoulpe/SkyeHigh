using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Script_Player_Manager : MonoBehaviour
{
    public enum DashState { Ready, Dashing, Cooldown }
    public enum Player { Skye, Dawn, KingNimbus, CommanderBolt, Smog, Angel, Sirocco, Entity, BirdMan, DarkSkye}

    [Header("Player")]
    public Player playerList;

    [Header("Movement")]
    public float m_movementSpeed;
    private float f_moveHorizontal;
    private float f_moveVertical;
    private Vector2 v_movement;
    private Rigidbody2D rigidbodyPlayer { get { return GetComponent<Rigidbody2D>(); } }

    [Header("Dash")]
    public float m_dashPower;
    public int m_maxDashCooldown;

    private bool b_canDash = true;
    private float f_dashCooldown = 0f;

    [Header("Cloud")]
    public SpriteRenderer cloudSprite;
    private Vector3 v_currentCloudScale;
    public int m_maxHealth;
    private int i_currentHealth;

    void Start()
    {
        i_currentHealth = m_maxHealth;

        switch(playerList)
        {
            case Player.Skye:
                Debug.Log("Skye");
                break;
            case Player.Dawn:
                Debug.Log("Dawn");
                break;
            case Player.KingNimbus:
                Debug.Log("KingNimbus");
                break;
            case Player.CommanderBolt:
                Debug.Log("CommanderBolt");
                break;
            case Player.Smog:
                Debug.Log("Smog");
                break;
            case Player.Angel:
                Debug.Log("Angel");
                break;
            case Player.Sirocco:
                Debug.Log("Sirocco");
                break;
            case Player.Entity:
                Debug.Log("Entity");
                break;
            case Player.BirdMan:
                Debug.Log("BirdMan");
                break;
            case Player.DarkSkye:
                Debug.Log("DarkSkye");
                break;
        }   
    }

    private void Movement()
    {
        f_moveHorizontal = Input.GetAxis("Horizontal");
        f_moveVertical = Input.GetAxis("Vertical");
        v_movement = new Vector2(f_moveHorizontal, f_moveVertical);
        rigidbodyPlayer.velocity = (v_movement * m_movementSpeed);
    }

    void Update()
    {
        Movement();

        if (Input.GetKeyDown("e") && b_canDash)
        {
            Dash();
        }

        if (Input.GetKeyDown("m"))
        {
            Script_Game_Manager.Instance.RestartGame();
        }

        if (f_dashCooldown > 0)
        {
            f_dashCooldown -= Time.deltaTime;
            if (f_dashCooldown <= 0)
            {
                f_dashCooldown = 0;
                b_canDash = true;
                Debug.Log("CanDash");
            }
        }
    }

    public void Dash()
    {
        Debug.Log("dashing");
        b_canDash = false;
        f_dashCooldown = m_maxDashCooldown;
        rigidbodyPlayer.AddForce(v_movement * m_dashPower);
    }

    public void DecreaseCloud()
    {
        Debug.Log("Decrease");
        if (i_currentHealth != 0)
        {
            cloudSprite.transform.localScale = cloudSprite.transform.localScale / 2;
            i_currentHealth--;
        }
        else
        {
            Death();
        }
    }

    public void IncreaseCloud()
    {
        Debug.Log("Increase");
        if (i_currentHealth != m_maxHealth)
        {
            cloudSprite.transform.localScale = cloudSprite.transform.localScale * 2;
            i_currentHealth = m_maxHealth;
        }
    }

    public void ReduceVelocity()
    {
        Debug.Log("ReduceVelocity");
    }

    public void Death()
    {
        Debug.Log("Death");
        Destroy(transform.gameObject);
    }
}
