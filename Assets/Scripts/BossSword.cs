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
        GetComponent<AudioSource>().Play();
    }

    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("player hit");
        }
    }
}
