using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Rigidbody enemyRb;
    GameObject player;

    [SerializeField]
    float enemySpeed = 3.0f;
    [SerializeField]
    bool isBoss = false;

    float spawnInterval = 2f;
    float nextSpawn;
    int miniEnemySpawnCount;

    SpawnManager spawnManager;

    public int MiniEnemy
    {
        set
        {
            miniEnemySpawnCount = value;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        enemyRb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player");

        if (isBoss)
        {
            spawnManager = FindObjectOfType<SpawnManager>();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        followPlayer();
        DestroyEnemy();
        BossPhase();
    }

    void followPlayer()
    {
        if (player != null && !GameObject.Find("UIManager").GetComponent<UIManager>().IsPaused)
        {
            Vector3 lookDirection = (player.transform.position - transform.position).normalized;
            enemyRb.AddForce(lookDirection * enemySpeed);
        }
    }

    void BossPhase()
    {
        if (isBoss)
        {
            if(Time.time > nextSpawn)
            {
                nextSpawn = Time.time + spawnInterval;
                spawnManager.SpawnMiniEnemy(miniEnemySpawnCount);
            }
        }
    }


    void DestroyEnemy()
    {
        if(transform.position.y < -5)
        {
            Destroy(gameObject);
        }
    }
}
