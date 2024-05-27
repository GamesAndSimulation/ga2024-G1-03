using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockwaveAtk : MonoBehaviour
{
    public int damage = 30; 
    private float growthRate = 7f; 
    private float maxRadius = 2.5f;

    private SphereCollider shockwaveCollider;

    void Start()
    {
        shockwaveCollider = GetComponent<SphereCollider>();
        shockwaveCollider.radius = 0.1f;
    }

    void Update()
    {
        if (shockwaveCollider.radius < maxRadius)
        {
            shockwaveCollider.radius += growthRate * Time.deltaTime;
            if (shockwaveCollider.radius > maxRadius)
            {
                shockwaveCollider.radius = maxRadius;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            /*
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }*/
            Debug.Log("enemy hit");
        }
    }
}
