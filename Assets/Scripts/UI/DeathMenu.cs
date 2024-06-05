using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Image = UnityEngine.UI.Image;

public class DeathMenu : MonoBehaviour
{
    public GameObject deathMenuUI; 
    public Image blood;
    public Image background;

    void Start()
    {
    }

    void Update()
    {
      /* if (!isDone)
       {
            StartCoroutine(BloodImage());
            isDone = true;
       }*/
    }


    public void Restart()
    {
        //SceneManager.LoadScene("Level_01_The_Forest");
        SceneManager.LoadScene("IntroCutscene", LoadSceneMode.Single);
    }

    
    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        // Application.Quit();
    }

    public IEnumerator BloodImage()
    {
        Color color1 = blood.color;
        color1.a = 0;
        blood.color = color1;
        Color color2 = background.color;
        color2.a = 0;
        background.color = color2;

        float elapsedTime = 0;

        while (elapsedTime < 2f)
        {
            elapsedTime += Time.deltaTime;
            color1.a = Mathf.Clamp01(elapsedTime / 2f) * 0.8f;
            color2.a = Mathf.Clamp01(elapsedTime / 2f) * 0.8f;
            blood.color = color1;
            background.color = color2;
            yield return null;
        }

        color1.a = 0.8f; 
        color2.a = 0.8f; 
        blood.color = color1;
        background.color = color2;
    }
}