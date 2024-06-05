using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class StartWithCharacter : MonoBehaviour
{

    [SerializeField] private CharacterManager charManager;
    [SerializeField] private Transform vfxTransform;
    public Characters character;

    // Start is called before the first frame update
    void Start()
    {
        print(charManager);
        print(character);
        print(vfxTransform);
        charManager.UnlockCharacter(character, vfxTransform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
