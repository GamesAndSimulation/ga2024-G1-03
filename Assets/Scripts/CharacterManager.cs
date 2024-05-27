using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using Image = UnityEngine.UI.Image;

public enum Characters
{
    Knight,
    Mage,
    Dwarf,
    TBD
}

public class CharacterManager : MonoBehaviour
{
    private float switchCost = 50f;
    private const int AmountCharacters = 3;
    private List<Characters> unlockedCharacters;
    public Characters current;  

    public List<GameObject> attacks;
    public List<GameObject> models;
    public GameObject helmet;
    [SerializeField] private GameObject swapVFX;
    [SerializeField] private Transform swapVFXPosition;
    [SerializeField] private PlayerCombat playerCombat;
    public Image[] characterSlots;
    public Sprite[] characterSprites;
    public ParticleSystem[] UIVfx;

    [SerializeField] private TextMeshProUGUI heightText;

    void Start()
    {
        unlockedCharacters = new List<Characters>
        {
            Characters.Knight,
            //Characters.Mage,
            //Characters.Dwarf
        };
        current = Characters.Knight;
    }

    void Update()
    {
        //can switch character only if its not in the middle of an attack
        if (playerCombat.stateInfo.IsName("Default") && !playerCombat.stateInfo2.IsName("DwarfAtk") && !playerCombat.animator.IsInTransition(1) /*&& !playerCombat.animator2.IsInTransition(0)*/){
            if (Input.GetKeyDown(KeyCode.Alpha1)) SwitchCharacter(unlockedCharacters[0]);
            if (Input.GetKeyDown(KeyCode.Alpha2)) {
                if (unlockedCharacters.Count > 1) SwitchCharacter(unlockedCharacters[1]);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3)) {

                if (unlockedCharacters.Count > 2) SwitchCharacter(unlockedCharacters[2]); 
            }
        }
    
    }

    public void SwitchCharacter(Characters character)
    {
        if (character == current) return;

        if(playerCombat.NoStaminaAlert(switchCost)) return;

        if (current == Characters.Dwarf && !HeightCheck())
        {
            StartCoroutine(playerCombat.FadeTextInAndOut(heightText, 2f));
            return;
        } 

        playerCombat.stamina -= switchCost;
        GameObject vfx = Instantiate(swapVFX, swapVFXPosition.position, swapVFXPosition.rotation);
        vfx.transform.SetParent(swapVFXPosition);
        Destroy(vfx, 1);

        if(current == Characters.Knight) helmet.SetActive(false);
        models[(int)current].SetActive(false);
        attacks[(int)current].SetActive(false);
        models[(int)character].SetActive(true);
        attacks[(int)character].SetActive(true);
        characterSlots[AmountCharacters].sprite = characterSprites[(int)character];
        if((int)character == 0) helmet.SetActive(true);
        current = character;

    }

    public void UnlockCharacter(Characters character, Transform transform)
    {
        GameObject vfx = Instantiate(swapVFX, transform.position, transform.rotation);
        Destroy(vfx, 1);

        unlockedCharacters.Add(character);
        characterSlots[unlockedCharacters.Count - 1].sprite = characterSprites[(int)character];
        UIVfx[unlockedCharacters.Count - 2].Play();
        characterSlots[unlockedCharacters.Count - 1].gameObject.SetActive(true);
    }

    //check if dwarf can switch to another character since he is smaller
    private bool HeightCheck()
    {
        Vector3 playerPosition = transform.position;
        
        float dwarfHeight = 1.0f; 
        float otherCharactersHeight = 2.0f; 
        float playerRadius = 0.5f; 

        Vector3 capsuleStart = playerPosition + Vector3.up * playerRadius;
        Vector3 capsuleEnd = playerPosition + Vector3.up * (dwarfHeight - playerRadius);

        RaycastHit hit;
        bool isSpaceAvailable = !Physics.CapsuleCast(capsuleStart, capsuleEnd, playerRadius, Vector3.up, out hit, otherCharactersHeight - dwarfHeight);

        return isSpaceAvailable;
    }



}