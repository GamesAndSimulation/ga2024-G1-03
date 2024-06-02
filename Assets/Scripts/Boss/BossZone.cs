using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossZone : MonoBehaviour
{
    public GameObject boss;
    public GameObject bossUI;
    private bool done = false;
    //private bool dialogDone = false;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !done)
        {
            // Dialog();
            GetComponent<AudioSource>().Play();
            GetComponent<Animator>().SetTrigger("Close");
            boss.GetComponent<CharacterController>().enabled = true;
            boss.GetComponent<BossScript>().enabled = true;
            bossUI.SetActive(true);
            done = true;
        }
    }
}
