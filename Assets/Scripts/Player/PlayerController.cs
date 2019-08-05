using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("Player")]
    public int playerNumber;
    public Script_Scriptable_Characters m_character_info;

    [Header("Movement")]
    private bool b_canMove = true;
    [Space]

    [Header("MovementHorizontal")]
    private float m_maxHorizontalSpeed;
    private float m_maxHorizontalDrag;
    private float m_horizontalAcceleration;
    [Space]

    [Header("MovementVertical")]
    private float m_maxVerticalUpSpeed;
    private float m_maxVerticalDownSpeed;
    private float m_VerticalUpAcceleration;
    private float m_VerticalDownAcceleration;
    private float m_maxVerticalDrag;
    [Space]

    [Header("Gravity")]
    private float m_gravityMultiplier;
    public float m_gravityFall;
    [Space]

    [Header("Dash")]
    private float m_dashPower;
    private float m_maxDashCooldown;
    private float m_dashCost;
    private float m_dashPostStun;
    private float m_dashStun;

    private bool b_playerIsDashing;
    private bool b_canDash = true;
    private float f_dashCooldown = 0f;
    private bool b_lastDash = false;
    [Space]

    [Header("Environment")]
    private float m_cloudSlow;
    private float m_delayToRegenerate;
    [Space]

    private Rigidbody2D rigidbodyPlayer = null;

    [Header("Cloud")]
    public SpriteRenderer m_cloudSprite;
    public List<SpriteRenderer> m_cloudSpriteList = new List<SpriteRenderer>();
    private PlayerCloud cloud = null;
    private float m_maxHealth = 100;
    private bool b_isInCloud;
    private bool b_canRegenerate = true;
    private float f_currentHealth;
    public Text m_textHealth;

    private string s_fireInput = "";

    void Start()
    {
        SetUpPlayer();
        rigidbodyPlayer = GetComponent<Rigidbody2D>();
        f_currentHealth = m_maxHealth;
        m_textHealth.text = f_currentHealth.ToString();
    }
    
    private void SetUpPlayer()
    {
        m_maxVerticalUpSpeed = m_character_info.m_maxVerticalUpSpeed;
        m_maxHorizontalSpeed = m_character_info.m_maxHorizontalSpeed;
        m_horizontalAcceleration = m_character_info.m_horizontalAcceleration;
        m_maxHorizontalDrag = m_character_info.m_maxHorizontalDrag;
        m_maxVerticalDownSpeed = m_character_info.m_maxVerticalDownSpeed;
        m_VerticalUpAcceleration = m_character_info.m_VerticalUpAcceleration;
        m_VerticalDownAcceleration = m_character_info.m_VerticalDownAcceleration;
        m_maxVerticalDrag = m_character_info.m_maxVerticalDrag;
        m_gravityMultiplier = m_character_info.m_gravityMultiplier;
        m_dashPower = m_character_info.m_dashPower;
        m_maxDashCooldown = m_character_info.m_maxDashCooldown;
        m_dashCost = m_character_info.m_dashCost;
        m_dashPostStun = m_character_info.m_dashPostStun;
        m_dashStun = m_character_info.m_dashStun;
        m_cloudSlow = m_character_info.m_cloudSlow;
        m_delayToRegenerate = m_character_info.m_delayToRegenerate;
        m_maxHealth = m_character_info.m_maxHealth;
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

        if(rigidbodyPlayer.velocity.x > m_maxHorizontalSpeed || rigidbodyPlayer.velocity.y > Mathf.Abs(m_maxVerticalUpSpeed))
        {
            b_playerIsDashing = true;
            if (rigidbodyPlayer.velocity.x == m_maxHorizontalSpeed || rigidbodyPlayer.velocity.y == Mathf.Abs(m_maxVerticalUpSpeed) && b_lastDash)
            {
                Fall();
            }
        }

        if (b_isInCloud)
        {
            rigidbodyPlayer.AddForce(new Vector2(horizontal, vertical) / m_cloudSlow, ForceMode2D.Impulse);
        }
        else
            rigidbodyPlayer.AddForce(new Vector2(horizontal, vertical), ForceMode2D.Impulse);

    }

    void Update()
    {
        string[] names = Input.GetJoystickNames();
        if (names[playerNumber - 1].Length == 19)
        {
            s_fireInput = "F1_PS4_P" + playerNumber;
        }
        else if (names[playerNumber - 1].Length == 33)
        {
            s_fireInput = "F1_XBOX_P" + playerNumber;
        }
        else
        {
            s_fireInput = "F1_PC_P" + playerNumber;
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
            b_playerIsDashing = false;
            f_dashCooldown -= Time.deltaTime;
            if (f_dashCooldown <= 0)
            {
                f_dashCooldown = 0;
                b_canDash = true;
            }
        }
    }

    public bool ReturnDashState()
    {
        return b_playerIsDashing;
    }

    public void Dash()
    {
        b_canDash = false;
        f_dashCooldown = m_maxDashCooldown;
        rigidbodyPlayer.AddForce(new Vector2(Input.GetAxis("Horizontal_P" + playerNumber), Input.GetAxis("Vertical_P" + playerNumber)) * m_dashPower, ForceMode2D.Impulse);
        DecreaseCloud();
    }

    public void DecreaseCloud()
    {
        Debug.Log("IncreaseCloud");
        if (f_currentHealth != 0)
        {
            f_currentHealth -= m_dashCost;
            if (f_currentHealth <= 0)
            {
                b_lastDash = true;
            }
        }
        UpdateCloudSprite();
    }

    public bool CanRegenerate()
    {
        return b_canRegenerate;
    }

    public void IncreaseCloud(float f_health)
    {
        if (f_currentHealth <= m_maxHealth)
        {
            Debug.Log("IncreaseCloud");
            b_canRegenerate = false;
            f_currentHealth += f_health;

            if (f_currentHealth >= m_maxHealth)
            {
                f_currentHealth = m_maxHealth;
            }
            UpdateCloudSprite();
        }
    }

    public void UpdateCloudSprite()
    {
        Debug.Log("UpdateCloud");
        if (f_currentHealth < 100)
        {
            if(f_currentHealth < 75)
            {
                if (f_currentHealth < 50)
                {
                    if (f_currentHealth < 25)
                    {
                        m_cloudSprite = m_cloudSpriteList[0];
                    }
                    m_cloudSprite = m_cloudSpriteList[1];
                }
                m_cloudSprite = m_cloudSpriteList[2];
            }
            m_cloudSprite = m_cloudSpriteList[3];
        }
        m_textHealth.text = f_currentHealth.ToString();
    }

    public void CloudSlow()
    {
        Debug.Log("Slow");
        b_isInCloud = true;
    }

    public void CloudUnSlow()
    {
        Debug.Log("UnSlow");
        b_isInCloud = false;
    }

    public float ReturnTimeBeforeRegenerate()
    {
        return m_delayToRegenerate;
    }

    public void SetCloud(PlayerCloud Cloud)
    {
        cloud = Cloud;
    }

    public void Fall()
    {
        b_canMove = false;
        if (cloud)
            cloud.DestroyCloud();
        rigidbodyPlayer.gravityScale = m_gravityFall;
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
    }

    public void Death()
    {
        if (cloud)
            cloud.DestroyCloud();
        Destroy(transform.gameObject);
    }
}
