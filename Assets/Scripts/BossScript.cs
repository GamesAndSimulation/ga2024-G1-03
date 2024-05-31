using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossScript : MonoBehaviour
{
    public CharacterController controller;
    [SerializeField] private GameObject swordsPrefab;
    [SerializeField] private Transform swordsSpawn;
    [SerializeField] private Transform sword;
    [SerializeField] private AudioSource swordsAudio;
    private Animator animator;
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
    private float leapSpeed; 
    private float nextLeapTime = 5f;
    private float leapInterval = 50f; 


    void Start()
    {
        controller = gameObject.GetComponentInParent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {

        distanceToPlayer = Vector3.Distance(transform.position, player.position);
        direction = (player.position - transform.position).normalized;

        if (Input.GetKeyDown(KeyCode.L))
        {
            StartCoroutine(SpinSwords());
        }

        move = new(0,0,0);
        direction = (player.position - transform.position).normalized;

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
            
           // float speed = isLeaping ? leapSpeed : walkSpeed;
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

        if (Time.time >= nextLeapTime)
        {
            nextLeapTime = Time.time + leapInterval;
            StartCoroutine(Leap());
        }
    }

    private IEnumerator SpinSwords()
    {

        canMove = false;
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
    }

    private IEnumerator Leap()
    {
        canMove = false;
        gameObject.transform.forward = direction;
        animator.SetTrigger("Leap");

        yield return new WaitForSeconds(0.5f);
        //gameObject.transform.forward = direction;

        //min-max'd distance to be leap speed
        float scaledLeapSpeed = Mathf.Lerp(1f, 7f, (distanceToPlayer - 2f) / (7f - 2f));
        leapSpeed = scaledLeapSpeed;

        //lock direction for the leap
        leapDirection = transform.forward;
        isLeaping = true;
        canMove = true;

        float leapDuration = 1.5f;
        yield return new WaitForSeconds(leapDuration);

        GameObject vfx = Instantiate(shockwaveVFX, shockwavePos.position, shockwavePos.rotation);
        Destroy(vfx, 4f);
        
        GameObject rock = Instantiate(rocks, rocksSpawn, false);
        rock.transform.SetParent(null);
        Destroy(rock, 5f);
        isLeaping = false;
        canMove = false;

        yield return new WaitForSeconds(5f);
        canMove = true;
        //gameObject.transform.forward = direction;
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

}
