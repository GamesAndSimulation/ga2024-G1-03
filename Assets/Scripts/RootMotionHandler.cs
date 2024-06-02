using UnityEngine;

public class RootMotionHandler : MonoBehaviour
{
    private Animator animator;
    private CharacterController characterController;

    private void Start()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
    }

    private void OnAnimatorMove()
    {
        if (FindObjectOfType<PlayerCombat>().dwarfAttack)
        {
            if (animator && characterController)
            {
            
                Vector3 rootMotionDelta = animator.deltaPosition;
                characterController.Move(rootMotionDelta);
                transform.rotation *= animator.deltaRotation;
            }
        }
    }
}

