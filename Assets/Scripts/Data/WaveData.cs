using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WaveData
{
    public bool isBossWave;
    public bool isRewardWave;

    [Space]
    public string waveName;
    public float startTime;
    public float endTime = 30f;
    public float minSpawnInterval = 2f;
    public float maxSpawnInterval = 5f;
    public int maxEnemiesAlive = 10;
    public List<EnemySpawnWeight> enemies = new();

    [Header("Boss Settings")]
    public GameObject bossPrefab;
    public bool spawnMinionWithBoss;
}

[Serializable]
public class EnemySpawnWeight
{
    public GameObject prefab;
    public int weight = 1;
}
