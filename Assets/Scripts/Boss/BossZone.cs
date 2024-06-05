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
<<<<<<< HEAD
        
=======
        audioSource = GetComponent<AudioSource>();
        audioSource.enabled = false;
        PreloadResources();

        EnableBoss();
        done = true;
>>>>>>> parent of 2ef63b8b (Revert "Merge branch 'Mansion-Level' into Forest-level")
    }

    void Update()
    {
        
    }

<<<<<<< HEAD
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
=======


    void PreloadResources()
    {
        if (audioSource.clip.loadState != AudioDataLoadState.Loaded)
        {
            audioSource.clip.LoadAudioData();
        }
    }

    void EnableBoss()
    {
        audioSource.enabled = true;
        StartCoroutine(EnableBossComponents());
    }

    IEnumerator EnableBossComponents()
    {
        yield return new WaitForSeconds(0.1f);
        boss.GetComponent<CharacterController>().enabled = true;
        boss.GetComponent<BossScript>().enabled = true;
        bossUI.SetActive(true);
    }
}
>>>>>>> parent of 2ef63b8b (Revert "Merge branch 'Mansion-Level' into Forest-level")
