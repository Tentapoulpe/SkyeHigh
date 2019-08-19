using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ParallaxBackground : MonoBehaviour
{
    [Serializable]
    public class ParallaxMap
    {
        public Transform m_Transform;
        [Range(0,1)]
        public float m_Value;
    }
    public List<ParallaxMap> m_Maps;
    private Transform m_MainCam;
    private Vector3 m_CamPos;

    private void Awake()
    {
        m_MainCam = Camera.main.transform;
        UpdateCamPos();
    }

    void LateUpdate()
    {
        UpdateParallax();
    }

    public void UpdateParallax()
    {
        foreach (ParallaxMap map in m_Maps)
        {
            map.m_Transform.position += ((m_MainCam.position - m_CamPos) * map.m_Value);
        }

        UpdateCamPos();
    }

    public void UpdateCamPos()
    {
        m_CamPos = m_MainCam.position;
    }
}
