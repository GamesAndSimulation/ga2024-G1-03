using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossZone : MonoBehaviour
{
    public GameObject boss;
    public GameObject bossUI;
    private bool done = false;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.enabled = false;
        PreloadResources();
    }

    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !done)
        {
            // Dialog();
            EnableBoss();
            done = true;
        }
    }

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
        GetComponent<Animator>().SetTrigger("Close");
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