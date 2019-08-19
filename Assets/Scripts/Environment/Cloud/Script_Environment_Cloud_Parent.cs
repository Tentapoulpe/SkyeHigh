using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_Environment_Cloud_Parent : MonoBehaviour
{
    public int m_numberOfCloudMax = 10;
    public GameObject m_prefabCloud;
    public GameObject m_spawnPoint;
    public List<GameObject> g_cloudChild = new List<GameObject>();
    private Rigidbody2D my_rigidbody;
    private GameObject myCloud;
    private bool b_getDirection;
    private bool b_goRight;
    private bool b_wantToBeDestroy = true;
    private float f_cloudVelocity;

    private void Start()
    {
        my_rigidbody = GetComponent<Rigidbody2D>();
        f_cloudVelocity = Random.Range(1f, 3f);
        //CreateCloudParent();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!b_getDirection)
        {
            if (collision.CompareTag("LeftBorder"))
            {
                Debug.Log("goright");
                b_goRight = true;
            }
            else
            {
                Debug.Log("goleft");
                b_goRight = false;
            }

            b_getDirection = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("LeftBorder") && !b_goRight)
        {
            Destroy();
        }
        else if(collision.CompareTag("RightBorder") && b_goRight)
        {
            Destroy();
        }
    }

    //public void UpdateCloudSprite()
    //{
    //    for (int i = 0; i < g_cloudChild.Count; i++)
    //    {
    //        if(g_cloudChild.Count == 0)
    //        {
    //            Destroy();
    //        }
    //        else if(g_cloudChild[i].activeSelf)
    //        {
    //            g_cloudChild[i].GetComponent<Script_Environment_Cloud>().CheckUpCloud();
    //        }
    //    }
    //}

    private void Destroy()
    {
        if (b_wantToBeDestroy)
        {
            GameManager.Instance.UnSpawnCloudParent(gameObject);
            Destroy(this.gameObject);
            b_wantToBeDestroy = false;
        }
    }

    //public void CreateCloudParent()
    //{
    //    int i_numberOfCloud = Random.Range(2,m_numberOfCloudMax);
    //    myCloud = m_spawnPoint;

    //    for (int i = 0; i < i_numberOfCloud; i++)
    //    {
    //        int i_myDirection = Random.Range(0, 3);
    //        float f_cloudScale = Random.Range(0.25f, 0.5f);
    //        switch (i_myDirection)
    //        {
    //                case 0:
    //                RaycastHit2D hitCornerUpLeft = Physics2D.Raycast(myCloud.transform.position, new Vector2(-1,1), 10f);
    //                    if (hitCornerUpLeft)
    //                    {
    //                        if (hitCornerUpLeft.collider.CompareTag("CloudEnvironment"))
    //                        {
    //                            return;
    //                        }
    //                        else
    //                        {
    //                            myCloud = Instantiate(m_prefabCloud, m_spawnPoint.transform.position + new Vector3(-1,1,1), Quaternion.identity, gameObject.transform);
    //                            myCloud.transform.localScale = new Vector3(f_cloudScale, f_cloudScale, f_cloudScale);
    //                        }
    //                    }
    //                    else
    //                    {
    //                        myCloud = Instantiate(m_prefabCloud, m_spawnPoint.transform.position + new Vector3(-1, 1, 1), Quaternion.identity,gameObject.transform);
    //                        myCloud.transform.localScale = new Vector3(f_cloudScale, f_cloudScale, f_cloudScale);
    //                    }
    //                    break;
    //                case 1:
    //                    RaycastHit2D hitCornerUpRight = Physics2D.Raycast(myCloud.transform.position, new Vector2(1,1), 10f);
    //                    if (hitCornerUpRight)
    //                    {
    //                        if (hitCornerUpRight.collider.CompareTag("CloudEnvironment"))
    //                        {
    //                            return;
    //                        }
    //                        else
    //                        {
    //                            myCloud = Instantiate(m_prefabCloud, m_spawnPoint.transform.position + new Vector3(1,1,1), Quaternion.identity, gameObject.transform);
    //                            myCloud.transform.localScale = new Vector3(f_cloudScale, f_cloudScale, f_cloudScale);
    //                        }
    //                    }
    //                    else
    //                    {
    //                        myCloud = Instantiate(m_prefabCloud, m_spawnPoint.transform.position + new Vector3(1, 1, 1), Quaternion.identity, gameObject.transform);
    //                        myCloud.transform.localScale = new Vector3(f_cloudScale, f_cloudScale, f_cloudScale);
    //                    }
    //                    break;
    //                case 2:
    //                    RaycastHit2D hitLeft = Physics2D.Raycast(myCloud.transform.position, -Vector2.right, 10f);
    //                    if (hitLeft)
    //                    {
    //                        if (hitLeft.collider.CompareTag("CloudEnvironment"))
    //                        {
    //                            return;
    //                        }
    //                        else
    //                        {
    //                            myCloud = Instantiate(m_prefabCloud, m_spawnPoint.transform.position + -Vector3.right, Quaternion.identity, gameObject.transform);
    //                            myCloud.transform.localScale = new Vector3(f_cloudScale, f_cloudScale, f_cloudScale);
    //                        }
    //                    }
    //                    else
    //                    {
    //                        myCloud = Instantiate(m_prefabCloud, m_spawnPoint.transform.position + -Vector3.right, Quaternion.identity, gameObject.transform);
    //                        myCloud.transform.localScale = new Vector3(f_cloudScale, f_cloudScale, f_cloudScale);
    //                    }
    //                    break;
    //                case 3:
    //                    RaycastHit2D hitRight = Physics2D.Raycast(myCloud.transform.position, Vector2.right, 10f);
    //                    if (hitRight)
    //                    {
    //                        if (hitRight.collider.CompareTag("CloudEnvironment"))
    //                        {
    //                            return;
    //                        }
    //                        else
    //                        {
    //                            myCloud = Instantiate(m_prefabCloud, m_spawnPoint.transform.position + Vector3.right, Quaternion.identity, gameObject.transform);
    //                            myCloud.transform.localScale = new Vector3(f_cloudScale, f_cloudScale, f_cloudScale);
    //                        }
    //                    }
    //                    else
    //                    {
    //                        myCloud = Instantiate(m_prefabCloud, m_spawnPoint.transform.position + Vector3.right, Quaternion.identity, gameObject.transform);
    //                        myCloud.transform.localScale = new Vector3(f_cloudScale, f_cloudScale, f_cloudScale);
    //                    }
    //                    break;
    //                default:
    //                    break;
    //        }
    //        g_cloudChild.Add(myCloud);
    //        g_cloudChild[i].gameObject.tag = "CloudEnvironment";
    //        m_spawnPoint = myCloud;
    //    }
    //    //UpdateCloudSprite();
    //}



    private void Update()
    {
        if(b_goRight)
        {
            my_rigidbody.velocity = new Vector2(f_cloudVelocity,0);
        }
        else
            my_rigidbody.velocity = new Vector2(-f_cloudVelocity, 0);
    }
}
