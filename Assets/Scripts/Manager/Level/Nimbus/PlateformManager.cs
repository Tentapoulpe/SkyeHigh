using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateformManager : MonoBehaviour
{
    public List<GameObject> m_Plateforms;
    public float m_MoveTimer;
    private float f_Timer;
    private List<Quaternion> q_FinalRot = new List<Quaternion>();
    bool b_NeedToRotate = false;

    private void Start()
    {
        f_Timer = m_MoveTimer;
        foreach (GameObject item in m_Plateforms)
        {
            q_FinalRot.Add(Quaternion.identity);
        }
    }

    private void Update()
    {
        if (f_Timer > 0f)
        {
            f_Timer -= Time.deltaTime;
            if (f_Timer <= 0f)
            {
                RotatePlateform();
            }
        }

        if (b_NeedToRotate)
        {
            for (int i = 0; i < m_Plateforms.Count; i++)
            {
                m_Plateforms[i].transform.rotation = Quaternion.Lerp(m_Plateforms[i].transform.rotation, q_FinalRot[i], 4f * Time.deltaTime);
            }
            if (m_Plateforms[0].transform.rotation.eulerAngles.z >= q_FinalRot[0].eulerAngles.z -1 && m_Plateforms[0].transform.rotation.eulerAngles.z <= 270f)
            {
                for (int i = 0; i < m_Plateforms.Count; i++)
                {
                    m_Plateforms[i].transform.rotation = q_FinalRot[i];
                }
                b_NeedToRotate = false;
                f_Timer = m_MoveTimer;
            }
        }
    }

    public void RotatePlateform()
    {
        for (int i = 0; i < m_Plateforms.Count; i++)
        {
            q_FinalRot[i] = m_Plateforms[i].transform.rotation * Quaternion.Euler(0f, 0f, 90f);
            q_FinalRot[i] = Quaternion.Euler(q_FinalRot[i].eulerAngles.x, q_FinalRot[i].eulerAngles.y, Mathf.Round(q_FinalRot[i].eulerAngles.z));
        }
        b_NeedToRotate = true;
    }
}
