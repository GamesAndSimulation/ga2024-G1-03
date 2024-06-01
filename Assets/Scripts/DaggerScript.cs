using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaggerScript : MonoBehaviour
{
    public int damage = 10; 
    private Animator animator;
    private AnimatorStateInfo stateInfo;

    void Start()
    {
        var playerController = FindObjectOfType<PlayerMovement>();
        animator = playerController.animator;

        var playerCombat = FindObjectOfType<PlayerCombat>();
        GetComponent<AudioSource>().clip = playerCombat.knightClip;
        GetComponent<AudioSource>().Play();
    }

    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        stateInfo = animator.GetCurrentAnimatorStateInfo(1);
        if (other.CompareTag("Enemy") && stateInfo.IsName("DaggerAttack" ))
        {
            /*
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }*/
            Debug.Log("enemy hit");
        }

        if (other.CompareTag("Boss"))
        {
            BossScript boss = other.GetComponent<BossScript>();
            boss.TakeDamage(damage);
        }
    }
}
