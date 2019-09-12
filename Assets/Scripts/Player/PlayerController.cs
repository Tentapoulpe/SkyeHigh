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

    [Header("Gravity")]
    public float m_gravityFall;
    [Space]

    [Header("Dash")]
    private float m_dashMinimalSpeed = 20f;

    private bool b_playerIsDashing;
    private bool b_canDash = true;
    private float f_dashCooldown = 0f;
    public float m_timerDashing;
    public float f_currentTimerDashing;

    public ParticleSystem m_DashParticle;
    public Material m_DashParticleMat;
    public Texture m_DashTexture;
    [Space]

    [Header("Environment")]
    private float m_delayToRegenerate;
    private bool b_isTopLimit;
    public float m_timerBeforeTakeDamage;
    private float f_currentTimerBeforeTakeDamage;
    public float m_amountDamage;
    [Space]

    private Rigidbody2D rigidbodyPlayer = null;
    private Animator a_Animator = null;

    [Header("Cloud")]
    public SpriteRenderer m_cloudSprite;
    public List<Sprite> m_cloudSpriteList;
    private PlayerCloud cloud = null;
    private bool b_isInCloud;
    private bool b_canRegenerate = true;
    private float f_currentHealth;
    public Text m_textHealth;
    [Space]

    private string s_fireInput = "";
    private string s_cancelInput = "";
    private bool b_mustFall = false;
    bool b_endGame = false;

    [Header("Stun")]
    private float f_currentStun = 0;
    private float f_collisionStunTime = 1f;
    private float f_stunGravity = 3f;
    [Range(1f, 2f)]
    public float m_timeReduceStun;
    private float f_currenttimeReduceStun = 1;

    [Header("VFX")]
    public ParticleSystem m_vfxStun;


    void Start()
    {
        m_DashParticle.GetComponent<ParticleSystemRenderer>().material = new Material(m_DashParticleMat);
        rigidbodyPlayer = GetComponent<Rigidbody2D>();
        a_Animator = GetComponent<Animator>();
        f_currentHealth = m_character_info.m_maxHealth;
        m_textHealth.text = f_currentHealth.ToString();
        UpdateCloudSprite();
    }
    

    private void FixedUpdate()
    {
        if (!b_canMove)
            return;
        float horizontal = Input.GetAxis("Horizontal_P" + playerNumber) * m_character_info.m_horizontalAcceleration * Time.deltaTime;
        if (horizontal >= 0)
        {
            if (horizontal > m_character_info.m_maxHorizontalSpeed)
            horizontal = m_character_info.m_maxHorizontalSpeed;
        }
        else
        {
            if (horizontal < -m_character_info.m_maxHorizontalSpeed)
                horizontal = -m_character_info.m_maxHorizontalSpeed;
        }
            
        float vertical = Input.GetAxis("Vertical_P" + playerNumber) * Time.deltaTime;
        if (vertical > 0)
        {
            vertical *= m_character_info.m_VerticalUpAcceleration;
            vertical /= m_character_info.m_gravityMultiplier;
                if (vertical > m_character_info.m_maxVerticalUpSpeed)
                    vertical = m_character_info.m_maxVerticalUpSpeed;

            
        }
        else if (vertical < 0)
        {
            vertical *= m_character_info.m_VerticalDownAcceleration;
            vertical *= m_character_info.m_gravityMultiplier;
                if (vertical < -m_character_info.m_maxVerticalDownSpeed)
                    vertical = -m_character_info.m_maxVerticalDownSpeed;
            
        }

        if (b_isInCloud)
        {
            rigidbodyPlayer.AddForce(new Vector2(horizontal, vertical) / m_character_info.m_cloudSlow, ForceMode2D.Impulse);
        }
        else
            rigidbodyPlayer.AddForce(new Vector2(horizontal, vertical), ForceMode2D.Impulse);

        //Animator
        a_Animator.SetFloat("Vertical", Mathf.Clamp(vertical,-1,1));
        a_Animator.SetFloat("Horizontal", Mathf.Clamp(horizontal,-1,1));

        Quaternion rot = transform.rotation;
        if (horizontal > 0)
        {
            m_DashParticle.GetComponent<ParticleSystemRenderer>().flip = new Vector3(1, 0, 0);
            rot.y = 180f;
        }
        else if (horizontal < 0)
        {
            m_DashParticle.GetComponent<ParticleSystemRenderer>().flip = new Vector3(0, 0, 0);
            rot.y = 0f;
        }
        transform.rotation = rot;

        
            

    }

    void Update()
    {
        string[] names = Input.GetJoystickNames();
        if (names[playerNumber - 1].Length == 19)
        {
            s_fireInput = "F1_PS4_P" + playerNumber;
            s_cancelInput = "F2_PS4_P" + playerNumber;
        }
        else if (names[playerNumber - 1].Length == 33)
        {
            s_fireInput = "F1_XBOX_P" + playerNumber;
            s_cancelInput = "F2_XBOX_P" + playerNumber;
        }
        else
        {
            s_fireInput = "F1_PC_P" + playerNumber;
            s_cancelInput = "F2_PC_P" + playerNumber;
        }

        if (!b_playerIsDashing && b_mustFall)
        {
            FallWithDelay();
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

        b_canRegenerate = true;

        if (b_playerIsDashing)
        {
            f_currentTimerDashing -= Time.deltaTime;
            if (f_currentTimerDashing <= 0)
            {
                m_DashParticle.Stop();
                b_playerIsDashing = false;
                rigidbodyPlayer.velocity *= new Vector2(0.25f,0.25f);
            }
            else
            {
                DashApplyMinimalSpeed();
            }
            b_canRegenerate = false;
        }

        if (f_currentHealth >= m_character_info.m_maxHealth || !b_canMove)
        {
            b_canRegenerate = false;
        }

        if (f_currentStun > 0)
        {
            StunCountdown(Time.deltaTime);
            b_canRegenerate = false;
        }

        if(b_isTopLimit && !b_endGame)
        {
            f_currentTimerBeforeTakeDamage -= Time.deltaTime;
            if (f_currentTimerBeforeTakeDamage <= 0)
            {
                DecreaseCloud(m_amountDamage);
                f_currentTimerBeforeTakeDamage = m_timerBeforeTakeDamage;
            }
        }

        if(Input.GetButtonUp(s_cancelInput))
        {
            f_currenttimeReduceStun = m_timeReduceStun;
            f_currenttimeReduceStun = 1f;
        }

        if (m_character_info.myChracters == Heroes.Bolt && !b_canElectrocuteCloud)
        {
            if (f_currentTimeBeforeActiveBoltPassive > 0)
            {
                f_currentTimeBeforeActiveBoltPassive -= Time.deltaTime;
                if (f_currentTimeBeforeActiveBoltPassive <= 0)
                {
                    b_canElectrocuteCloud = true;
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerController otherPlayer = collision.gameObject.GetComponent<PlayerController>();
        if(otherPlayer != null && ReturnDashState())
        {
            if (otherPlayer != this)
            {
                Vector2 currentSpeed = rigidbodyPlayer.velocity;
                if (otherPlayer.ReturnDashState())
                {
                    SetStun(f_collisionStunTime, otherPlayer.rigidbodyPlayer.velocity);
                    //RatioSpeed(new Vector2(-2, -2));
                    rigidbodyPlayer.velocity = otherPlayer.rigidbodyPlayer.velocity * new Vector2(2, 2);
                }
                if (otherPlayer.f_currentStun <= 0)
                { 
                    otherPlayer.SetStun(f_collisionStunTime, otherPlayer.rigidbodyPlayer.velocity);
                    otherPlayer.rigidbodyPlayer.velocity = currentSpeed*new Vector2(2,2);
                }

                StopDash(false);
            }
        }
    }

    public void RatioSpeed(Vector2 speedRatio)
    {
        rigidbodyPlayer.velocity *= speedRatio;
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
        f_dashCooldown = m_character_info.m_maxDashCooldown;
        rigidbodyPlayer.AddForce(new Vector2(Input.GetAxis("Horizontal_P" + playerNumber), Input.GetAxis("Vertical_P" + playerNumber)) * m_character_info.m_dashPower, ForceMode2D.Impulse);
        DecreaseCloud(m_character_info.m_dashCost);
        m_DashParticle.GetComponent<ParticleSystemRenderer>().material.SetTexture("_NewTex_1", m_DashTexture);
        m_DashParticle.Play();
    }

    public void StopDash(bool stopSpeed)
    {
        b_playerIsDashing = false;
        if(stopSpeed)
            rigidbodyPlayer.velocity = Vector2.zero;
        m_DashParticle.Stop();
    }

    public void DecreaseCloud(float f_health)
    {
        if (f_currentHealth != 0)
        {
            Debug.Log("Decrease");
            f_currentHealth -= f_health;
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
        if (f_currentHealth <= m_character_info.m_maxHealth)
        {
            f_currentHealth += f_health;
            
            if (f_currentHealth >= m_character_info.m_maxHealth)
            {
                f_currentHealth = m_character_info.m_maxHealth;
                UpdateCloudSprite();
                return;
            }
            UpdateCloudSprite();
        }
    }

    public void SetCloudHealth(float f_health)
    {
        f_currentHealth = f_health;
        Mathf.Clamp(f_currentHealth,0f, m_character_info.m_maxHealth);
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
        if (cloud)
        {
            if (f_currentHealth <= 100)
            {
                m_cloudSprite.sprite = m_cloudSpriteList[0];
                if (f_currentHealth < 100)
                {
                    m_cloudSprite.sprite = m_cloudSpriteList[1];
                    if (f_currentHealth < 75)
                    {
                        m_cloudSprite.sprite = m_cloudSpriteList[2];
                        if (f_currentHealth < 50)
                        {
                            m_cloudSprite.sprite = m_cloudSpriteList[3];
                            if (f_currentHealth < 25)
                            {
                                m_cloudSprite.sprite = m_cloudSpriteList[4];
                            }
                        }
                    }
                }
            }
            m_textHealth.text = f_currentHealth.ToString();
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

    public void PlayerIsTopScreen()
    {
        f_currentTimerBeforeTakeDamage = m_timerBeforeTakeDamage;
        b_isTopLimit = true;
    }

    public void PlayerIsNotTopScreen()
    {
        f_currentTimerBeforeTakeDamage = m_timerBeforeTakeDamage;
        b_isTopLimit = false;
    }

    public float ReturnTimeBeforeRegenerate()
    {
        return m_delayToRegenerate;
    }

    public void SetCloud(PlayerCloud Cloud)
    {
        cloud = Cloud;
    }

    public void SetStun(float stunValue,Vector3 ennemypos)
    {
        m_vfxStun.gameObject.SetActive(true);
        m_vfxStun.Play();
        f_currentStun = stunValue;
        b_canMove = false;
        b_canDash = false;
        StopDash(false);
        //rigidbodyPlayer.AddForce((ennemypos).normalized  * 20f, ForceMode2D.Impulse);
        rigidbodyPlayer.gravityScale = f_stunGravity;
    }

    public void EndStun()
    {
        m_vfxStun.gameObject.SetActive(false);
        f_currentStun = 0;
        b_canMove = true;
        b_canDash = true;
        rigidbodyPlayer.gravityScale = 0;
        rigidbodyPlayer.velocity = Vector2.zero;
    }

    public void StunCountdown(float deltaTime)
    {
        if(f_currentStun > 0)
        {
            f_currentStun -= deltaTime * f_currenttimeReduceStun;
            if(f_currentStun <= 0)
            {
                EndStun();
            }
        }
    }

    public void CountDown()
    {
        if(b_isTopLimit)
        {
            f_currentTimerBeforeTakeDamage -= Time.deltaTime;
            if(f_currentTimerBeforeTakeDamage <= 0)
            {
                Debug.Log("COUNTDOWN");
                DecreaseCloud(m_amountDamage);
                f_currentTimerBeforeTakeDamage = m_timerBeforeTakeDamage;
            }
        }
    }


    #region Powers

    [Header("Character Power")]

    [Header("Bolt")]
    private bool b_canElectrocuteCloud;
    private float f_currentTimeBeforeActiveBoltPassive;
    public float m_TimeBeforeActiveBoltPassive;

    public void DeactiveBoltPassif()
    {
        f_currentTimeBeforeActiveBoltPassive = m_TimeBeforeActiveBoltPassive;
        b_canElectrocuteCloud = false;
    }

    public bool BoltPassif()
    {
        return b_canElectrocuteCloud;
    }

    #endregion

    public void Fall()
    {
        a_Animator.SetTrigger("Fall");
        LockControls();

        if (cloud)
        {
            cloud.DestroyCloud();
        }

        b_playerIsDashing = false;
        m_DashParticle.Stop();
        GetComponent<UltraPolygonCollider2D>().Destroy();
        Falling();
    }
    public void FallWithDelay()
    {
        a_Animator.SetTrigger("Fall");
        LockControls();
        if (cloud)
        {
            cloud.DestroyCloud();
        }
        b_playerIsDashing = false;
        m_DashParticle.Stop();
        GetComponent<UltraPolygonCollider2D>().Destroy();
        Invoke("Falling", 1f);
    }

    public void Falling()
    {
        rigidbodyPlayer.gravityScale = m_gravityFall;
    }


    public void Death()
    {
        if (cloud)
            cloud.DestroyCloud();
        GameManager.Instance.PlayerDied(playerNumber);
        Destroy(gameObject);
    }

    public void GMDeath()
    {
        if (cloud)
            cloud.DestroyCloud();
        Destroy(gameObject);
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

    public void LockControlsEG()
    {
        b_endGame = true;
        b_canMove = false;
        rigidbodyPlayer.velocity = Vector3.zero;
        rigidbodyPlayer.gravityScale = 0;
    }
    public void UnlockControls()
    {
        b_canMove = true;
    }


}
