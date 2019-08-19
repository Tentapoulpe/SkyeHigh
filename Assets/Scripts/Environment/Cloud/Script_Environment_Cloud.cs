using System.Collections;
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
    private bool b_bonsoir;

    private void Start()
    {
        my_rigidbody = GetComponent<Rigidbody2D>();
    }

    //private void Update()
    //{
    //    RaycastHit2D hitRight = Physics2D.Raycast(gameObject.transform.position, Vector2.right, 10);
    //    Debug.DrawRay(gameObject.transform.position, Vector2.right * 10f);
    //    if (hitRight)
    //    {
    //        Debug.Log("HEY" + hitRight.collider.gameObject.name);
    //        //if (hitRight.collider.gameObject.CompareTag("Untagged"))
    //        //{
    //        //    Debug.Log("HEY");
    //        //    return;
    //        //}
    //        //else
    //        //{
    //        //    Debug.Log("TOUCHE");
    //        //}
    //    }
    //    else
    //    {
    //        Debug.Log("PASTOUCHE");
    //    }
    //}

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
        RaycastHit hit;
        for (int i = 0; i < 3; i++)
        {
            switch(i)
            {
                case 0:
                    if(Physics.Raycast(transform.position, Vector2.up, out hit, 1f))
                    {
                        if (hit.transform.gameObject.tag != "CloudEnvironment")
                        {
                            b_upCloud = true;
                        }
                    }
                    break;
                case 1:
                    if (Physics.Raycast(transform.position, -Vector2.up, out hit, 1f))
                    {
                        if (hit.transform.gameObject.tag != "CloudEnvironment")
                        {
                            b_downCloud = true;
                        }
                    }
                    break;
                case 2:
                    if (Physics.Raycast(transform.position, -Vector2.right, out hit, 1f))
                    {
                        if (hit.transform.gameObject.tag != "CloudEnvironment")
                        {
                            b_leftCloud = true;
                        }
                    }
                    break;
                case 3:
                    if (Physics.Raycast(transform.position, Vector2.right, out hit, 1f))
                    {
                        if (hit.transform.gameObject.tag != "CloudEnvironment")
                        {
                            b_rightCloud = true;
                        }
                    }
                    break;
                default:
                    break;
            }
        }
        UpdateCloud();
    }

    public void UpdateCloud()
    {
        //false = 1 true = 0
        int i_first = Convert.ToInt32(b_upCloud);
        int i_second = Convert.ToInt32(b_downCloud);
        int i_third = Convert.ToInt32(b_leftCloud);
        int i_fourth = Convert.ToInt32(b_rightCloud);

        i_amountSprite = 2 ^ 0 * i_first + 2 ^ 1 *  i_second + 2 ^ 2 * i_third + 2 ^ 3 * i_fourth;

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
