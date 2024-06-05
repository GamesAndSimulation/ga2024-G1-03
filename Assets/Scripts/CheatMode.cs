using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheatMode : MonoBehaviour
{
    
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M)){
            SceneManager.LoadScene("Level_01_The_Forest", LoadSceneMode.Single);
        }
        if (Input.GetKeyDown(KeyCode.N)){
            SceneManager.LoadScene("Mansion", LoadSceneMode.Single);
        }
        if (Input.GetKeyDown(KeyCode.B)){
           SceneManager.LoadScene("FinalBoss", LoadSceneMode.Single);
        }
    }
}
