using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeapEnemy : MonoBehaviour
{
    public CharacterController controller;
    public Transform player;
    //private float verticalVelocity;
    //private float gravityValue = 9.81f;
    public float walkSpeed = 3.0f; 
    public float leapSpeed = 7.0f; 
    public float leapInterval = 2.0f; 
    private float nextLeapTime = 2f;
    private Animator animator;
    private bool isLeaping = false;
    private bool canMove = true;
    private Vector3 leapDirection;
    private float distanceToPlayer;
    private float minLeapSpeed = 2f;
    private float maxLeapSpeed = 10f;
    private float minimumChaseDistance = 3f;
    private float startChaseDistance = 4f;
    private bool isChasing = false;
    private Vector3 move;
    private Vector3 direction;
    public GameObject attackVFX;
    public Transform attackVFXPos;

    void Start()
    {
        controller = gameObject.GetComponentInParent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        /*bool groundedPlayer = controller.isGrounded;

        if (groundedPlayer && verticalVelocity < 0)
        {
            verticalVelocity = 0f;
        }

        verticalVelocity -= gravityValue * Time.deltaTime;*/
        distanceToPlayer = Vector3.Distance(transform.position, player.position);
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
            
            float speed = isLeaping ? leapSpeed : walkSpeed;
            
            if (isLeaping)
            {
                move = leapDirection * speed;
            }
            else
            {
                move = new Vector3(direction.x, 0, direction.z) * speed;
                gameObject.transform.forward = move;
            }

            
            controller.Move(move * Time.deltaTime);
        }
        animator.SetFloat("speed", move.magnitude);

        if (Time.time >= nextLeapTime)
        {
            nextLeapTime = Time.time + leapInterval;
            StartCoroutine(Leap());
        }
    }

    IEnumerator Leap()
    {
        canMove = false;
        animator.SetTrigger("leap");

        yield return new WaitForSeconds(0.5f);
        gameObject.transform.forward = direction;

        //min-max'd distance to be leap speed
        float scaledLeapSpeed = Mathf.Lerp(minLeapSpeed, maxLeapSpeed, (distanceToPlayer - 3f) / (10f - 3f));
        leapSpeed = scaledLeapSpeed;

        //lock direction for the leap
        leapDirection = transform.forward;
        isLeaping = true;
        canMove = true;

        float leapDuration = 1.0f;
        yield return new WaitForSeconds(leapDuration);

        GameObject vfx = Instantiate(attackVFX, attackVFXPos.position, Quaternion.Euler(0, 0, 0));
        Destroy(vfx, 1.5f);

        isLeaping = false;
        canMove = false;

        yield return new WaitForSeconds(2.1f);
        canMove = true;
        gameObject.transform.forward = direction;
    }
}
