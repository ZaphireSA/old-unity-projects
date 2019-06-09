using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour {

    public WaveEnemy[] waveEnemies;
    public float spawnDelay = 0.2f;
    public Transform[] spawnPoints;

    [Header("Infinite")]
    public bool isInfinite = true;
    public int infiniteStartPoints = 20;
    public int infiniteNewPoints = 10;
    public float infiniteModifier = 1.0f;
    public float infiniteTimeBetweenSpawns = 0.5f;
    public int infinitePreWaveAmount = 3;
    int preWaveCounter = 0;

    [Header("Waves")]
    public List<Wave> waves;
    Wave currentWave;
    int currentWaveNumber;

    Queue<WaveEnemy> enemiesToSpawn;
    int enemiesRemainingToSpawn = 0;
    int enemiesRemainingAlive = 0;
    float nextSpawnTime;

    bool isActivated = false;

    public event System.Action<int> OnNewWave;
    public event System.Action<int> OnPreWave;
    public event System.Action NoMoreWaves;
    public event System.Action<int> TotalEnemiesRemaining;

    void Start()
    {
        GameObject.FindObjectOfType<Player>().OnDeath += OnPlayerDeath;
        if (isInfinite)
        {
            waves = new List<Wave>();
            preWaveCounter = infinitePreWaveAmount;
        }
    }

    void OnPlayerDeath()
    {
        isActivated = false;
    }

    void Update()
    {
        if (isActivated && currentWave != null)
        {
            if ((enemiesRemainingToSpawn > 0) && Time.time > nextSpawnTime)
            {
                enemiesRemainingToSpawn--;
                nextSpawnTime = Time.time + currentWave.timeBetweenSpawns;
                StartCoroutine(SpawnEnemy());
            }
        }
    }

    IEnumerator SpawnEnemy()
    {
        float spawnDelay = 1f;

        Color initialColor = Color.white;
        Color flashColor = Color.red;
        float spawnTimer = 0f;

        while (spawnTimer < spawnDelay)
        {
            spawnTimer += Time.deltaTime;
            yield return null;
        }

        Transform enemyToSpawn = enemiesToSpawn.Dequeue().enemyPrefab;

        Vector3 spawnPos = Vector3.zero;
        
        if (spawnPoints.Length > 0)
        {
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)].transform;
            Vector3 spawnBox = transform.localScale;
            Vector3 spawnPosition = new Vector3(Random.value * spawnBox.x, spawnBox.y, Random.value * spawnBox.z);
            spawnPosition = spawnPoint.TransformPoint(spawnPosition - spawnBox / 2);

            Ray ray = new Ray(spawnPosition, Vector3.down);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                spawnPos = hit.point;
            }
        }

        if (enemyToSpawn != null)
        {
            Quaternion spawnRotation = Quaternion.Euler(0, Random.Range(0f, 360f), 0);
            GameObject spawnedEnemy = Instantiate(enemyToSpawn, spawnPos + Vector3.up, spawnRotation).gameObject;

            EnemyStateManager enemySM = spawnedEnemy.GetComponentInParent<EnemyStateManager>();
            if (enemySM != null)
            {
                enemySM.OnDeath += OnEnemyDeath;
                enemySM.isRaged = true;
            }
            

        }
    }

    public void TriggerSpawns()
    {
        isActivated = true;
        StartCoroutine(NextWaveTrigger());        
    }

    IEnumerator NextWaveTrigger()
    {
        yield return new WaitForSeconds(0.1f);
        if (isInfinite)
        {
            //print (preWaveCounter + " : " + infinitePreWaveAmount);
            GenerateNewInfiniteWave((preWaveCounter == infinitePreWaveAmount));
        }

        if (currentWaveNumber > 0)
        {
            //AudioManager.instance.PlaySound2D("Level Complete");
        }

        if (currentWaveNumber < waves.Count)
        {
            if (waves[currentWaveNumber].prePause)
            {
                //if (OnPreWave != null)
                //{
                //    OnPreWave(currentWaveNumber + 1);
                //}

                yield return new WaitForSeconds(5f);
            }
        }
        NextWave();
    }

    void NextWave()
    {
        currentWaveNumber++;
        //nextCampCheckTime = timeBetweenCampingChecks + Time.time;
        if (currentWaveNumber - 1 < waves.Count)
        {
            enemiesToSpawn = new Queue<WaveEnemy>();
            currentWave = waves[currentWaveNumber - 1];

            PopulateEnemyList(currentWave.totalPoints);
            enemiesRemainingToSpawn = enemiesToSpawn.Count;
            enemiesRemainingAlive = enemiesRemainingToSpawn;

            if (TotalEnemiesRemaining != null)
            {
                TotalEnemiesRemaining(enemiesRemainingAlive);
            }

            //if (txtWaveNumber != null)
            //{
            //    txtWaveNumber.text = currentWaveNumber.ToString();
            //    txtWaveMessage.text = "";
            //}
            if (OnNewWave != null)
            {
                OnNewWave(currentWaveNumber);
            }

            //ResetPlayerPosition();
        }
        else
        {
            if (NoMoreWaves != null)
            {
                NoMoreWaves();
            }
        }
    }

    void OnEnemyDeath()
    {
        enemiesRemainingAlive--;
        if (TotalEnemiesRemaining != null)
        {
            TotalEnemiesRemaining(enemiesRemainingAlive);
        }
        if (enemiesRemainingAlive == 0)
        {
            //NextWave();
            StartCoroutine(NextWaveTrigger());
        }
    }

    void PopulateEnemyList(int maxPoints)
    {
        int pointsLeft = maxPoints;
        while (pointsLeft > 0)
        {
            WaveEnemy tempEnemy = GetRandomEnemy(pointsLeft);
            if (tempEnemy != null)
            {
                pointsLeft -= tempEnemy.pointCost;
                enemiesToSpawn.Enqueue(tempEnemy);
            } else
            {
                break;
            }
        }
    }

    WaveEnemy GetRandomEnemy(int maxPointCost)
    {
        List<WaveEnemy> belowThresholdEnemies = new List<WaveEnemy>();
        foreach (WaveEnemy wEnemy in waveEnemies)
        {
            if (wEnemy.requiredWave <= (currentWaveNumber) && wEnemy.pointCost <= maxPointCost)
            {
                belowThresholdEnemies.Add(wEnemy);
            }
        }
        return (belowThresholdEnemies.Count > 0) ? belowThresholdEnemies[Random.Range(0, belowThresholdEnemies.Count)] : null;
    }

    void GenerateNewInfiniteWave(bool isPre)
    {
        int wavePoints = infiniteStartPoints + infiniteNewPoints;
        if (waves.Count > 0)
        {
            wavePoints = waves[currentWaveNumber - 1].totalPoints + infiniteNewPoints;
        }
        wavePoints = Mathf.RoundToInt(wavePoints * infiniteModifier);
        Wave newWave = new Wave(isPre, infiniteTimeBetweenSpawns, wavePoints);
        waves.Add(newWave);
        preWaveCounter = isPre ? 0 : preWaveCounter + 1;
    }

}

[System.Serializable]
public class Wave
{
    public bool prePause;
    public float timeBetweenSpawns;
    public int totalPoints;

    public Wave(bool prePause, float timeBetweenSpawns, int totalPoints)
    {
        this.prePause = prePause;
        this.timeBetweenSpawns = timeBetweenSpawns;
        this.totalPoints = totalPoints;
    }
}

[System.Serializable]
public class WaveEnemy
{
    public Transform enemyPrefab;
    public int pointCost;
    public int requiredWave;
}
