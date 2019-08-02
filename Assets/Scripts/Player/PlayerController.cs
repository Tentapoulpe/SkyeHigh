using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [Header("Player")]
    public int playerNumber;
    public Script_Scriptable_Characters m_character_info;

    [Header("Movement")]
    private bool b_canMove = true;
    [Space]

    [Header("MovementHorizontal")]
    private float m_maxVerticalDownSpeed;
    private float m_maxVerticalUpSpeed;
    private float m_maxHorizontalSpeed;
    private float m_maxHorizontalDrag;
    [Space]

    [Header("MovementVertical")]
    private float m_horizontalAcceleration;
    private float m_VerticalUpAcceleration;
    private float m_VerticalDownAcceleration;
    private float m_maxVerticalDrag;
    [Space]

    [Header("Gravity")]
    private float m_gravityMultiplier;
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
    [Space]

    [Header("Environment")]
    private float m_cloudSlow;
    private float m_delayToRegenerate;
    [Space]

    private Rigidbody2D rigidbodyPlayer = null;

    [Header("Cloud")]
    public SpriteRenderer cloudSprite;
    private PlayerCloud cloud = null;
    private int m_maxHealth;
    private bool b_isInCloud;

    private Vector3 v_currentCloudScale;
    private int i_currentHealth;

    private string s_fireInput = "";

    void Start()
    {
        SetUpPlayer();
        rigidbodyPlayer = GetComponent<Rigidbody2D>();
        i_currentHealth = m_maxHealth; 
    }
    
    private void SetUpPlayer()
    {
        m_maxVerticalUpSpeed = m_character_info.m_maxVerticalUpSpeed;
        m_maxHorizontalSpeed = m_character_info.m_maxHorizontalSpeed;
        m_horizontalAcceleration = m_character_info.m_horizontalAcceleration;
        m_maxHorizontalDrag = m_character_info.m_maxHorizontalDrag;
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


        if(b_isInCloud)
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
        b_playerIsDashing = true;
        b_canDash = false;
        f_dashCooldown = m_maxDashCooldown;
        rigidbodyPlayer.AddForce(new Vector2(Input.GetAxis("Horizontal_P" + playerNumber), Input.GetAxis("Vertical_P" + playerNumber)) * m_dashPower, ForceMode2D.Impulse);
        DecreaseCloud();
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
            i_currentHealth++;
        }
    }

    public void CloudSlow()
    {
        Debug.Log("SLOW");
        b_isInCloud = true;
    }

    public void CloudUnSlow()
    {
        Debug.Log("UNSLOW");
        b_isInCloud = false;
    }

    public float ReturnTimeBeforeRegenerate()
    {
        return m_delayToRegenerate;
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
