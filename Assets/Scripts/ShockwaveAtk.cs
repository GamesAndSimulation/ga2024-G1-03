using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockwaveAtk : MonoBehaviour
{
    public int damage; 
    private float growthRate = 7f; 
    private float maxRadius = 2.5f;
    private SphereCollider shockwaveCollider;
    public bool isPlayer;

    void Start()
    {
        shockwaveCollider = GetComponent<SphereCollider>();
        shockwaveCollider.radius = 0.1f;
        
        if (TryGetComponent<AudioSource>(out var audio))
            audio.Play();
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
        if (isPlayer)
        {
            if (other.CompareTag("Enemy"))
            {
                other.GetComponent<EnemyHealth>().TakeDamage(damage);
            }
            
            if (other.CompareTag("Boss"))
            {
                BossScript boss = other.GetComponent<BossScript>();
                boss.TakeDamage(damage);
            }
        }
    }
}
