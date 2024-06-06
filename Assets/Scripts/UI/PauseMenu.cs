using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI; 
    public bool isPaused;
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        FindObjectOfType<PlayerCombat>().isDead = false;
        pauseMenuUI.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1f; 
        isPaused = false;
    }

    public void Pause()
    {
        if (!FindObjectOfType<PlayerCombat>().isDead){
            FindObjectOfType<PlayerCombat>().isDead = true;
            pauseMenuUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0f; 
            isPaused = true;
        }
    }

    public void Restart()
    {
        //SceneManager.LoadScene("Level_01_The_Forest");
        SceneManager.LoadScene("IntroCutscene", LoadSceneMode.Single);
    }

    
    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }
}