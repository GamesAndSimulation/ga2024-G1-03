using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller;
    private float verticalVelocity;
    private float groundedTimer;     //to allow rolling when going down ramps
    public float walkSpeed = 2.0f;
    public float runSpeed = 3.0f;
    public float jumpHeight = 1.0f;
    public float gravityValue = 9.81f;
    private float rollSpeedMultiplier = 2f; 
    private float rollDuration = 1f;        
    public Animator animator;
    public bool isRolling = false;
    private float rollTimer = 0f;
    private Vector3 rollDirection;
    private float storedSpeed;

    void Start()
    {
        controller = gameObject.GetComponentInParent<CharacterController>();
        animator = gameObject.GetComponent<Animator>();
    }

    void Update()
    {
        bool groundedPlayer = controller.isGrounded;
        if (groundedPlayer)
        {
            //cooldown interval to allow reliable rolling even when coming down ramps
            groundedTimer = 0.2f;
        }
        if (groundedTimer > 0)
        {
            groundedTimer -= Time.deltaTime;
        }

        //slam into the ground
        if (groundedPlayer && verticalVelocity < 0)
        {
            verticalVelocity = 0f;
        }

        //apply gravity
        verticalVelocity -= gravityValue * Time.deltaTime;

        Vector3 move;

        ClickRoll();

        if (!isRolling) //pnly update movement direction if not rolling
        {
            move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            if (move.magnitude > 1)
            {
                move.Normalize(); //normalize the move vector to ensure consistent speed
            }
            if (Input.GetKey(KeyCode.LeftShift))
            {
                move *= runSpeed;
            }
            else
            {
                move *= walkSpeed;
            }
            animator.SetFloat("Speed", move.magnitude);

            //only align to motion if we are providing enough input
            if (move.magnitude > 0.3f)
            {
                gameObject.transform.forward = move;
            }
        }
        else
        {
            Roll();
            move = rollSpeedMultiplier * storedSpeed * rollDirection;
        }

        move.y = verticalVelocity;
        controller.Move(move * Time.deltaTime);
    }

    void ClickRoll()
    {
        if (Input.GetButton("Jump") && !isRolling && groundedTimer > 0)
        {
            animator.SetTrigger("Roll");
            isRolling = true;
            rollTimer = rollDuration;
            rollDirection = gameObject.transform.forward; 
            storedSpeed = controller.velocity.magnitude;  
        }
    }

    void Roll()
    {
        rollTimer -= Time.deltaTime;
        if (rollTimer <= 0)
        {
            isRolling = false;
        }

        if (storedSpeed < 0.5f)
        {
            storedSpeed = 1.0f;
            if (rollTimer > 0.8f)
            {
                rollSpeedMultiplier = 0f;
            }
        }
        if (rollTimer <= 0.5f)
        {
            rollSpeedMultiplier = 1.5f;
        }
        else if (rollTimer <= 0.8f && rollTimer >= 0.5f)
        {
            rollSpeedMultiplier = 2.5f;
        }
    }
}


