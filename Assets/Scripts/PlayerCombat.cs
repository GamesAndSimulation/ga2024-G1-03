using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public int health = 100;
    private Animator animator;
    public PlayerMovement movementScript;

    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetButton("Fire1"))
        {
            KnightAttack();
        }

    }

    void KnightAttack(){
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(1); 

        //can only attack after last attack is done and if not rolling
        if (!stateInfo.IsName("DaggerAttack") && !stateInfo.IsName("PunchRight") && !animator.IsInTransition(1) && !movementScript.isRolling){ 
            if(Random.Range(0f, 1f) >= 0.3f)
                animator.SetTrigger("Attack");
            else
                animator.SetTrigger("Punch");
        }

    }
}
