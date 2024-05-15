/*
using System;
using System.Collections.Generic;
using UnityEngine;

interface IEnemyBehaviour
{
    void NoticeObjectKilled();
}

public class EnemyController : MonoBehaviour
{
    private HashSet<GameObject> SpawnedCatHouse;
    private Dictionary<string, HashSet<GameObject>> spawnedEnemyDict;

    public HashSet<GameObject> this[string enemyTag]
    {
        get { return spawnedEnemyDict[enemyTag]; }
    }
    [SerializeField] private ParticleSystem explosion;

    private void Awake()
    {
        SpawnedCatHouse = new HashSet<GameObject>();
        GameObject[] existingCatHouses = GameObject.FindGameObjectsWithTag("ENEMY_CatHouse");
        for (int i = 0; i < existingCatHouses.Length; ++i)
        {
            SpawnedCatHouse.Add(existingCatHouses[i]);
        }

        spawnedEnemyDict = new Dictionary<string, HashSet<GameObject>>
        {
            { "ENEMY_CatHouse", SpawnedCatHouse }
        };
    }


    // PlayerBasicAttack ฐ๚ ม฿บน ตส
    public void ActivateHitParticles(Vector2 hitSpot)
    {
        GameObject explosionParticle = Instantiate(explosion.gameObject, hitSpot, Quaternion.identity);
        Destroy(explosionParticle.gameObject, 5f);
    }
}
*/