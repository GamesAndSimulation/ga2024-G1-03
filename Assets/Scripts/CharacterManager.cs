using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    private List<Characters> unlockedCharacters;
    private int current;
    private enum Characters
    {
        Knight,
        Mage,
        Dwarf,
        TBD
    }

    public List<GameObject> models;
    public GameObject helmet;

    void Start()
    {
        unlockedCharacters = new List<Characters>
        {
            Characters.Knight,
            Characters.Mage
        };
        current = 0;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) SwitchCharacter(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SwitchCharacter(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) SwitchCharacter(2); 
    }

    public void SwitchCharacter(int index)
    {
        if (index < 0 || index >= unlockedCharacters.Count || index == current) return;

        if(current == 0) helmet.SetActive(false);
        models[current].SetActive(false);
        models[index].SetActive(true);
        if(index == 0) helmet.SetActive(true);
        current = index;

    }

    public void UnlockCharacter(int index)
    {
        unlockedCharacters.Add((Characters)index);
    }

}