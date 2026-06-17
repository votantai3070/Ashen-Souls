using System.Collections.Generic;
using UnityEngine;

public class SpawnSystem : MonoBehaviour
{
    public static SpawnSystem instance;

    [SerializeField] private PolygonCollider2D confinerBounds;

    [Header("Spawn Settings")]
    [SerializeField] private float totalSpawnDuration = 300f;
    [SerializeField] private int maxRetry = 50;
    [SerializeField] private List<WaveData> waves = new();

    [Header("Enemy Spawn Check")]
    [SerializeField] private float enemySpawnCheckRadius = 0.8f;
    [SerializeField] private LayerMask blockedSpawnLayer; // Obstacle + Breakable (+ Wall nếu cần)

    [Header("Breakable Spawn")]
    [SerializeField] private List<GameObject> breakablePrefabs = new();
    [SerializeField] private int minBreakablePerWave = 3;
    [SerializeField] private int maxBreakablePerWave = 6;
    [SerializeField] private float breakableSpawnCheckRadius = 0.8f;

    [Space]
    [SerializeField] private float spawnTimer;
    [SerializeField] private float nextSpawnInterval;
    [SerializeField] private float elapsedTime;

    private bool isSpawning;
    [SerializeField] private List<GameObject> aliveEnemies = new();

    private WaveData currentWave;
    private WaveData previousWave;
    private bool hasEndedGame;

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

            if (!hasEndedGame && currentWave.isRewardWave && elapsedTime >= currentWave.endTime)
            {
                hasEndedGame = true;
                UI.instance.EndGame();
            }
        }
    }

    public void StartSpawning()
    {
        elapsedTime = 0f;
        spawnTimer = 0f;
        isSpawning = true;
        hasEndedGame = false;

        UpdateCurrentWave();

        if (currentWave != null)
        {
            SpawnBreakableAndObstacleObjectsAtWaveStart();
        }

        SetNextInterval();
    }

    private void SpawningEnemy()
    {
        elapsedTime += Time.deltaTime;
        if (!isSpawning) return;

        aliveEnemies.RemoveAll(e => e == null || !e.activeSelf);

        if (elapsedTime >= totalSpawnDuration)
        {
            isSpawning = false;
            Debug.Log("Spawn phase ended!");
            return;
        }

        UpdateCurrentWave();

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
        WaveData newWave = null;

        for (int i = 0; i < waves.Count; i++)
        {
            if (elapsedTime >= waves[i].startTime && elapsedTime < waves[i].endTime)
            {
                newWave = waves[i];
                break;
            }
        }

        if (newWave != currentWave)
        {
            previousWave = currentWave;
            currentWave = newWave;

            if (currentWave != null)
            {
                EnvironmentSystem.instance.GetRandomEnvironment();

                if (currentWave.isBossWave)
                    SpawnBoss();
            }
        }
    }

    private void SpawnBoss()
    {
        if (currentWave.bossPrefab == null) return;

        if (TryGetValidEnemySpawnPoint(out Vector2 spawnPos))
        {
            GameObject boss = ObjectPool.instance.Spawn(currentWave.bossPrefab.name, spawnPos, Quaternion.identity);
            if (boss != null)
                aliveEnemies.Add(boss);
        }
    }

    private void TrySpawnEnemy()
    {
        if (currentWave == null) return;

        if (currentWave.isBossWave && !currentWave.spawnMinionWithBoss) return;

        if (aliveEnemies.Count >= currentWave.maxEnemiesAlive) return;
        if (currentWave.enemies == null || currentWave.enemies.Count == 0) return;

        if (!TryGetValidEnemySpawnPoint(out Vector2 spawnPos))
            return;

        GameObject prefab = GetRandomEnemyByWeight(currentWave.enemies);
        if (prefab == null) return;

        GameObject enemy = ObjectPool.instance.Spawn(prefab.name, spawnPos, Quaternion.identity);
        if (enemy != null)
        {
            aliveEnemies.Add(enemy);
        }
    }

    private bool TryGetValidEnemySpawnPoint(out Vector2 validPoint)
    {
        Bounds bounds = confinerBounds.bounds;
        Camera cam = Camera.main;

        for (int attempts = 0; attempts < maxRetry; attempts++)
        {
            Vector2 candidate = new Vector2(
                Random.Range(bounds.min.x, bounds.max.x),
                Random.Range(bounds.min.y, bounds.max.y)
            );

            Vector3 viewportPos = cam.WorldToViewportPoint(candidate);
            bool outsideCamera = viewportPos.x < 0 || viewportPos.x > 1 ||
                                 viewportPos.y < 0 || viewportPos.y > 1;

            if (!confinerBounds.OverlapPoint(candidate))
                continue;

            if (!outsideCamera)
                continue;

            if (Physics2D.OverlapCircle(candidate, enemySpawnCheckRadius, blockedSpawnLayer) != null)
                continue;

            validPoint = candidate;
            return true;
        }

        validPoint = Vector2.zero;
        return false;
    }

    private void SpawnBreakableAndObstacleObjectsAtWaveStart()
    {
        if (breakablePrefabs == null || breakablePrefabs.Count == 0) return;

        int spawnCount = Random.Range(minBreakablePerWave, maxBreakablePerWave + 1);

        for (int i = 0; i < spawnCount; i++)
        {
            GameObject prefab = breakablePrefabs[Random.Range(0, breakablePrefabs.Count)];
            if (prefab == null) continue;

            if (!TryGetValidBreakableSpawnPoint(out Vector2 spawnPos))
                continue;

            ObjectPool.instance.Spawn(prefab.name, spawnPos, Quaternion.identity);
        }
    }

    private bool TryGetValidBreakableSpawnPoint(out Vector2 validPoint)
    {
        Bounds bounds = confinerBounds.bounds;

        for (int attempts = 0; attempts < maxRetry; attempts++)
        {
            Vector2 candidate = new Vector2(
                Random.Range(bounds.min.x, bounds.max.x),
                Random.Range(bounds.min.y, bounds.max.y)
            );

            if (!confinerBounds.OverlapPoint(candidate))
                continue;

            if (Physics2D.OverlapCircle(candidate, breakableSpawnCheckRadius, blockedSpawnLayer) != null)
                continue;

            validPoint = candidate;
            return true;
        }

        validPoint = Vector2.zero;
        return false;
    }

    public void OnBossDefeated()
    {
        KillAllAliveMinions();
        GoToRewardPhase();
    }

    private void KillAllAliveMinions()
    {
        for (int i = aliveEnemies.Count - 1; i >= 0; i--)
        {
            GameObject enemy = aliveEnemies[i];
            if (enemy == null) continue;

            if (currentWave != null && currentWave.isBossWave && currentWave.bossPrefab != null)
            {
                if (enemy.name.Contains(currentWave.bossPrefab.name))
                    continue;
            }

            enemy.GetComponent<Enemy>().GetComponent<DropSystem>().SpawnDrop(enemy.transform.position);
            enemy.GetComponent<Enemy>().TryToDieState();
        }

        aliveEnemies.RemoveAll(e => e == null || !e.activeSelf);
    }

    private void GoToRewardPhase()
    {
        WaveData rewardWave = null;

        for (int i = 0; i < waves.Count; i++)
        {
            if (waves[i].isRewardWave)
            {
                rewardWave = waves[i];
                break;
            }
        }

        if (rewardWave == null)
        {
            isSpawning = false;
            return;
        }

        previousWave = currentWave;
        currentWave = rewardWave;

        elapsedTime = currentWave.startTime;
        spawnTimer = 0f;
        nextSpawnInterval = 0f;

        isSpawning = false;
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

    public int GetElapsedTime() => Mathf.FloorToInt(elapsedTime);

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, enemySpawnCheckRadius);
    }
#endif
}