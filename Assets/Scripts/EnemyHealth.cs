using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float health;
    public BlinkScript blink;

    // Start is called before the first frame update
    void Start()
    {
        blink = GetComponent<BlinkScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float dmg)
    {
        health -= dmg;
        StartCoroutine(blink.FlashWhite(0.5f));

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

}
