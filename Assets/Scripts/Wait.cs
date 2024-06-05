using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wait : MonoBehaviour
{
    private int Time;
    public StartWithCharacter Mage;
    public StartWithCharacter Dwarf;
    // Start is called before the first frame update
    void Start()
    {
        Time = 100;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time > 0) {
            Time--;
        }
        else{
            Mage.enabled = true;
            Dwarf.enabled = true;
            Destroy(this);
        }
    }
}
