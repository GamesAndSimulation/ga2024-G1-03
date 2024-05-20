using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public int health = 100;
    public Animator animator;
    public PlayerMovement movementScript;
    [SerializeField] private CharacterManager charManager;
    public AnimatorStateInfo stateInfo;
    [SerializeField] private GameObject spell;
    [SerializeField] private GameObject swordVFX;
    [SerializeField] private Transform swordVFXPosition;

    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
    }

    void Update()
    {
        stateInfo = animator.GetCurrentAnimatorStateInfo(1);
        if (Input.GetButton("Fire1"))
        {
            switch (charManager.current)
            {
                case Characters.Knight:
                    KnightAttack();
                    break;
                case Characters.Mage:
                    MageAttack();
                    break;
                default:
                    break;
            }

        }
    }

    void KnightAttack()
    {
        //can only attack after last attack is done and if not rolling
        if (!stateInfo.IsName("DaggerAttack") && !stateInfo.IsName("PunchRight") && !animator.IsInTransition(1) && !movementScript.isRolling)
        {
            //if (Random.Range(0f, 1f) >= 0.3f){
                animator.SetTrigger("Attack");
                StartCoroutine(DelayedSword());
           // } else
             //   animator.SetTrigger("Punch");
        }
    }

    void MageAttack()
    {
        if (!stateInfo.IsName("SpellCast") && !animator.IsInTransition(1) && !movementScript.isRolling)
        {
            animator.SetTrigger("Spell"); 
            StartCoroutine(DelayedSpell());
        }
    }

    IEnumerator DelayedSpell()
    {
        yield return new WaitForSeconds(0.7f);
        GameObject newspell = Instantiate(spell, charManager.attacks[1].transform.position, charManager.attacks[1].transform.rotation);
        Destroy(newspell, 5);
    }

    IEnumerator DelayedSword()
    {
        Quaternion rotation = Quaternion.Euler(-45, 180, 10) * Quaternion.Euler(0, 0, transform.rotation.eulerAngles.y+180);

        yield return new WaitForSeconds(0.3f);
        GameObject vfx = Instantiate(swordVFX, swordVFXPosition.position + (transform.forward/3), rotation);
        Destroy(vfx, 1.5f);
    }

}
