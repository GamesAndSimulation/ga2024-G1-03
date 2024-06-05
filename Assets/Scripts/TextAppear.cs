using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class TextAppear : MonoBehaviour
{
    public TextMeshProUGUI pickupText;

    void Start()
    {
        pickupText.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            pickupText.enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            pickupText.enabled = false;
        }
    }

}