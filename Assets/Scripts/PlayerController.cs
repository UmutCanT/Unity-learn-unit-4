using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody playerRb;
    GameObject focalPoint;
    GameObject tmpRocket;
    Coroutine powerUpCountdown;
    PowerUpType currentPowerUp = PowerUpType.None;

    [SerializeField]
    GameObject powerUpIndicator;
    [SerializeField]
    GameObject rocketPrefab;

    bool gameOver = false;
    float moveSpeed = 10.0f;
    float pushPower = 60.0f;

    //Earthquake skill variables
    float hangTime = 0.5f;
    float smashSpeed = 10f;
    float explosionForce = 50f;
    float explosionRadius = 8f;
    bool smashing = false;
    float floorY;

    public bool IsGameOver
    {
        get
        {
            return gameOver;
        }

        set
        {
            gameOver = value;
        }
    }

    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("FocalPoint");
    }

    void Update()
    {
        PlayerPosition();
        if (!gameOver)
        {
            VerticalMovement();
            PowerUpIndicatorPosition();
            FireRockets();
            Earthquake();
        }
        else
        {
            GameObject.Find("UIManager").GetComponent<UIManager>().GameOver();
        }      
    }

    void PlayerPosition()
    {
        if (transform.position.y < -5)
        {
            gameOver = true;
            Destroy(gameObject);
        }
    }

    void VerticalMovement()
    {
        float verticalInput = Input.GetAxis("Vertical");

        playerRb.AddForce(focalPoint.transform.forward * verticalInput * moveSpeed);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Powerup"))
        {        
            currentPowerUp = other.gameObject.GetComponent<PowerUps>().powerUpType;
            powerUpIndicator.gameObject.SetActive(true);
            powerUpIndicator.gameObject.GetComponent<Renderer>().material.color = PowerUpColor(currentPowerUp);
            Destroy(other.gameObject);
            if(powerUpCountdown != null)
            {
                StopCoroutine(powerUpCountdown);
            }
            powerUpCountdown = StartCoroutine(PowerUpCountdownRoutine());
        }
    }

    IEnumerator PowerUpCountdownRoutine()
    {
        yield return new WaitForSeconds(7);
        currentPowerUp = PowerUpType.None;
        powerUpIndicator.gameObject.SetActive(false);
    }

    IEnumerator Smash()
    {
        //Player orginal position.y
        floorY = transform.position.y;

        float jumpTime = Time.time + hangTime;

        while(Time.time < jumpTime)
        {
            //Moving the player up while still keeping their velocity
            playerRb.velocity = new Vector2(playerRb.velocity.x, smashSpeed);
            yield return null;
        }

        //Moving the player down
        while(transform.position.y > floorY)
        {
            playerRb.velocity = new Vector2(playerRb.velocity.x, -smashSpeed * 2);
            yield return null;
        }

        foreach (var enemy in FindObjectsOfType<Enemy>())
        {
            if(enemy != null)
            {
                enemy.GetComponent<Rigidbody>().AddExplosionForce(explosionForce, transform.position, explosionRadius, 0.0f, ForceMode.Impulse);
            }
        }

        smashing = false;
    }

    void Earthquake()
    {
        if(currentPowerUp == PowerUpType.Earthquake && !smashing && Input.GetKeyDown(KeyCode.Space))
        {
            smashing = true;
            StartCoroutine(Smash());
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Enemy") && currentPowerUp == PowerUpType.ZapCannon)
        {
            Rigidbody enemyRb = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = collision.gameObject.transform.position - transform.position;
            enemyRb.AddForce(awayFromPlayer * pushPower, ForceMode.Impulse);
        }
    }

    void LaunchRockets()
    {
        foreach(var enemy in FindObjectsOfType<Enemy>())
        {
            tmpRocket = Instantiate(rocketPrefab, transform.position, Quaternion.identity);
            tmpRocket.GetComponent<RocketBehaviour>().Fire(enemy.transform);
        }
    }
    void FireRockets()
    {
        if(currentPowerUp == PowerUpType.RocketBarrage && Input.GetKeyDown(KeyCode.F))
        {
            LaunchRockets();
        }
    }

    void PowerUpIndicatorPosition()
    {
        Vector3 offset = new Vector3(0, -0.25f, 0);
        powerUpIndicator.transform.position = transform.position + offset;
    }

    Color PowerUpColor(PowerUpType currentOne)
    {
        if (currentOne == PowerUpType.ZapCannon)
        {
            return Color.yellow;
        }else if (currentOne == PowerUpType.RocketBarrage)
        {
            return Color.red;
        }
        else
        {
            return Color.gray;
        }
    }
}
