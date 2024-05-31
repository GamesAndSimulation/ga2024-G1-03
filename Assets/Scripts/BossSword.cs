using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSword : MonoBehaviour
{
    public int damage = 20; 
    private Animator animator;
    private AnimatorStateInfo stateInfo;

    void Start()
    {
        var bossController = FindObjectOfType<BossScript>();
        animator = bossController.animator;

        GetComponent<AudioSource>().clip = bossController.swordClip;
        GetComponent<AudioSource>().Play();
    }

    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (other.CompareTag("Player") && stateInfo.IsName("Melee"))
        {
            Debug.Log("player hit");
        }
    }
}
