using System.Collections;
using UnityEngine;

public class SpawnerScript : MonoBehaviour
{
    public Vector2[] spawnpoints;
    public GameObject enemyPrefab;
    void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    void Update()
    {

    }

    IEnumerator SpawnEnemies()
    {
        while (true)
        {
            float randomTime = Random.Range(3f, 10f);
            yield return new WaitForSeconds(randomTime);
            Vector2 spawnPoint = spawnpoints[Random.Range(0, spawnpoints.Length)];
            Instantiate(enemyPrefab, spawnPoint, Quaternion.identity);
        }
    }

}
