using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    public Vector2 spawnOffset;

    [SerializeField]
    public float spawnChance = 0.01f;

    [SerializeField]
    public float maxSpawnChance = 0.1f;

    [SerializeField]
    public float spawnRamp = 0.01f;

    [SerializeField]
    public float spawnRampSteps = 10f;

    [SerializeField]
    public float nextSpawnRamp = 1f;

    [SerializeField]
    public Enemy enemyPrefab;

    [SerializeField]
    public PlayerController player;

    public Vector2 RandomizeLocation()
    {
        Vector2 spawnLocation = transform.position;
        int selection = Random.Range(0, 4);
        switch (selection)
        {
            default: case 0:
                spawnLocation.x += Random.Range(-spawnOffset.x, spawnOffset.x);
                spawnLocation.y += spawnOffset.y;
                break;
            case 1:
                spawnLocation.x += spawnOffset.x;
                spawnLocation.y += Random.Range(-spawnOffset.y, spawnOffset.y);
                break;
            case 2:
                spawnLocation.x += Random.Range(-spawnOffset.x, spawnOffset.x);
                spawnLocation.y += -spawnOffset.y;
                break;
            case 3:
                spawnLocation.x += -spawnOffset.x;
                spawnLocation.y += Random.Range(-spawnOffset.y, spawnOffset.y);
                break;
        }
        return spawnLocation;
    }

    private void FixedUpdate()
    {
        if (GameManager.Instance.Paused)
            return;

        TrySpawn();
        UpdateSpawnRamp();
    }

    public void TrySpawn()
    {
        if (Random.Range(0f, 1) < spawnChance)
        {
            Enemy enemy = Instantiate(enemyPrefab, RandomizeLocation(), Quaternion.identity);
            enemy.target = player;
        }
    }

    public void UpdateSpawnRamp()
    {
        if (Time.fixedTime > nextSpawnRamp && spawnChance < maxSpawnChance)
        {
            nextSpawnRamp = Time.fixedTime + spawnRampSteps;
            spawnChance += spawnRamp;
        }
    }
}
