using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class OpenDoor : MonoBehaviour
{
    public TextMeshProUGUI pickupText;
    public GameObject doorLeft;
    public GameObject doorRight;

    void Start()
    {
        pickupText.enabled = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && pickupText.enabled){
            doorLeft.transform.eulerAngles = new Vector3(0, 180, 0);
            doorRight.transform.eulerAngles = new Vector3(0, 180, 0);
            Destroy(gameObject);
        }
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