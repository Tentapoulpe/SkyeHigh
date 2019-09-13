using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoKiller : MonoBehaviour
{
    public float m_Timer;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("Death", m_Timer);
    }

    public void Death()
    {
        Destroy(gameObject);
    }

    
}
