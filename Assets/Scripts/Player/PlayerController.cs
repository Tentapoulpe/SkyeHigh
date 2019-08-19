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
    private float m_dashMinimalSpeed = 20f;

    private bool b_playerIsDashing;
    private bool b_canDash = true;
    private float f_dashCooldown = 0f;
    public float m_timerDashing;
    public float f_currentTimerDashing;
    [Space]

    [Header("Environment")]
    private float m_cloudSlow;
    private float m_delayToRegenerate;
    [Space]

    private Rigidbody2D rigidbodyPlayer = null;
    private Animator a_Animator = null;

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
    private bool b_mustFall = false;

    private float f_currentStun = 0;

    void Start()
    {
        SetUpPlayer();
        rigidbodyPlayer = GetComponent<Rigidbody2D>();
        a_Animator = GetComponent<Animator>();
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

        if (b_isInCloud)
        {
            rigidbodyPlayer.AddForce(new Vector2(horizontal, vertical) / m_cloudSlow, ForceMode2D.Impulse);
        }
        else
            rigidbodyPlayer.AddForce(new Vector2(horizontal, vertical), ForceMode2D.Impulse);

        //Animator
        a_Animator.SetFloat("Vertical", Mathf.Clamp(vertical,-1,1));
        a_Animator.SetFloat("Horizontal", Mathf.Clamp(horizontal,-1,1));
        if (horizontal > 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
        else if (horizontal < 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }
        if (vertical > 0)
        {
            GetComponent<SpriteRenderer>().flipY = false;
        }
        else if (vertical < 0)
        {
            GetComponent<SpriteRenderer>().flipY = true;
        }

    }

    void Update()
    {
        m_textHealth.text = f_currentHealth.ToString();

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

        if(!b_playerIsDashing && b_mustFall)
        {
            Fall();
            b_mustFall = false;
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
                a_Animator.SetBool("Dash", false);
            }
        }

        if(b_playerIsDashing)
        {
            f_currentTimerDashing -= Time.deltaTime;
            if (f_currentTimerDashing <= 0)
            {
                b_playerIsDashing = false;
                rigidbodyPlayer.velocity *= new Vector2(0.25f,0.25f);
            }
            else
            {
                DashApplyMinimalSpeed();
            }
        }

        if (f_currentHealth >= m_maxHealth)
        {
            b_canRegenerate = false;
        }
        else
        {
            b_canRegenerate = true;
        }
    }

    public bool ReturnDashState()
    {
        return b_playerIsDashing;
    }

    public void DashApplyMinimalSpeed()
    {
        float currentSpeed = Mathf.Abs(rigidbodyPlayer.velocity.x) + Mathf.Abs(rigidbodyPlayer.velocity.y);
        if(currentSpeed < m_dashMinimalSpeed)
        {
            float hRatio = (Mathf.Abs(rigidbodyPlayer.velocity.x) * 100) / currentSpeed;
            //Debug.Log("hRatio is: " + hRatio + "%");
            float vRatio = 100 - hRatio;
            //Debug.Log("vRatio is: " + vRatio + "%");
            float hSpeed = m_dashMinimalSpeed * (hRatio / 100) * Mathf.Sign(rigidbodyPlayer.velocity.x);
            float vSpeed = m_dashMinimalSpeed * (vRatio / 100) * Mathf.Sign(rigidbodyPlayer.velocity.y);
            rigidbodyPlayer.velocity = new Vector2(hSpeed,vSpeed);
        }
    }

    public void Dash()
    {
        a_Animator.SetBool("Dash",true);
        b_canDash = false;
        b_playerIsDashing = true;
        f_currentTimerDashing = m_timerDashing;
        f_dashCooldown = m_maxDashCooldown;
        rigidbodyPlayer.AddForce(new Vector2(Input.GetAxis("Horizontal_P" + playerNumber), Input.GetAxis("Vertical_P" + playerNumber)) * m_dashPower, ForceMode2D.Impulse);
        DecreaseCloud();
    }

    public void StopDash()
    {
        b_playerIsDashing = false;
        rigidbodyPlayer.velocity = Vector2.zero;
    }

    public void DecreaseCloud()
    {
        if (f_currentHealth != 0)
        {
            f_currentHealth -= m_dashCost;
            if (f_currentHealth <= 0)
            {
                if (!b_playerIsDashing)
                    Fall();
                else
                    b_mustFall = true;
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
            f_currentHealth += f_health;
            if (f_currentHealth >= m_maxHealth)
            {
                f_currentHealth = m_maxHealth;
                return;
            }
            UpdateCloudSprite();
        }
    }

    public void SetCloudHealth(float f_health)
    {
        f_currentHealth = f_health;
        Mathf.Clamp(f_currentHealth,0f, m_maxHealth);
        if (f_currentHealth > 0 && b_mustFall)
            b_mustFall = false;
        UpdateCloudSprite();
    }

    public float GetCloudHealth()
    {
        return f_currentHealth;
    }

    public void UpdateCloudSprite()
    {
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
    }

    public void CloudSlow()
    {
        b_isInCloud = true;
    }

    public void CloudUnSlow()
    {
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

    public void SetStun(float stunValue)
    {
        f_currentStun = stunValue;
        b_canMove = false;
        b_canDash = false;
        rigidbodyPlayer.gravityScale = m_gravityFall;
    }

    public void EndStun()
    {
        f_currentStun = 0;
        b_canMove = true;
        b_canDash = true;
        rigidbodyPlayer.gravityScale = 0;
    }

    public void StunCountdown(float deltaTime)
    {
        if(f_currentStun > 0)
        {
            f_currentStun -= deltaTime;
            if(f_currentStun <= 0)
            {
                EndStun();
            }
        }
    }



    public void Fall()
    {
        a_Animator.SetTrigger("Fall");
        LockControls();
        if (cloud)
        {
            cloud.DestroyCloud();
        }
        b_playerIsDashing = false;
        rigidbodyPlayer.gravityScale = m_gravityFall;
        gameObject.GetComponent<Collider2D>().isTrigger = true;
    }

    public void Death()
    {
        if (cloud)
            cloud.DestroyCloud();
        GameManager.Instance.PlayerDied(playerNumber);
        Destroy(transform.gameObject);
    }

    public void SetPlayerNumber(int i)
    {
        playerNumber = i;
    }

    public void LockControls()
    {
        b_canMove = false;
        rigidbodyPlayer.velocity = Vector3.zero;
    }
    public void UnlockControls()
    {
        b_canMove = true;
    }
}
