using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossZone : MonoBehaviour
{
    public GameObject boss;
    public GameObject bossUI;
    private bool done = false;
 

    void Start()
    {

    }

    void Update()
    {
        
    }



    /*void PreloadResources()
    {
        if (audioSource.clip.loadState != AudioDataLoadState.Loaded)
        {
            audioSource.clip.LoadAudioData();
        }
    }*/

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !done)
        {
            boss.GetComponent<CharacterController>().enabled = true;
            boss.GetComponent<BossScript>().enabled = true;
            bossUI.SetActive(true);
            done = true;
        }
    }

}