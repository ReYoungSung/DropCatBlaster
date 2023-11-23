using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private ObjectSpawnManager spawnManager;

    private void Awake()
    {
        spawnManager = GameObject.Find("[ObjectSpawnManager]").GetComponent<ObjectSpawnManager>();
    }
}
