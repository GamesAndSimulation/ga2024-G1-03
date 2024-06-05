using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossScript : MonoBehaviour
{
    public float health = 500f;
    public CharacterController controller;
    [SerializeField] private GameObject swordsPrefab;
    [SerializeField] private Transform swordsSpawn;
    [SerializeField] private Transform sword;
    [SerializeField] private AudioSource swordsAudio;
    public Animator animator;
    private float distanceToPlayer;
    public Transform player;
    private Vector3 direction;
    [SerializeField] private GameObject rocks;
    [SerializeField] private Transform rocksSpawn;
    [SerializeField] private GameObject shockwaveVFX;
    [SerializeField] private Transform shockwavePos;
    [SerializeField] private AudioSource shockwaveAudio;
    private Vector3 move;
    private float minimumChaseDistance = 2f;
    private float startChaseDistance = 3f;
    private bool isChasing = false;
    private bool isLeaping = false;
    private bool canMove = true;
    private Vector3 leapDirection;
    public float walkSpeed = 3.0f; 
    public bool stunPlayer = false; 
    private float meleeCooldown = 5f;
    private float lastMeleeTime = 0f;
    [SerializeField] private GameObject swordVFX;
    [SerializeField] private Transform swordVFXPosition;
    [SerializeField] private GameObject fireball;
    [SerializeField] private AudioSource fireballAudio;
    [SerializeField] private GameObject jumpTrigger;
    private float abilityCooldown = 50f;
    private float lastAbilityTime = 0f;
    private float comboCooldown = 15f;
    private float lastComboTime = 0f;
    private AnimatorStateInfo stateInfo;
    private bool phaseTwo = false;
    private bool inAbility = false;
    private bool inCombo = false;
    private bool transitioning = false;
    private BlinkScript blink;
    [SerializeField] private Image hpBar;
    private bool isDead = false;
    public GameObject bossUI;
    public GameObject phaseTwoEffect;
    private bool isDodging = false;

    void Start()
    {
        controller = gameObject.GetComponentInParent<CharacterController>();   
        blink = GetComponent<BlinkScript>();
        animator.SetTrigger("Awake");
        lastAbilityTime = Time.time;
        lastComboTime = Time.time;
        lastMeleeTime = Time.time;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.M))
        {
            StartCoroutine(StartPhaseTwo());
        }


        if (!isDead)
        {
            stateInfo = animator.GetCurrentAnimatorStateInfo(0);

            if (stunPlayer)
            {
                stunPlayer = false;
                StartCoroutine(FindObjectOfType<PlayerMovement>().Stun(3f, 30f));
            }

            distanceToPlayer = Vector3.Distance(transform.position, player.position);
            direction = (player.position - transform.position).normalized;

            if (!transitioning)
            {       
                Melee();
                AbilitiesManager();
                ComboManager();
            }

            move = new(0,0,0);

            if (!isChasing && distanceToPlayer > startChaseDistance)
            {
                isChasing = true;
            }
            else if (isChasing && distanceToPlayer <= minimumChaseDistance)
            {
                isChasing = false;
            }

            if (canMove && (isChasing || isLeaping))
            {

                float speed = walkSpeed;

                if (isLeaping)
                {
                    move = leapDirection;
                }
                else
                {
                    move = new Vector3(direction.x, 0, direction.z) * speed;
                    gameObject.transform.forward = move;
                }

                controller.Move(move * Time.deltaTime);
            }
            animator.SetFloat("Speed", move.magnitude);
        }
    }

    private IEnumerator SpinSwords()
    {

        canMove = false;
        inAbility = true;
        animator.SetTrigger("SpinSwords");
        yield return new WaitForSeconds(2.3f);

        GameObject swords = Instantiate(swordsPrefab, swordsSpawn.position, swordsSpawn.rotation);
        Destroy(swords, 15f);
        swordsAudio.Play();
        StartCoroutine(RotateSwordCenter(swords));
    }

    private IEnumerator RotateSwordCenter(GameObject swords)
    {
        float rotationSpeed = 180f; 
        float rotationAmount = 0f;
        
        float elapsedTime = 1f;
        float duration = Random.Range(4f, 7f);
        while (elapsedTime < duration)
        {
            rotationAmount = rotationSpeed/(elapsedTime/2) * Time.deltaTime;
            swordsAudio.pitch = rotationAmount > 1.5f ? rotationAmount/1.5f : rotationAmount;
            swords.transform.Rotate(0f, rotationAmount, 0f);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        elapsedTime = 1f;
        while (elapsedTime < duration*0.67f)
        {
            rotationAmount = elapsedTime*5000/rotationSpeed * Time.deltaTime;
            swordsAudio.pitch = rotationAmount;
            swords.transform.Rotate(0f, -rotationAmount, 0f);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        elapsedTime = 1f;
        while (elapsedTime < duration * 0.55f)
        {
            rotationAmount -= 1f * Time.deltaTime;
            swordsAudio.pitch = rotationAmount;
            swords.transform.Rotate(0f, -rotationAmount, 0f);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        swordsAudio.Stop();
        yield return new WaitForSeconds(0.3f);
        StartCoroutine(ThrowSwords(swords));
    }

    private IEnumerator ThrowSwords(GameObject swords)
    {
        
        foreach(Projectile sword in swords.GetComponentsInChildren<Projectile>())
        {
            yield return new WaitForSeconds(0.1f);
            sword.enabled = true;
        }
        yield return new WaitForSeconds(0.6f);
        animator.SetTrigger("SpinDone");
        yield return new WaitForSeconds(0.8f);
        canMove = true;
        inAbility = false;
    }

    private IEnumerator Leap()
    {
        canMove = false;
        inAbility = true;
        direction = (player.position - transform.position).normalized;
        gameObject.transform.forward = direction;
        animator.SetTrigger("Leap");

        yield return new WaitForSeconds(0.5f);

        //lock direction for the leap
        leapDirection = transform.forward;
        isLeaping = true;

        yield return new WaitForSeconds(1.2f);
        shockwaveAudio.Play();
         yield return new WaitForSeconds(0.3f);

        GameObject vfx = Instantiate(shockwaveVFX, shockwavePos.position, shockwavePos.rotation);
        Destroy(vfx, 4f);
        
        GameObject rock = Instantiate(rocks, rocksSpawn, false);
        rock.transform.SetParent(null);
        Destroy(rock, 5f);
        isLeaping = false;

        yield return new WaitForSeconds(0.7f);
        GameObject vfx2 = Instantiate(shockwaveVFX, shockwavePos.position, shockwavePos.rotation);
        vfx2.transform.localScale *= 1.7f;
        Destroy(vfx2, 4f);
        yield return new WaitForSeconds(1f);
        animator.SetTrigger("DoneCombo");
        yield return new WaitForSeconds(2f);
        canMove = true;
        inAbility = false;
    }

    public IEnumerator Dodge()
    {
        float odd = Random.Range(0f, 1f);
        isDodging = true;
        if (odd < 0.3f)
        {
            if (odd < 0.15f)

            animator.SetTrigger("DodgeR");
            else
            animator.SetTrigger("DodgeL");
        }
        yield return new WaitForSeconds(0.5f);
        isDodging = false;
    }

    private void Melee()
    {
        float currentTime = Time.time;
        if (!inAbility && !inCombo && !isDodging && distanceToPlayer < startChaseDistance && currentTime >= lastMeleeTime + meleeCooldown 
        && canMove && (stateInfo.IsName("Idle") || stateInfo.IsName("Walk"))) 
        {
            gameObject.transform.forward = direction;
            lastMeleeTime = currentTime;
            animator.SetTrigger("Melee");
            StartCoroutine(DelayedSword(false));
        }
    }

    private IEnumerator DelayedSword(bool inverse)
    {
        direction = (player.position - transform.position).normalized;
        gameObject.transform.forward = direction;
        Quaternion baseRotation = Quaternion.Euler(-45, 180, 10) * Quaternion.Euler(0, 0, transform.rotation.eulerAngles.y+180);
        Quaternion rotation = inverse ? Quaternion.Euler(180, 0, 0) * baseRotation : baseRotation;
        yield return new WaitForSeconds(0.3f);
        yield return new WaitForSeconds(0.1f);
        GameObject vfx = Instantiate(swordVFX, swordVFXPosition.position + (transform.forward/3), rotation);
        Destroy(vfx, 1.5f);
        yield return new WaitForSeconds(0.1f);
    }

    private IEnumerator BasicCombo()
    {
        canMove = false;
        inCombo = true;
        animator.SetTrigger("Combo"); 
        
        yield return new WaitForSeconds(0.3f);
        StartCoroutine(DelayedSword(true));
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(DelayedSword(false));
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(DelayedSword(true));
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(DelayedSword(false));
        yield return new WaitForSeconds(2f);

        animator.SetTrigger("DoneCombo");
        canMove = true;
        inCombo = false;
        
    }

    private IEnumerator JumpCombo()
    {
        canMove = false;
        inCombo = true;
        animator.SetTrigger("ComboJump");

        StartCoroutine(DelayedSword(false));
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(DelayedSword(true));           //mudar o damage do ultimo hit para mais
        yield return new WaitForSeconds(1.5f);
        jumpTrigger.SetActive(true);
        StartCoroutine(DelayedSword(false));
        yield return new WaitForSeconds(2f);

        animator.SetTrigger("DoneCombo");
        jumpTrigger.SetActive(false);
        canMove = true;
        inCombo = false;
    }

    private IEnumerator FireBall()
    {
        canMove = false;
        inCombo = true;
        animator.SetTrigger("Slice");
        yield return new WaitForSeconds(1.3f);
        direction = (player.position - transform.position).normalized;
        gameObject.transform.forward = direction;
        fireballAudio.Play();
        yield return new WaitForSeconds(0.2f);
        GameObject slice = Instantiate(fireball, swordVFXPosition.position, swordVFXPosition.rotation);
        Destroy(slice, 1f);
        yield return new WaitForSeconds(0.7f);
        animator.SetTrigger("DoneCombo");
        yield return new WaitForSeconds(0.3f);
        canMove = true;
        inCombo = false;
    }

    private void AbilitiesManager()
    {
        float currentTime = Time.time;
        if (currentTime >= lastAbilityTime + abilityCooldown) 
        {
            if (!inCombo)
            {
                if (!phaseTwo)
                {
                    StartCoroutine(SpinSwords());
                }
                else
                {
                    //phase two
                    float odd = Random.Range(0f, 1f);

                    if (odd < 0.5f)
                    {
                        StartCoroutine(SpinSwords());
                    } 
                    else
                    {
                        StartCoroutine(Leap());
                    }
                }
            }
            if (phaseTwo)
                lastAbilityTime = currentTime - 25f;
            else
                lastAbilityTime = currentTime;
        }
    }

    private void ComboManager()
    {
        float currentTime = Time.time;
        if (currentTime >= lastComboTime + comboCooldown) 
        {
            Debug.Log(1);
            if(!inAbility)
            {
                if (!phaseTwo)
                {
                        float odd = Random.Range(0f, 1f);
                        
                        if (odd < 0.6f)
                        {
                            StartCoroutine(BasicCombo());
                        } 
                        else
                        {
                            StartCoroutine(FireBall());
                        }
                }
                else
                {
                    //phase two
                    float odd = Random.Range(0f, 1f);

                    if (odd < 0.4f)
                    {
                        StartCoroutine(JumpCombo());
                    } 
                    else if (odd < 0.8f)
                    {
                        StartCoroutine(BasicCombo());
                    }
                    else
                    {
                        StartCoroutine(FireBall());
                    }
                }
            }

            if (phaseTwo)
                lastComboTime = currentTime - 5f;
            else
                lastComboTime = currentTime;
        }
    }

    public void TakeDamage(float damage)
    {
        if (!inAbility && !transitioning && !isDead)
        { 
            health -= damage;
            hpBar.fillAmount = health / 500f;
            StartCoroutine(blink.FlashWhite(0.5f));

            if (!phaseTwo && health < 250f)
            {
                StopAllCoroutines();
                StartCoroutine(blink.FlashWhite(0.5f));
                StartCoroutine(StartPhaseTwo());
            }

            if (health <= 0)
            {
                StopAllCoroutines();
                StartCoroutine(blink.FlashWhite(0.5f));
                StartCoroutine(Die());
            }
        }
    }

    private IEnumerator StartPhaseTwo()
    {
        phaseTwo = true;
        canMove = false;
        transitioning = true;
        animator.SetTrigger("PhaseTwo");
        //meter falas da historia
        yield return new WaitForSeconds(2f);
        animator.SetTrigger("Rise");
        yield return new WaitForSeconds(4f);
        phaseTwoEffect.SetActive(true);
        yield return new WaitForSeconds(1f);

        //reset timers
        lastAbilityTime = Time.time;
        lastComboTime = Time.time;
        lastMeleeTime = Time.time;
        transitioning = false;
        canMove = true;
        
    }

    private IEnumerator Die()
    {
        animator.SetTrigger("Die");
        isDead = true;
        bossUI.SetActive(false);
        Destroy(gameObject, 10f);
        yield return new WaitForSeconds(0.5f);
        phaseTwoEffect.SetActive(false);
        //Dialogo
    }

}
