using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLauncher : MonoBehaviour
{
    public List<GameObject> CloudSpawn;
    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject item in CloudSpawn)
        {

            GameManager.Instance.m_spawnPointCloud.Add(item);
        }
        GameManager.Instance.StartGame();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
