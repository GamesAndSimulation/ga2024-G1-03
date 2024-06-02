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
    public Animator animator2;
    public PlayerMovement movementScript;
    [SerializeField] private CharacterManager charManager;
    public AnimatorStateInfo stateInfo;
    public AnimatorStateInfo stateInfo2;
    [SerializeField] private GameObject spell;
    [SerializeField] private GameObject swordVFX;
    [SerializeField] private Transform swordVFXPosition;
    [SerializeField] private GameObject dwarfVFX;
    [SerializeField] private Transform dwarfVFXPosition;
    [SerializeField] private Image hpBar;
    [SerializeField] private Image staminaBar;
    [SerializeField] private TextMeshProUGUI staminaText;
    [SerializeField] private Image backgroundImage;
    public Vector3 dwarfAtkDirection;
    public GameObject dwarf;
    public bool dwarfAttack = false;
    public bool isAttacking = false;
    [SerializeField] private SoundGeneration soundGeneration;
    public AudioClip knightClip;
    [SerializeField] private BossScript boss;
    public BlinkScript blink;

    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        movementScript = GetComponent<PlayerMovement>();
        blink = GetComponent<BlinkScript>();
    }

    void Update()
    {
        //for debug, remove later
        if(Input.GetKeyDown(KeyCode.L))
        {
            StartCoroutine(TakeDamage(10f));
        }

        hpBar.fillAmount = health/100;
        staminaBar.fillAmount = stamina/100;

        stateInfo = animator.GetCurrentAnimatorStateInfo(1);
        stateInfo2 = animator2.GetCurrentAnimatorStateInfo(0);

        if (Input.GetButton("Fire1") && !movementScript.isStunned)
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
            stamina += 0.1f * Time.deltaTime * 60f;
            if (stamina > 100) stamina = 100;
            //stamina += 100f;
        }
    }

    private void AttackStamina(float cost)
    {
        stamina -= cost;
    }

    void KnightAttack()
    {
        //can only attack after last attack is done and if not rolling
        if (!stateInfo.IsName("DaggerAttack") && !animator.IsInTransition(1) && !movementScript.isRolling)
        {
            if (NoStaminaAlert(knightCost)) return;
            if (boss.enabled == true) boss.Dodge();
            knightClip = soundGeneration.GenerateAudio();
            isAttacking = true;
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
            if (boss.enabled == true) boss.Dodge();
            isAttacking = true;
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
        yield return new WaitForSeconds(0.15f);
        isAttacking = false;
    }

    IEnumerator DelayedSword()
    {
        Quaternion rotation = Quaternion.Euler(-45, 180, 10) * Quaternion.Euler(0, 0, transform.rotation.eulerAngles.y+180);

        yield return new WaitForSeconds(0.3f);
        GameObject vfx = Instantiate(swordVFX, swordVFXPosition.position + (transform.forward/3), rotation);
        Destroy(vfx, 1.5f);
        yield return new WaitForSeconds(0.1f);
        isAttacking = false;
    }

    void DwarfAttack()
    {
        if (!stateInfo2.IsName("DwarfAtk") && !animator2.IsInTransition(0) && !movementScript.isRolling)
        {
            if (NoStaminaAlert(dwarfCost)) return;
            if (boss.enabled == true) boss.Dodge();
            isAttacking = true;
            dwarfAttack = true;
            //dwarf.transform.SetParent(null);
            AttackStamina(dwarfCost);
            animator2.SetTrigger("DwarfAtk");
            StartCoroutine(DelayedDwarf());
        }
    }

    IEnumerator DelayedDwarf()
    {
        yield return new WaitForSeconds(1.44f);
        GameObject vfx = Instantiate(dwarfVFX, dwarfVFXPosition.position, Quaternion.Euler(0, 0, 0));
        Destroy(vfx, 1.5f);

        yield return new WaitForSeconds(1.2f);
        //dwarf.transform.SetParent(movementScript.controller2.gameObject.transform);
        dwarfAttack = false;
        isAttacking = false;
    }

    public bool NoStaminaAlert(float cost)
    {
        if (stamina < cost)
        {
            if (staminaText.alpha == 0)
            {
                StartCoroutine(FadeImageInAndOut(backgroundImage, 1f));
                StartCoroutine(FadeTextInAndOut(staminaText, 1f));
            }
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

    public IEnumerator FadeImageInAndOut(Image image, float duration)
    {
        float halfDuration = duration / 2f;

        float elapsedTime = 0f;
        while (elapsedTime < halfDuration)
        {
            elapsedTime += Time.deltaTime;
            Color tempColor = image.color;
            tempColor.a = Mathf.Lerp(0, 0.9f, elapsedTime / halfDuration);
            image.color = tempColor;
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        elapsedTime = 0f;
        while (elapsedTime < halfDuration)
        {
            elapsedTime += Time.deltaTime;
            Color tempColor = image.color;
            tempColor.a = Mathf.Lerp(0.9f, 0, elapsedTime / halfDuration);
            image.color = tempColor;
            yield return null;
        }

        Color finalColor = image.color;
        finalColor.a = 0;
        image.color = finalColor;
    }

    public IEnumerator TakeDamage(float damage)
    {
        if(!movementScript.isRolling)
        {
            health -= damage;
            animator.SetTrigger("Hit");
            animator2.SetTrigger("Hit");
            movementScript.isStunned = true;
            StartCoroutine(blink.FlashWhite(0.5f));
            yield return new WaitForSeconds(0.2f);
            movementScript.isStunned = false;
        }
    }

    private void Die()
    {
        
    }

}
