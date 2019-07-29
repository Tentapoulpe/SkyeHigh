using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Script_Player_Manager : MonoBehaviour
{
    public enum DashState { Ready, Dashing, Cooldown }
    public enum Player { Skye, Dawn, KingNimbus, CommanderBolt, Smog, Angel, Sirocco, Entity, BirdMan, DarkSkye}

    [Header("Player")]
    public Player player_list;

    [Header ("Movement")]
    private Rigidbody2D rigidbody_player;
    public float f_movement_speed;
    private float f_move_horizontal;
    private float f_move_vertical;
    private Vector2 v_movement;

    [Header("Dash")]
    public float f_dash_speed;
    private float f_current_dash_cooldown;
    public float f_dash_cooldown;
    private bool b_can_dash;
    private float f_dash_timer = 1f;
    public DashState dash_state;

    [Header("Cloud")]
    public SpriteRenderer cloud_sprite;
    private Vector3 v_current_cloud_scale;
    public int i_max_health;
    private int i_current_health;

    void Start()
    {
        i_current_health = i_max_health;
        f_current_dash_cooldown = f_dash_cooldown;
        rigidbody_player = GetComponent<Rigidbody2D>();

        switch(player_list)
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
        f_move_horizontal = Input.GetAxis("Horizontal");
        f_move_vertical = Input.GetAxis("Vertical");
        v_movement = new Vector2(f_move_horizontal, f_move_vertical);
        rigidbody_player.velocity = (v_movement * f_movement_speed);
    }

    void Update()
    {
        Movement();

        if (Input.GetKeyDown("e") && b_can_dash)
        {
            Dash();
        }

        if (Input.GetKeyDown("m"))
        {
            Script_Game_Manager.Instance.RestartGame();
        }

        #region Dash
        switch (dash_state)
            {
                case DashState.Ready:
                b_can_dash = true;
                    break;

                case DashState.Dashing:
                f_dash_timer -= Time.deltaTime;
                if(f_dash_timer <= 0)
                {
                    f_dash_timer = 1;
                    dash_state = DashState.Cooldown;
                }
                dash_state = DashState.Cooldown;
                    break;

                case DashState.Cooldown:
                f_current_dash_cooldown -= Time.deltaTime;
                    if (f_current_dash_cooldown <= 0)
                    {
                        f_current_dash_cooldown = f_dash_cooldown;
                        dash_state = DashState.Ready;
                    }
                    break;
            }
        #endregion
    }

    public void Dash()
    {
        Debug.Log("Dash");
        b_can_dash = false;
        rigidbody_player.velocity = v_movement.normalized * f_dash_speed;
        DecreaseCloud();
        dash_state = DashState.Dashing;
    }

    public void DecreaseCloud()
    {
        Debug.Log("Decrease");
        if (i_current_health != 0)
        {
            cloud_sprite.transform.localScale = cloud_sprite.transform.localScale / 2;
            i_current_health--;
        }
        else
        {
            Death();
        }
    }

    public void IncreaseCloud()
    {
        Debug.Log("Increase");
        if (i_current_health != i_max_health)
        {
            cloud_sprite.transform.localScale = cloud_sprite.transform.localScale * 2;
            i_current_health = i_max_health;
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
