using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    public Camera cinemachine;
    public GameObject freeCamera;
    public bool isFreeCameraActive = false;
    [SerializeField] private PlayerMovement playerMovement;

    private bool isPaused = false;

    void Start()
    {
        freeCamera.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U)) 
        {
            isFreeCameraActive = !isFreeCameraActive;

            if (!isFreeCameraActive)
            {
                freeCamera.SetActive(false);
                //cinemachine.gameObject.SetActive(true);
                cinemachine.enabled = true;
            }
            else
            {
                playerMovement.animator.SetFloat("Speed", 0f);
                playerMovement.animator2.SetFloat("Speed", 0f);
                freeCamera.transform.SetPositionAndRotation(cinemachine.transform.position, cinemachine.transform.rotation);
                freeCamera.SetActive(true);
                //cinemachine.gameObject.SetActive(false);
                cinemachine.enabled = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            isPaused = !isPaused;

            if (isPaused) Time.timeScale = 0f; 
            else Time.timeScale = 1f; 
        }
    }
}
