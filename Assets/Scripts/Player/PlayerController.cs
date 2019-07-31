using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public enum DashState { Ready, Dashing, Cooldown }
    public enum Player { Skye, Dawn, KingNimbus, CommanderBolt, Smog, Angel, Sirocco, Entity, BirdMan, DarkSkye}

    [Header("Player")]
    public Player playerList;

    [Header("Movement")]
    public float m_maxVerticalDownSpeed;
    public float m_maxVerticalUpSpeed;
    public float m_maxHorizontalSpeed;
    [Space]
    public float m_gravityMultiplier;
    public float m_horizontalAcceleration;
    public float m_VerticalAcceleration;
    private Rigidbody2D rigidbodyPlayer { get { return GetComponent<Rigidbody2D>(); } }

    [Header("Dash")]
    public float m_dashPower;
    public float m_maxDashCooldown;

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
    

    private void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal") * m_horizontalAcceleration * Time.deltaTime;
        if (horizontal >= 0)
        {
            if (horizontal > m_maxHorizontalSpeed)
            horizontal = m_maxHorizontalSpeed;
        }
        else
        {
            if (horizontal < -m_maxHorizontalSpeed)
                horizontal = -m_maxHorizontalSpeed;
        }
            
        float vertical = Input.GetAxis("Vertical") * m_VerticalAcceleration * Time.deltaTime;
        if (vertical > 0)
        {
            vertical /= m_gravityMultiplier;
                if (vertical > m_maxVerticalUpSpeed)
                    vertical = m_maxVerticalUpSpeed;
            
        }
        else if (vertical < 0)
        {
            vertical *= m_gravityMultiplier;
                if (vertical < -m_maxVerticalDownSpeed)
                    vertical = -m_maxVerticalDownSpeed;
            
        }
        rigidbodyPlayer.AddForce(new Vector2(horizontal, vertical), ForceMode2D.Impulse);
    }
    void Update()
    {

        if (Input.GetKeyDown("space") && b_canDash)
        {
            Dash();
        }

        if (Input.GetKeyDown("m"))
        {
            GameManager.Instance.RestartGame();
        }

        if (f_dashCooldown > 0)
        {
            f_dashCooldown -= Time.deltaTime;
            if (f_dashCooldown <= 0)
            {
                f_dashCooldown = 0;
                b_canDash = true;
            }
        }
    }

    public void Dash()
    {
        b_canDash = false;
        f_dashCooldown = m_maxDashCooldown;
        rigidbodyPlayer.AddForce(new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) * m_dashPower, ForceMode2D.Impulse);
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
