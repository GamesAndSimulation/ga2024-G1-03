using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class PlayerScript : MonoBehaviour
{
    private CharacterController controller;
    private float verticalVelocity;
    private float groundedTimer;        // to allow jumping when going down ramps
    private float playerSpeed = 2.0f;
    private float jumpHeight = 1.0f;
    private float gravityValue = 9.81f;
 
    private void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
    }
 
    void Update()
    {
        bool groundedPlayer = controller.isGrounded;
        if (groundedPlayer)
        {
            // cooldown interval to allow reliable jumping even whem coming down ramps
            groundedTimer = 0.2f;
        }
        if (groundedTimer > 0)
        {
            groundedTimer -= Time.deltaTime;
        }
 
        // slam into the ground
        if (groundedPlayer && verticalVelocity < 0)
        {
            verticalVelocity = 0f;
        }
 
        // apply gravity always, to let us track down ramps properly
        verticalVelocity -= gravityValue * Time.deltaTime;

        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        move *= playerSpeed;
 
        // only align to motion if we are providing enough input
        if (move.magnitude > 0.05f)
        {
            gameObject.transform.forward = move;
        }

        if (Input.GetButton("Jump"))
        {
            // must have been grounded recently to allow jump
            if (groundedTimer > 0)
            {
                groundedTimer = 0;
                verticalVelocity += Mathf.Sqrt(jumpHeight * 2 * gravityValue);
            }
        }

        move.y = verticalVelocity;
        controller.Move(move * Time.deltaTime);
    }
}
