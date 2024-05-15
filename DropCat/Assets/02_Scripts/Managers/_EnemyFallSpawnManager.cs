using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _EnemyFallSpawnManager : MonoBehaviour
{
    [SerializeField] GameObject enemyBoxObject;
    [SerializeField] BoxCollider2D groundHitBox;
    [SerializeField] bool isSpawning = true;

    private void Start()
    {
        StartCoroutine(SpawnEnemyBoxRoutine());
    }

    private IEnumerator SpawnEnemyBoxRoutine()
    {
        while (isSpawning)
        {
            SpawnEnemyBox();
            yield return new WaitForSeconds(1);
        }
    }

    private void SpawnEnemyBox()
    {
        GameObject enemyBox = enemyBoxObject;
        float enemyBoxSizeX = enemyBox.GetComponent<BoxCollider2D>().bounds.extents.x;
        Vector2 spawnPoint =  GetRandomSpawnPoint(enemyBoxSizeX);

        Instantiate(enemyBox, spawnPoint, Quaternion.identity);
    }

    private Vector2 GetRandomSpawnPoint(float enemyBoxSizeX)
    {
        float minX = groundHitBox.transform.position.x - groundHitBox.bounds.extents.x + enemyBoxSizeX;
        float maxX = groundHitBox.transform.position.x + groundHitBox.bounds.extents.x - enemyBoxSizeX;
        return new Vector2(Random.Range(minX, maxX), 180);
    }
}
