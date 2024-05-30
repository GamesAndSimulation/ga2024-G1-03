using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI; 
    private bool isPaused = false;

    void Update()
    {
       /* if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }*/
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1f; 
        isPaused = false;
    }

    public void Pause()
    {
        //if (!player.isDead){
          //  player.audioSource.Stop();
            //player.isDead = true;
            pauseMenuUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0f; 
            isPaused = true;
        //}
    }

    public void Restart()
    {
        pauseMenuUI.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1f; 
        SceneManager.LoadScene("Level_01_The_Forest");
    }

    
    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        // Application.Quit();
    }
}