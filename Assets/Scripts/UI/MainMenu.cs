using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenuUI; 
    public bool hasStarted = false;
    

    void Start()
    {
        Time.timeScale = 0f; 
        hasStarted = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void Update()
    {
       
    }

    public void StartGame()
    {
        mainMenuUI.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        SceneManager.LoadScene("IntroCutscene", LoadSceneMode.Single);
        Time.timeScale = 1f;
        hasStarted = true; 
    }

    
    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        // Application.Quit();
    }
}