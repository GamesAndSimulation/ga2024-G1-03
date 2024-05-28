using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeCamera : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float lookSpeed = 2f;
    [SerializeField] CameraManager cameraManager;

    void Update()
    {
        HandleMovement();
        HandleRotation();
    }

    void HandleMovement()
    {
        float h = Input.GetAxisRaw("Horizontal"); 
        float v = Input.GetAxisRaw("Vertical"); 

        Vector3 move = transform.right * h + transform.forward * v;
        transform.position += move * moveSpeed * Time.unscaledDeltaTime;

        if (Input.GetKey(KeyCode.Q)) 
        {
            transform.position += Vector3.down * moveSpeed * Time.unscaledDeltaTime;
        }

        if (Input.GetKey(KeyCode.E)) 
        {
            transform.position += Vector3.up * moveSpeed * Time.unscaledDeltaTime;
        }
    }

    void HandleRotation()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * lookSpeed;
        float mouseY = Input.GetAxisRaw("Mouse Y") * lookSpeed;

        Vector3 rotation = transform.localEulerAngles;
        rotation.y += mouseX;
        rotation.x -= mouseY;
        transform.localEulerAngles = rotation;
    }
}
