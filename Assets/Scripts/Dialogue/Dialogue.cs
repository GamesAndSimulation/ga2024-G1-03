using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour
{
    //UI

    [SerializeField]
    private GameObject dialogueCanvas;

    [SerializeField]
    private TMP_Text speakerText;

    [SerializeField]
    private TMP_Text dialogueText;

    [SerializeField]
    private Image portraitImage;

    //Dialogue

    [SerializeField]
    private string[] speaker;

    [SerializeField]
    [TextArea]
    private string[] dialogueWords;

    [SerializeField]
    private Sprite[] portrait;

    [SerializeField]
    private bool loopable;

    [SerializeField]
    private bool destroyable;

    private bool dialogueActivated;

    public bool dialogueFinished;

    private int step;

    [SerializeField]
    private bool spawnsObjects;

  
    public bool spawn = false;



    void Update()
    {
     if(Input.GetKeyDown(KeyCode.E) && dialogueActivated == true)
     {
         if(step >= speaker.Length)
         {
             dialogueCanvas.SetActive(false);
             step = 0;
             if(!loopable){
                 dialogueFinished = true;
             }
             if(spawnsObjects){
                 spawn = true;
             }
         }
         else
         {
             dialogueCanvas.SetActive(true);

             speakerText.text = speaker[step];
             dialogueText.text = dialogueWords[step];
             portraitImage.sprite = portrait[step];

             step += 1;
         }
     }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            dialogueActivated = true;
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        dialogueActivated = false;
        dialogueCanvas.SetActive(false);
    }
}
