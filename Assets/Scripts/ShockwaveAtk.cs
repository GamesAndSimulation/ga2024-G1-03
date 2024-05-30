using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockwaveAtk : MonoBehaviour
{
    public int damage = 30; 
    private float growthRate = 7f; 
    private float maxRadius = 2.5f;
    private SphereCollider shockwaveCollider;
    public bool isPlayer;

    void Start()
    {
        shockwaveCollider = GetComponent<SphereCollider>();
        shockwaveCollider.radius = 0.1f;
        GetComponent<AudioSource>().Play();
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
        string otherTag = isPlayer ? "Enemy" : "Player";
        if (isPlayer)
        {
            if (other.CompareTag("Enemy"))
            {
                //Debug.Log("enemy hit");
            }
        }
        else
        {
            if (other.CompareTag("Player"))
            {
                //Debug.Log("player hit");
            }
        }
    }
}
