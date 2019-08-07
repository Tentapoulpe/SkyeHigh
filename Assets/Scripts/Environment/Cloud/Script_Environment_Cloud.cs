using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Script_Environment_Cloud : MonoBehaviour
{
    public class CloudDirection
    {
        public bool b_upCloud;//0
        public bool b_downCloud;//1
        public bool b_leftCloud;//2
        public bool b_rightCloud;//3
    }

    private Rigidbody2D my_rigidbody;
    public float f_healthToRegenerate;
    private int i_cloudIdx;
    public List<SpriteRenderer> m_spriteList = new List<SpriteRenderer>();
    private RaycastHit2D hit;
    private CloudDirection my_cloudDirection;
    private int i_amountSprite;

    private void Start()
    {
        my_rigidbody = GetComponent<Rigidbody2D>();    
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (collision.GetComponentInParent<PlayerController>().CanRegenerate() == true)
            {
                collision.GetComponentInParent<PlayerController>().IncreaseCloud(f_healthToRegenerate);
                GetComponentInParent<Script_Environment_Cloud_Parent>().UpdateCloudSprite();
                Destroy(this);
            }
            else
            {
                collision.gameObject.GetComponentInParent<PlayerController>().CloudUnSlow();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("InCloud");
            collision.gameObject.GetComponentInParent<PlayerController>().CloudSlow();
        }
    }

    public void CheckUpCloud()
    {
        for(int i = 0; i < 3; i++)
        {
            switch(i)
            {
                case 0:
                    hit = Physics2D.Raycast(transform.position, Vector2.up, 1f);
                    break;
                case 1:
                    hit = Physics2D.Raycast(transform.position, -Vector2.up, 1f);
                    break;
                case 2:
                    hit = Physics2D.Raycast(transform.position, -Vector2.right, 1f);
                    break;
                case 3:
                    hit = Physics2D.Raycast(transform.position, Vector2.right, 1f);
                    break;
                default:
                    break;
            }

            if(hit.collider != null && hit.collider.tag != "CloudEnvironment")
            {
                switch(i)
                {
                    case 0:
                        my_cloudDirection.b_upCloud = true;
                        break;
                    case 1:
                        my_cloudDirection.b_downCloud = true;
                        break;
                    case 2:
                        my_cloudDirection.b_leftCloud = true;
                        break;
                    case 3:
                        my_cloudDirection.b_rightCloud = true;
                        break;
                    default:
                        break;
                }
            }
        }
        UpdateCloud();
    }

    public void UpdateCloud()
    {
        //false = 1 true = 0
        int i_first = Convert.ToInt32(my_cloudDirection.b_upCloud);
        int i_second = Convert.ToInt32(my_cloudDirection.b_downCloud);
        int i_third = Convert.ToInt32(my_cloudDirection.b_leftCloud);
        int i_fourth = Convert.ToInt32(my_cloudDirection.b_rightCloud);

        i_amountSprite = 2 ^ i_first + 2 ^ i_second + 2 ^ i_third + 2 ^ i_fourth;

        switch(i_amountSprite)
        {
            case 0:
                gameObject.GetComponent<SpriteRenderer>().sprite = m_spriteList[0].sprite;
                break;
            case 1:
                gameObject.GetComponent<SpriteRenderer>().sprite = m_spriteList[1].sprite;
                break;
            case 2:
                gameObject.GetComponent<SpriteRenderer>().sprite = m_spriteList[2].sprite;
                break;
            case 3:
                gameObject.GetComponent<SpriteRenderer>().sprite = m_spriteList[3].sprite;
                break;
            case 4:
                gameObject.GetComponent<SpriteRenderer>().sprite = m_spriteList[4].sprite;
                break;
            case 5:
                gameObject.GetComponent<SpriteRenderer>().sprite = m_spriteList[5].sprite;
                break;
            case 6:
                gameObject.GetComponent<SpriteRenderer>().sprite = m_spriteList[6].sprite;
                break;
            case 7:
                gameObject.GetComponent<SpriteRenderer>().sprite = m_spriteList[7].sprite;
                break;
            case 8:
                gameObject.GetComponent<SpriteRenderer>().sprite = m_spriteList[8].sprite;
                break;
            case 9:
                gameObject.GetComponent<SpriteRenderer>().sprite = m_spriteList[9].sprite;
                break;
            case 10:
                gameObject.GetComponent<SpriteRenderer>().sprite = m_spriteList[10].sprite;
                break;
            case 11:
                gameObject.GetComponent<SpriteRenderer>().sprite = m_spriteList[11].sprite;
                break;
            case 12:
                gameObject.GetComponent<SpriteRenderer>().sprite = m_spriteList[12].sprite;
                break;
            case 13:
                gameObject.GetComponent<SpriteRenderer>().sprite = m_spriteList[13].sprite;
                break;
            case 14:
                gameObject.GetComponent<SpriteRenderer>().sprite = m_spriteList[14].sprite;
                break;
            case 15:
                gameObject.GetComponent<SpriteRenderer>().sprite = m_spriteList[15].sprite;
                break;
            default:
                break;
        }

    }
}
