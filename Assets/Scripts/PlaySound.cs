using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlaySound : MonoBehaviour
{
    public AudioSource[] audios;
    public String triggerTag;

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag(triggerTag))
        {
            for (int i = 0; i < audios.Length; i++)
            {
                audios[i].Play();
            }
        }
    }

}
