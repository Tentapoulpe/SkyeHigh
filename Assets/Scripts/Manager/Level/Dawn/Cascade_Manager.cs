using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cascade_Manager : MonoBehaviour
{
    public float m_maxTimer;
    public List<GameObject> my_Spawn;
    public GameObject m_coinsVFX;
    private bool b_spawn;
    private float f_currenTimer = 0;
    private bool b_alreadySpawn;

    private void Start()
    {
        SetTimer();
    }

    private void SetTimer()
    {
        f_currenTimer = Random.RandomRange(2f, m_maxTimer);
    }

    private void Update()
    {
        Timer();
    }

    private void ActivateCascade()
    {
        int f_spawnIdx = Random.RandomRange(0, my_Spawn.Count);
        GameObject g_currentCoinsVFX = Instantiate(m_coinsVFX.gameObject, my_Spawn[f_spawnIdx].transform);
        DeleteCurrentCascade(g_currentCoinsVFX);
        b_alreadySpawn = true;
    }

    private void DeleteCurrentCascade(GameObject g_currentVFX)
    {
        Destroy(g_currentVFX, 8f);
        SetTimer();
    }

    private void Timer()
    {
        if(f_currenTimer > 0)
        {
            f_currenTimer -= Time.deltaTime;
            if(f_currenTimer <= 0)
            {
                f_currenTimer = 0;
                if (b_alreadySpawn)
                {
                    b_alreadySpawn = false;
                    SetTimer();
                }
                else if (!b_alreadySpawn)
                {
                    ActivateCascade();
                }
            }
        }
    }
}
