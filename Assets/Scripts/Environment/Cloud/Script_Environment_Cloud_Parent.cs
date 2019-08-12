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
    private RaycastHit2D hit = new RaycastHit2D();
    private GameObject myCloud;

    private void Start()
    {
        my_rigidbody = GetComponent<Rigidbody2D>();
        hit = Physics2D.Raycast(transform.position, Vector2.up, 1f);
        if (hit.transform.gameObject.tag == "Untagged")
        {
            Debug.Log("UNITY C DE LA MERDE");
        }
        else
            Debug.Log("bonsoir");
        //CreateCloudParent();
    }

    public void UpdateCloudSprite()
    {
        for (int i = 0; i < g_cloudChild.Count; i++)
        {
            if(g_cloudChild.Count == 0)
            {
                Destroy(this.gameObject);
            }
            else if(g_cloudChild[i].activeSelf)
            {
                g_cloudChild[i].GetComponent<Script_Environment_Cloud>().CheckUpCloud();
            }
        }
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
                int i_myDirection = Random.Range(0, 3);
                switch (i_myDirection)
                {
                    case 0:
                        hit = Physics2D.Raycast(g_cloudChild[i - 1].transform.position, Vector2.up, 1f);
                        if (hit.transform.gameObject.tag != "CloudEnvironment")
                        {
                            myCloud = Instantiate(myCloud, m_spawnPoint.transform.position + Vector3.up, Quaternion.identity);
                        }
                        break;
                    case 1:
                        hit = Physics2D.Raycast(g_cloudChild[i-1].transform.position, -Vector2.up, 1f);
                        if (hit.transform.gameObject.tag != "CloudEnvironment")
                        {
                            myCloud = Instantiate(myCloud, m_spawnPoint.transform.position + -Vector3.up, Quaternion.identity);
                        }
                        break;
                    case 2:
                        hit = Physics2D.Raycast(g_cloudChild[i-1].transform.position, -Vector2.right, 1f);
                        if (hit.transform.gameObject.tag != "CloudEnvironment")
                        {
                            myCloud = Instantiate(myCloud, m_spawnPoint.transform.position + -Vector3.right, Quaternion.identity);
                        }
                        break;
                    case 3:
                        hit = Physics2D.Raycast(g_cloudChild[i-1].transform.position, Vector2.right, 1f);
                        if (hit.transform.gameObject.tag != "CloudEnvironment")
                        {
                            myCloud = Instantiate(myCloud, m_spawnPoint.transform.position + Vector3.right, Quaternion.identity);
                        }
                        break;
                    default:
                        break;
                }
            }
            Debug.Log("add cloud");
            g_cloudChild.Add(myCloud);
            g_cloudChild[i].gameObject.tag = "CloudEnvironment";
            m_spawnPoint = myCloud;
        }
    }



    private void Update()
    {
        float my_velocity = Random.Range(0.1f, 1f);
        //int my_Spawn = Random,Range(1, 
        //if()
        //my_rigidbody.velocity()   
    }
}
