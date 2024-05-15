using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaggerScript : MonoBehaviour
{
    public int damage = 10; 
    public Animator animator;
    private AnimatorStateInfo stateInfo;

    void Start()
    {
     
    }

    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        stateInfo = animator.GetCurrentAnimatorStateInfo(1);
        if (other.CompareTag("Enemy") && (stateInfo.IsName("DaggerAttack") || stateInfo.IsName("PunchRight")))
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
