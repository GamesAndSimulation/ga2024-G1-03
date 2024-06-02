using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTrigger : MonoBehaviour
{
    public int damage = 20; 
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
        stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (other.CompareTag("Player") && stateInfo.IsName("combo_jump") )
        {
            StartCoroutine(FindObjectOfType<PlayerCombat>().TakeDamage(damage));
        }
    }
}
