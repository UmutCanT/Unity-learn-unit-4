using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    GameObject enemyPrefab;
    [SerializeField]
    GameObject captainEnemyPrefab;
    [SerializeField]
    GameObject bossEnemyPrefab;
    [SerializeField]
    GameObject[] miniEnemyPrefabs;
    [SerializeField]
    GameObject[] powerUpPrefabs;

    PlayerController playerController;

    float spawnRange = 9.0f;
    int enemyCount;
    int waveNumber = 1;
    int bossRound = 5;

    public int WaveCount
    {
        get
        {
            return waveNumber;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerController.IsGameOver)
        {
            GameLoop();
        }       
    }

    void SpawnEnemy(int spawnCount)
    {
        if(spawnCount%3 == 0)
        {
            spawnCount -= 1;
            Instantiate(captainEnemyPrefab, GenerateSpawnPosition(), enemyPrefab.transform.rotation);
        }

        for (int i = 0; i < spawnCount; i++)
        {
            Instantiate(enemyPrefab, GenerateSpawnPosition(), enemyPrefab.transform.rotation);
        }
    }

    void SpawnPowerUp(int spawnCount)
    {
        if(spawnCount > 5)
        {
            spawnCount = 5;
        }

        int randomIndex = Random.Range(0, powerUpPrefabs.Length);

        for (int i = 0; i < spawnCount; i++)
        {
            Instantiate(powerUpPrefabs[randomIndex], GenerateSpawnPosition(), powerUpPrefabs[randomIndex].transform.rotation);
        }
    }

    Vector3 GenerateSpawnPosition()
    {
        float spawnPosX = Random.Range(-spawnRange, spawnRange);
        float spawnPosZ = Random.Range(-spawnRange, spawnRange);

        return new Vector3(spawnPosX, 0.5f, spawnPosZ);
    }

    void SpawnBossWave(int currentRound)
    {
        int miniEnemiesToSpawn;
        if(bossRound != 0)
        {
            miniEnemiesToSpawn = currentRound / bossRound;
        }
        else
        {
            miniEnemiesToSpawn = 1;
        }

        GameObject boss = Instantiate(bossEnemyPrefab, GenerateSpawnPosition(), bossEnemyPrefab.transform.rotation);
        boss.GetComponent<Enemy>().MiniEnemy = miniEnemiesToSpawn;
    }

    public void SpawnMiniEnemy(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            int randomMiniIndex = Random.Range(0, miniEnemyPrefabs.Length);
            Instantiate(miniEnemyPrefabs[randomMiniIndex], GenerateSpawnPosition(), miniEnemyPrefabs[randomMiniIndex].transform.rotation);
        }
    }

    void GameLoop()
    {       
        enemyCount = FindObjectsOfType<Enemy>().Length;

        if(enemyCount == 0)
        {
            if(waveNumber % bossRound == 0)
            {
                SpawnBossWave(waveNumber);
            }
            else
            {
                SpawnEnemy(waveNumber);
            }           
            SpawnPowerUp(waveNumber);
            waveNumber++;
        }
    }
}
