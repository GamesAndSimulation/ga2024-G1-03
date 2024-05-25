using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerCombat : MonoBehaviour
{
    public float health = 100;
    public float stamina = 100;
    public float knightCost = 10;
    public float mageCost = 20;
    public float dwarfCost = 15;
    public Animator animator;
    private PlayerMovement movementScript;
    [SerializeField] private CharacterManager charManager;
    public AnimatorStateInfo stateInfo;
    [SerializeField] private GameObject spell;
    [SerializeField] private GameObject swordVFX;
    [SerializeField] private Transform swordVFXPosition;
    [SerializeField] private Image hpBar;
    [SerializeField] private Image staminaBar;
    [SerializeField] private TextMeshProUGUI staminaText;

    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        movementScript = GetComponent<PlayerMovement>();
    }

    void Update()
    {

        hpBar.fillAmount = health/100;
        staminaBar.fillAmount = stamina/100;

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
                case Characters.Dwarf:
                    DwarfAttack();
                    break;
                default:
                    break;
            }

        }

        if (stamina < 100){
            stamina += 0.025f;
        }
    }

    private void AttackStamina(float cost)
    {
        stamina -= cost;
    }

    void KnightAttack()
    {
        //can only attack after last attack is done and if not rolling
        if (!stateInfo.IsName("DaggerAttack") && !stateInfo.IsName("PunchRight") && !animator.IsInTransition(1) && !movementScript.isRolling)
        {
            if (NoStaminaAlert(knightCost)) return;
            AttackStamina(knightCost);
            animator.SetTrigger("Attack");
            StartCoroutine(DelayedSword());
        }
    }

    void MageAttack()
    {
        if (!stateInfo.IsName("SpellCast") && !animator.IsInTransition(1) && !movementScript.isRolling)
        {
            if (NoStaminaAlert(mageCost)) return;
            AttackStamina(mageCost);
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

    void DwarfAttack()
    {
        if (!stateInfo.IsName("DwarfAtk") && !animator.IsInTransition(1) && !movementScript.isRolling)
        {
            if (NoStaminaAlert(dwarfCost)) return;
            AttackStamina(dwarfCost);
            animator.SetTrigger("DwarfAtk"); 
        }
    }

    public bool NoStaminaAlert(float cost)
    {
        if (stamina < cost)
        {
            StartCoroutine(FadeTextInAndOut(staminaText, 0.5f));
            return true;
        }

        return false;
    }

    public IEnumerator FadeTextInAndOut(TextMeshProUGUI text, float duration)
    {
        float halfDuration = duration / 2f;

        //fade in
        float elapsedTime = 0f;
        while (elapsedTime < halfDuration)
        {
            elapsedTime += Time.deltaTime;
            text.alpha = Mathf.Lerp(0, 1, elapsedTime / halfDuration);
            yield return null;
        }

        //pause briefly at full opacity
        yield return new WaitForSeconds(0.5f);

        //fade out
        elapsedTime = 0f;
        while (elapsedTime < halfDuration)
        {
            elapsedTime += Time.deltaTime;
            text.alpha = Mathf.Lerp(1, 0, elapsedTime / halfDuration);
            yield return null;
        }

        text.alpha = 0;
    }

}
