﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Script_Environment_Cloud : MonoBehaviour
{
    [Header("CloudDirection")]
    private bool b_upCloud;//0
    private bool b_downCloud;//1
    private bool b_leftCloud;//2
    private bool b_rightCloud;//3
    [Space]

    private Rigidbody2D my_rigidbody;
    public float f_healthToRegenerate;
    private int i_cloudIdx;
    public List<SpriteRenderer> m_spriteList = new List<SpriteRenderer>();
    private RaycastHit2D hit;
    private int i_amountSprite;
    public List<Script_Environment_Cloud> m_cloudNeighbor;

    private bool b_isElectrocuted;
    private bool b_canBeElectrocute;
    public float m_TimeBeforeElectrocute;
    private float f_currentTimeBeforeElectrocute;

    private void Start()
    {
        my_rigidbody = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        PlayerController player = collision.GetComponentInParent<PlayerController>();

        if (player)
        {
            if (player.CanRegenerate() == true)
            {
                player.IncreaseCloud(f_healthToRegenerate);
                //GetComponentInParent<Script_Environment_Cloud_Parent>().UpdateCloudSprite();
                Destroy(gameObject);
            }

            if(player.m_character_info.myChracters != Heroes.Nimbus)
            {
                player.CloudUnSlow();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController player = collision.GetComponentInParent<PlayerController>();

        if (player && player.m_character_info.myChracters != Heroes.Nimbus)
        {
            if (b_isElectrocuted)
            {
                player.Fall();
            }
            Debug.Log("InCloud");
            player.CloudSlow();
        }

        if (player && player.m_character_info.myChracters == Heroes.Bolt && player.BoltPassif())
        {
            ActivateThunder();
            player.DeactiveBoltPassif();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        PlayerController player = collision.GetComponentInParent<PlayerController>();

        if(player && b_isElectrocuted)
        {
            player.Fall();
        }
    }

    public void ActivateThunder()
    {
        foreach (Script_Environment_Cloud cloud in m_cloudNeighbor)
        {
            if (cloud.enabled && !b_isElectrocuted)
            {
                cloud.ActivateThunder();
                cloud.Thunder();
            }
        }
    }

    public void Thunder()
    {
        if (b_canBeElectrocute)
        {
            b_isElectrocuted = true;
            b_canBeElectrocute = false;
        }
    }

    private void CountDownElectrocute()
    {
        if (f_currentTimeBeforeElectrocute > 0)
        {
            f_currentTimeBeforeElectrocute -= Time.deltaTime;

            if(f_currentTimeBeforeElectrocute <= 0)
            {
                b_canBeElectrocute = true;
                f_currentTimeBeforeElectrocute = m_TimeBeforeElectrocute;
            }
        }
    }

    private void Update()
    {
        if(!b_canBeElectrocute)
        {
            CountDownElectrocute();
        }
    }

    //public void CheckUpCloud()
    //{
    //    RaycastHit hit;
    //    for (int i = 0; i < 3; i++)
    //    {
    //        switch(i)
    //        {
    //            case 0:
    //                if(Physics.Raycast(transform.position, Vector2.up, out hit, 1f))
    //                {
    //                    if (hit.transform.gameObject.tag != "CloudEnvironment")
    //                    {
    //                        b_upCloud = true;
    //                    }
    //                }
    //                break;
    //            case 1:
    //                if (Physics.Raycast(transform.position, -Vector2.up, out hit, 1f))
    //                {
    //                    if (hit.transform.gameObject.tag != "CloudEnvironment")
    //                    {
    //                        b_downCloud = true;
    //                    }
    //                }
    //                break;
    //            case 2:
    //                if (Physics.Raycast(transform.position, -Vector2.right, out hit, 1f))
    //                {
    //                    if (hit.transform.gameObject.tag != "CloudEnvironment")
    //                    {
    //                        b_leftCloud = true;
    //                    }
    //                }
    //                break;
    //            case 3:
    //                if (Physics.Raycast(transform.position, Vector2.right, out hit, 1f))
    //                {
    //                    if (hit.transform.gameObject.tag != "CloudEnvironment")
    //                    {
    //                        b_rightCloud = true;
    //                    }
    //                }
    //                break;
    //            default:
    //                break;
    //        }
    //    }
    //    UpdateCloud();
    //}

    //public void UpdateCloud()
    //{
    //    //false = 1 true = 0
    //    int i_first = Convert.ToInt32(b_upCloud);
    //    int i_second = Convert.ToInt32(b_downCloud);
    //    int i_third = Convert.ToInt32(b_leftCloud);
    //    int i_fourth = Convert.ToInt32(b_rightCloud);

    //    i_amountSprite = 2 ^ 0 * i_first + 2 ^ 1 *  i_second + 2 ^ 2 * i_third + 2 ^ 3 * i_fourth;

    //    switch(i_amountSprite)
    //    {
    //        case 0:
    //            gameObject.GetComponent<SpriteRenderer>().sprite = m_spriteList[0].sprite;
    //            break;
    //        case 1:
    //            gameObject.GetComponent<SpriteRenderer>().sprite = m_spriteList[1].sprite;
    //            break;
    //        case 2:
    //            gameObject.GetComponent<SpriteRenderer>().sprite = m_spriteList[2].sprite;
    //            break;
    //        case 3:
    //            gameObject.GetComponent<SpriteRenderer>().sprite = m_spriteList[3].sprite;
    //            break;
    //        case 4:
    //            gameObject.GetComponent<SpriteRenderer>().sprite = m_spriteList[4].sprite;
    //            break;
    //        case 5:
    //            gameObject.GetComponent<SpriteRenderer>().sprite = m_spriteList[5].sprite;
    //            break;
    //        case 6:
    //            gameObject.GetComponent<SpriteRenderer>().sprite = m_spriteList[6].sprite;
    //            break;
    //        case 7:
    //            gameObject.GetComponent<SpriteRenderer>().sprite = m_spriteList[7].sprite;
    //            break;
    //        case 8:
    //            gameObject.GetComponent<SpriteRenderer>().sprite = m_spriteList[8].sprite;
    //            break;
    //        case 9:
    //            gameObject.GetComponent<SpriteRenderer>().sprite = m_spriteList[9].sprite;
    //            break;
    //        case 10:
    //            gameObject.GetComponent<SpriteRenderer>().sprite = m_spriteList[10].sprite;
    //            break;
    //        case 11:
    //            gameObject.GetComponent<SpriteRenderer>().sprite = m_spriteList[11].sprite;
    //            break;
    //        case 12:
    //            gameObject.GetComponent<SpriteRenderer>().sprite = m_spriteList[12].sprite;
    //            break;
    //        case 13:
    //            gameObject.GetComponent<SpriteRenderer>().sprite = m_spriteList[13].sprite;
    //            break;
    //        case 14:
    //            gameObject.GetComponent<SpriteRenderer>().sprite = m_spriteList[14].sprite;
    //            break;
    //        case 15:
    //            gameObject.GetComponent<SpriteRenderer>().sprite = m_spriteList[15].sprite;
    //            break;
    //        default:
    //            break;
    //    }
    //}
}
