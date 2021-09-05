using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketBehaviour : MonoBehaviour
{
    Transform target;
    float speed = 15f;
    float rocketStrength = 15f;
    float aliveTime = 5f;

    bool homing;

    void Update()
    {
        RotateTheRocket();
    }

    public void Fire(Transform homingTarget)
    {
        target = homingTarget;
        homing = true; 
        Destroy(gameObject, aliveTime);
    }

    void RotateTheRocket()
    {
        if(homing && target != null)
        {
            Vector3 moveDirection = (target.transform.position - transform.position).normalized;
            transform.position += moveDirection * speed * Time.deltaTime;
            transform.LookAt(target);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if(target != null)
        {
            if (collision.gameObject.CompareTag(target.tag))
            {
                Rigidbody targetRb = collision.gameObject.GetComponent<Rigidbody>();
                Vector3 away = -collision.GetContact(0).normal;
                targetRb.AddForce(away * rocketStrength, ForceMode.Impulse);
                Destroy(gameObject);
            }
        }
    }
}
