using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

public class NewCharacterZone : MonoBehaviour
{
    public TextMeshProUGUI pickupText;
    [SerializeField] private CharacterManager charManager;
    [SerializeField] private Transform vfxTransform;
    public Characters character;
    private bool inArea = false;

    void Start()
    {
        pickupText.enabled = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && pickupText.enabled){
            charManager.UnlockCharacter(character, vfxTransform);
            Destroy(transform.parent.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inArea = true;
            pickupText.enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inArea = false;
            pickupText.enabled = false;
        }
    }

}