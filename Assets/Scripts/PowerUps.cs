using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PowerUpType {None, ZapCannon, RocketBarrage, Earthquake}

public class PowerUps : MonoBehaviour
{
    public PowerUpType powerUpType;

    float lifeTime = 10f;
    float rotationSpeed = 90f;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }
}
