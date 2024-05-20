using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum Characters
{
    Knight,
    Mage,
    Dwarf,
    TBD
}

public class CharacterManager : MonoBehaviour
{
    private List<Characters> unlockedCharacters;
    public Characters current;


    public List<GameObject> attacks;
    public List<GameObject> models;
    public GameObject helmet;
    [SerializeField] private GameObject swapVFX;
    [SerializeField] private Transform swapVFXPosition;

    [SerializeField] private PlayerCombat playerCombat;

    void Start()
    {
        unlockedCharacters = new List<Characters>
        {
            Characters.Knight,
            Characters.Dwarf,
            Characters.Mage
        };
        current = 0;
    }

    void Update()
    {
        //can switch character only if its not in the middle of an attack
        if (playerCombat.stateInfo.IsName("Default") && !playerCombat.animator.IsInTransition(1)){
            if (Input.GetKeyDown(KeyCode.Alpha1)) SwitchCharacter(unlockedCharacters[0]);
            if (Input.GetKeyDown(KeyCode.Alpha2)) SwitchCharacter(unlockedCharacters[1]);
            if (Input.GetKeyDown(KeyCode.Alpha3)) SwitchCharacter(unlockedCharacters[2]); 
        }
    }

    public void SwitchCharacter(Characters character)
    {
        if (character == current) return;
        GameObject vfx = Instantiate(swapVFX, swapVFXPosition.position, swapVFXPosition.rotation);
        vfx.transform.SetParent(swapVFXPosition);
        Destroy(vfx, 1);

        if(current == Characters.Knight) helmet.SetActive(false);
        models[(int)current].SetActive(false);
        attacks[(int)current].SetActive(false);
        models[(int)character].SetActive(true);
        attacks[(int)character].SetActive(true);
        if((int)character == 0) helmet.SetActive(true);
        current = character;

    }

    public void UnlockCharacter(Characters character)
    {
        unlockedCharacters.Add(character);
    }

}