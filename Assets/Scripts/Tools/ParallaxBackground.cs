using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    public class ParallaxMap
    {
        public Transform m_Transform;
        public float m_Value;
    }
    public List<ParallaxMap> m_Maps;
    private Transform m_MainCam;
    private Vector3 m_CamPos;

    private void Awake()
    {
        m_MainCam = Camera.main.transform;
        m_CamPos = m_MainCam.position;
    }

    void Update()
    {
        UpdateParallax();
    }

    public void UpdateParallax()
    {

    }
}
