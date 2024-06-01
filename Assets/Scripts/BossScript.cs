using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private float abilityCooldown = 50f;
    private float lastAbilityTime = 0f;
    private float comboCooldown = 15f;
    private float lastComboTime = 0f;
    private AnimatorStateInfo stateInfo;
    private bool phaseTwo = false;
    private bool inAbility = false;
    private bool transitioning = false;
    [SerializeField] private Renderer characterRenderer;
    [SerializeField] private Renderer helmetRenderer;
    [SerializeField] private Material whiteMaterial; 
    [SerializeField] private float flashDuration = 0.1f; 
    private Material originalMaterial;


    void Start()
    {
        controller = gameObject.GetComponentInParent<CharacterController>();
        animator = GetComponent<Animator>();

        if (characterRenderer != null)
        {
            originalMaterial = characterRenderer.material;
        }
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.L))
        {
            //StartCoroutine(FireBall());
            TakeDamage(50f);
        }

        stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        if (stunPlayer)
        {
            stunPlayer = false;
            StartCoroutine(FindObjectOfType<PlayerMovement>().Stun(3f));
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
            //swordsAudio.pitch = rotationAmount;
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
        //gameObject.transform.forward = direction;

        //lock direction for the leap
        leapDirection = transform.forward;
        isLeaping = true;
        //canMove = true;

        float leapDuration = 1.5f;
        yield return new WaitForSeconds(leapDuration);

        GameObject vfx = Instantiate(shockwaveVFX, shockwavePos.position, shockwavePos.rotation);
        Destroy(vfx, 4f);
        
        GameObject rock = Instantiate(rocks, rocksSpawn, false);
        rock.transform.SetParent(null);
        Destroy(rock, 5f);
        isLeaping = false;

        yield return new WaitForSeconds(5f);
        animator.SetTrigger("DoneCombo");
        canMove = true;
        inAbility = false;
    }

    public void Dodge()
    {
        float odd = Random.Range(0f, 1f);
        if (odd < 0.5f)
        {
            if (odd < 0.25f)

            animator.SetTrigger("DodgeR");
            else
            animator.SetTrigger("DodgeL");
        }
    }

    private void Melee()
    {
        float currentTime = Time.time;
        if (distanceToPlayer < startChaseDistance && currentTime >= lastMeleeTime + meleeCooldown && canMove && (stateInfo.IsName("Idle") || stateInfo.IsName("Walk"))) 
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
        
    }

    private IEnumerator JumpCombo()
    {
        canMove = false;
        animator.SetTrigger("ComboJump");
        StartCoroutine(DelayedSword(false));
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(DelayedSword(true));           //mudar o damage do ultimo hit para mais
        yield return new WaitForSeconds(1.5f);
        StartCoroutine(DelayedSword(false));
        yield return new WaitForSeconds(2f);
        animator.SetTrigger("DoneCombo");
        canMove = true;
    }

    private IEnumerator FireBall()
    {
        canMove = false;
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
    }

    private void AbilitiesManager()
    {
        float currentTime = Time.time;
        if (currentTime >= lastAbilityTime + abilityCooldown) 
        {
            if (!phaseTwo)
            {
                StartCoroutine(SpinSwords());
                lastAbilityTime = currentTime;
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
                lastAbilityTime = currentTime;
            }
        }
    }

    private void ComboManager()
    {
        float currentTime = Time.time;
        if (currentTime >= lastComboTime + comboCooldown) 
        {
            if (!phaseTwo)
            {
                if (!inAbility)
                {
                    float odd = Random.Range(0f, 1f);
                    
                    if (odd < 0.5f)
                    {
                        StartCoroutine(BasicCombo());
                    } 
                    else
                    {
                        StartCoroutine(FireBall());
                    }
                }
                lastComboTime = currentTime;
            }
            else
            {
                //phase two
                float odd = Random.Range(0f, 1f);
                lastAbilityTime = currentTime;

                if (odd < 0.4f)
                {
                    StartCoroutine(JumpCombo());
                } 
                else if (odd < 0.7f)
                {
                    StartCoroutine(BasicCombo());
                }
                else
                {
                    StartCoroutine(FireBall());
                }
                lastComboTime = currentTime;
            }

        }
    }

    public void TakeDamage(float damage)
    {
        if (!inAbility)
        { 
            health -= damage;

            if (characterRenderer != null && whiteMaterial != null)
            {
                StartCoroutine(FlashWhite());
            }

            if (!phaseTwo && health < 250f)
            {
                StartCoroutine(StartPhaseTwo());
            }

            if (health <= 0)
            {
                animator.SetTrigger("Die");
            }
        }
    }

    private IEnumerator FlashWhite()
    {
        characterRenderer.material = whiteMaterial;
        helmetRenderer.material = whiteMaterial;
        yield return new WaitForSeconds(flashDuration/7);
        characterRenderer.material = originalMaterial;
        helmetRenderer.material = originalMaterial;
        yield return new WaitForSeconds(flashDuration/5);
        characterRenderer.material = whiteMaterial;
        helmetRenderer.material = whiteMaterial;
        yield return new WaitForSeconds(flashDuration/7);
        characterRenderer.material = originalMaterial;
        helmetRenderer.material = originalMaterial;
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
        yield return new WaitForSeconds(2f);
        //meter efeito power up + damage
        yield return new WaitForSeconds(5f);

        //reset timers
        lastAbilityTime = Time.time;
        lastComboTime = Time.time;
        lastMeleeTime = Time.time;
        transitioning = false;
        canMove = true;
        
    }

}
