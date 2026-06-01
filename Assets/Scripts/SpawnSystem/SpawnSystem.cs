using System.Collections.Generic;
using UnityEngine;

public class SpawnSystem : MonoBehaviour
{
    public static SpawnSystem instance;

    [SerializeField] private PolygonCollider2D confinerBounds;

    [Header("Spawn Settings")]
    [SerializeField] private float totalSpawnDuration = 300f;
    [SerializeField] private float maxRetry = 50;
    [SerializeField] private List<WaveData> waves = new();

    [Header("Breakable Spawn")]
    [SerializeField] private List<GameObject> breakablePrefabs = new();
    [SerializeField] private int minBreakablePerWave = 3;
    [SerializeField] private int maxBreakablePerWave = 6;

    [Space]
    [SerializeField] private float spawnTimer;
    [SerializeField] private float nextSpawnInterval;
    [SerializeField] private float elapsedTime;

    private bool isSpawning;
    [SerializeField] private List<GameObject> aliveEnemies = new();

    private WaveData currentWave;
    private WaveData previousWave;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        StartSpawning();
    }

    private void Update()
    {
        SpawningEnemy();

        if (currentWave != null)
        {
            UI.instance.ingameUI.waveUI.SetWaveInfo(currentWave.waveName, elapsedTime, currentWave);
        }
    }

    public void StartSpawning()
    {
        elapsedTime = 0f;
        spawnTimer = 0f;
        isSpawning = true;

        UpdateCurrentWave();
        previousWave = currentWave;

        if (currentWave != null)
            SpawnBreakableObjectsAtWaveStart();

        SetNextInterval();
    }

    private void SpawningEnemy()
    {
        if (!isSpawning) return;

        elapsedTime += Time.deltaTime;
        aliveEnemies.RemoveAll(e => e == null || !e.activeSelf);

        if (elapsedTime >= totalSpawnDuration)
        {
            isSpawning = false;
            Debug.Log("Spawn phase ended!");
            return;
        }

        WaveData oldWave = currentWave;
        UpdateCurrentWave();

        if (currentWave != oldWave)
        {
            previousWave = oldWave;

            if (currentWave != null)
                SpawnBreakableObjectsAtWaveStart();
        }

        if (currentWave == null) return;

        spawnTimer += Time.deltaTime;

        if (spawnTimer >= nextSpawnInterval)
        {
            spawnTimer = 0f;
            TrySpawnEnemy();
            SetNextInterval();
        }
    }

    private void UpdateCurrentWave()
    {
        currentWave = null;

        for (int i = 0; i < waves.Count; i++)
        {
            if (elapsedTime >= waves[i].startTime && elapsedTime < waves[i].endTime)
            {
                currentWave = waves[i];
                return;
            }
        }
    }

    private void TrySpawnEnemy()
    {
        if (currentWave == null) return;
        if (aliveEnemies.Count >= currentWave.maxEnemiesAlive) return;
        if (currentWave.enemies == null || currentWave.enemies.Count == 0) return;

        Vector2 spawnPos = GetRandomPointInBounds();
        GameObject prefab = GetRandomEnemyByWeight(currentWave.enemies);
        if (prefab == null) return;

        GameObject enemy = ObjectPool.instance.Spawn(prefab.name, spawnPos, Quaternion.identity);
        if (enemy != null)
        {
            aliveEnemies.Add(enemy);
        }
    }

    private void SpawnBreakableObjectsAtWaveStart()
    {
        if (breakablePrefabs == null || breakablePrefabs.Count == 0) return;

        int spawnCount = Random.Range(minBreakablePerWave, maxBreakablePerWave + 1);

        for (int i = 0; i < spawnCount; i++)
        {
            GameObject prefab = breakablePrefabs[Random.Range(0, breakablePrefabs.Count)];
            if (prefab == null) continue;

            Vector2 spawnPos = GetRandomPointInsidePolygon();
            ObjectPool.instance.Spawn(prefab.name, spawnPos, Quaternion.identity);
        }
    }

    private void SetNextInterval()
    {
        if (currentWave == null) return;

        nextSpawnInterval = Random.Range(currentWave.minSpawnInterval, currentWave.maxSpawnInterval);
    }

    private GameObject GetRandomEnemyByWeight(List<EnemySpawnWeight> list)
    {
        int totalWeight = 0;

        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].prefab != null && list[i].weight > 0)
                totalWeight += list[i].weight;
        }

        if (totalWeight <= 0) return null;

        int randomValue = Random.Range(0, totalWeight);

        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].prefab == null || list[i].weight <= 0) continue;

            randomValue -= list[i].weight;
            if (randomValue < 0)
                return list[i].prefab;
        }

        return null;
    }

    private Vector2 GetRandomPointInBounds()
    {
        Bounds bounds = confinerBounds.bounds;
        Camera cam = Camera.main;

        Vector2 randomPoint = Vector2.zero;
        int attempts = 0;

        do
        {
            randomPoint = new Vector2(
                Random.Range(bounds.min.x, bounds.max.x),
                Random.Range(bounds.min.y, bounds.max.y)
            );

            attempts++;

            Vector3 viewportPos = cam.WorldToViewportPoint(randomPoint);
            bool outsideCamera = viewportPos.x < 0 || viewportPos.x > 1 ||
                                 viewportPos.y < 0 || viewportPos.y > 1;

            if (confinerBounds.OverlapPoint(randomPoint) && outsideCamera)
                return randomPoint;

        } while (attempts < maxRetry);

        return randomPoint;
    }

    private Vector2 GetRandomPointInsidePolygon()
    {
        Bounds bounds = confinerBounds.bounds;

        Vector2 randomPoint = Vector2.zero;
        int attempts = 0;

        do
        {
            randomPoint = new Vector2(
                Random.Range(bounds.min.x, bounds.max.x),
                Random.Range(bounds.min.y, bounds.max.y)
            );

            attempts++;

            if (confinerBounds.OverlapPoint(randomPoint))
                return randomPoint;

        } while (attempts < maxRetry);

        return randomPoint;
    }

    public int GetElapsedTime() => Mathf.FloorToInt(elapsedTime);
}