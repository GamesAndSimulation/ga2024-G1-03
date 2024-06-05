using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class DestroyWall: MonoBehaviour
{
    private bool pickupText;
    public GameObject wall;

    void Start()
    {
        pickupText = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && pickupText)
        {
            wall.SetActive(false);
            Destroy(wall);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            pickupText = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            pickupText = false;
        }
    }

}