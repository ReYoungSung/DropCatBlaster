using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private ObjectSpawnManager enemySpawner;

    private bool spawningActivated = false;

    private void Awake()
    {
        enemySpawner = GameObject.Find("[ObjectSpawnManager]").GetComponent<ObjectSpawnManager>();
    }

    private void Update()
    {
        if(spawningActivated)
        {
            enemySpawner.onSpawningTriggerEnter();
            spawningActivated = true;
        }
        else
        {
            enemySpawner.onSpawningTriggerExit();
            spawningActivated = false;
        }
    }

    public bool SwitchSpawning()
    { 
        if(spawningActivated)
        {
            spawningActivated = false;
        }
        else
        {
            spawningActivated = true;
        }
        return spawningActivated;
    }
}
