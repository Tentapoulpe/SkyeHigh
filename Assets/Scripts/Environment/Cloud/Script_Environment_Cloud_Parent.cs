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
    private bool b_goRight;
    private float f_cloudVelocity;

    private void Start()
    {
        my_rigidbody = GetComponent<Rigidbody2D>();
        f_cloudVelocity = Random.Range(1f, 5f);
        CreateCloudParent();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("LeftBorder"))
        {
            b_goRight = true;
        }
        else
            b_goRight = false;

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("LeftBorder") && !b_goRight)
        {
            Destroy();
        }
        if( collision.CompareTag("RightBorder") && b_goRight)
        {
            Destroy();
        }
    }

    public void UpdateCloudSprite()
    {
        for (int i = 0; i < g_cloudChild.Count; i++)
        {
            if(g_cloudChild.Count == 0)
            {
                Destroy();
            }
            else if(g_cloudChild[i].activeSelf)
            {
                g_cloudChild[i].GetComponent<Script_Environment_Cloud>().CheckUpCloud();
            }
        }
    }

    private void Destroy()
    {
        GameManager.Instance.UnSpawnCloudParent();
        Destroy(this.gameObject);
    }

    public void CreateCloudParent()
    {
        int i_numberOfCloud = Random.Range(2,m_numberOfCloudMax);

        for (int i = 0; i < i_numberOfCloud; i++)
        {
            if (i == 0)
            {
                myCloud = Instantiate(m_prefabCloud, m_spawnPoint.transform);
            }
            else if(i > 0)
            {
                RaycastHit hit;
                int i_myDirection = Random.Range(0, 3);
                switch (i_myDirection)
                {
                    case 0:
                        if(Physics.Raycast(g_cloudChild[i - 1].transform.position, Vector2.up, out hit, 1f))
                        {
                            if (hit.transform.gameObject.tag != "CloudEnvironment")
                            {
                                Debug.Log("HEY");
                                return;
                            }
                        }
                        else
                        {
                            myCloud = Instantiate(m_prefabCloud, m_spawnPoint.transform.position + Vector3.up, Quaternion.identity,gameObject.transform);
                        }
                        break;
                    case 1:
                        if (Physics.Raycast(g_cloudChild[i - 1].transform.position, -Vector2.up, out hit, 1f))
                        {
                            if (hit.transform.gameObject.tag != "CloudEnvironment")
                            {
                                Debug.Log("HEY");
                                return;
                            }
                        }
                        else
                        {
                            myCloud = Instantiate(m_prefabCloud, m_spawnPoint.transform.position + -Vector3.up, Quaternion.identity, gameObject.transform);
                        }
                        break;
                    case 2:
                        if (Physics.Raycast(g_cloudChild[i - 1].transform.position, -Vector2.right, out hit, 1f))
                        {
                            if (hit.transform.gameObject.tag != "CloudEnvironment")
                            {
                                Debug.Log("HEY");
                                return;
                            }
                        }
                        else
                        {
                            myCloud = Instantiate(m_prefabCloud, m_spawnPoint.transform.position + -Vector3.right, Quaternion.identity, gameObject.transform);
                        }
                        break;
                    case 3:
                        if (Physics.Raycast(g_cloudChild[i - 1].transform.position, Vector2.right, out hit, 1f))
                        {
                            if (hit.transform.gameObject.tag != "CloudEnvironment")
                            {
                                Debug.Log("HEY");
                                return;
                            }
                        }
                        else
                        {
                            myCloud = Instantiate(m_prefabCloud, m_spawnPoint.transform.position + Vector3.right, Quaternion.identity, gameObject.transform);
                        }
                        break;
                    default:
                        break;
                }
            }
            g_cloudChild.Add(myCloud);
            g_cloudChild[i].gameObject.tag = "CloudEnvironment";
            m_spawnPoint = myCloud;
        }
        //UpdateCloudSprite();
    }



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
