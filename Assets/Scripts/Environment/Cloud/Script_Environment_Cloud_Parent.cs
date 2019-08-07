using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_Environment_Cloud_Parent : MonoBehaviour
{
    private List<GameObject> m_cloudChild = new List<GameObject>();

    private void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            m_cloudChild.Add(transform.GetChild(i).gameObject);
        }
        UpdateCloudSprite();
    }

    public void UpdateCloudSprite()
    {
        for (int i = 0; i < m_cloudChild.Count; i++)
        {
            if(m_cloudChild[i].activeSelf)
            {
                m_cloudChild[i].GetComponent<Script_Environment_Cloud>().CheckUpCloud();
            }
        }
    }
}
