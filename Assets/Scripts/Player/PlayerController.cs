using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [Header("Player")]
    public int playerNumber;

    private string s_fireInput = "";
    private bool b_canMove = true;

    [Header("Movement")]
    public float m_maxVerticalDownSpeed;
    public float m_maxVerticalUpSpeed;
    public float m_maxHorizontalSpeed;
    [Space]
    public float m_gravityMultiplier;
    public float m_horizontalAcceleration;
    public float m_VerticalUpAcceleration;
    public float m_VerticalDownAcceleration;

    private Rigidbody2D rigidbodyPlayer = null;

    [Header("Dash")]
    public float m_dashPower;
    public float m_maxDashCooldown;

    private bool b_canDash = true;
    private float f_dashCooldown = 0f;

    [Header("Cloud")]
    public SpriteRenderer cloudSprite;
    private PlayerCloud cloud = null;
    public int m_maxHealth;

    private Vector3 v_currentCloudScale;
    private int i_currentHealth;

    void Start()
    {
        rigidbodyPlayer = GetComponent<Rigidbody2D>();
        i_currentHealth = m_maxHealth; 
    }
    

    private void FixedUpdate()
    {
        if (!b_canMove)
            return;
        float horizontal = Input.GetAxis("Horizontal_P" + playerNumber) * m_horizontalAcceleration * Time.deltaTime;
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
            
        float vertical = Input.GetAxis("Vertical_P" + playerNumber) * Time.deltaTime;
        if (vertical > 0)
        {
            vertical *= m_VerticalUpAcceleration;
            vertical /= m_gravityMultiplier;
                if (vertical > m_maxVerticalUpSpeed)
                    vertical = m_maxVerticalUpSpeed;

            
        }
        else if (vertical < 0)
        {
            vertical *= m_VerticalDownAcceleration;
            vertical *= m_gravityMultiplier;
                if (vertical < -m_maxVerticalDownSpeed)
                    vertical = -m_maxVerticalDownSpeed;
            
        }
        rigidbodyPlayer.AddForce(new Vector2(horizontal, vertical), ForceMode2D.Impulse);
    }
    void Update()
    {
        string[] names = Input.GetJoystickNames();
        if (names[playerNumber - 1].Length == 19)
        {
            s_fireInput = "F_PS4_P" + playerNumber;
        }
        else if (names[playerNumber - 1].Length == 33)
        {
            s_fireInput = "F_XBOX_P" + playerNumber;
        }
        else
        {
            s_fireInput = "F_PC_P" + playerNumber;
        }


        if (Input.GetButtonDown(s_fireInput) && b_canDash && b_canMove)
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
        rigidbodyPlayer.AddForce(new Vector2(Input.GetAxis("Horizontal_P" + playerNumber), Input.GetAxis("Vertical_P" + playerNumber)) * m_dashPower, ForceMode2D.Impulse);

    }

    public void DecreaseCloud()
    {
        if (i_currentHealth != 0)
        {
            cloudSprite.transform.localScale = cloudSprite.transform.localScale / 2;
            i_currentHealth--;
        }
        else
        {
            Fall();
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

    public void setCloud(PlayerCloud Cloud)
    {
        cloud = Cloud;
    }

    public void Fall()
    {
        b_canMove = false;
        if (cloud)
            cloud.DestroyCloud();
        rigidbodyPlayer.gravityScale = 2f;
    }

    public void Death()
    {
        if (cloud)
            cloud.DestroyCloud();
        Destroy(transform.gameObject);
    }

}
