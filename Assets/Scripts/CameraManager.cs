using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    public CinemachineVirtualCamera cinemachine;
    public GameObject freeCamera;
    public bool isFreeCameraActive = false;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerCombat playerCombat;
    private bool isPaused = false;

    void Start()
    {
        freeCamera.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false; 
    }

    void Update()
    {
       // if (isFreeCameraActive) playerMovement.move = new Vector3(0, 0, 0);
        //Debug.Log(playerMovement.move);
        if (Input.GetKeyDown(KeyCode.U) /*&& !playerMovement.isRolling && !playerCombat.dwarfAttack*/) 
        {
            isFreeCameraActive = !isFreeCameraActive;

            if (!isFreeCameraActive)
            {
                freeCamera.SetActive(false);
                cinemachine.gameObject.SetActive(true);
            }
            else
            {
                playerMovement.animator.SetFloat("Speed", 0f);
                playerMovement.animator2.SetFloat("Speed", 0f);
                freeCamera.transform.SetPositionAndRotation(cinemachine.transform.position, cinemachine.transform.rotation);
                freeCamera.SetActive(true);
                cinemachine.gameObject.SetActive(false);
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
