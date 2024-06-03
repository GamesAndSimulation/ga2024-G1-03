using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FPSDisplay : MonoBehaviour
{
    public TextMeshProUGUI fpsText;
    private bool isFPSVisible = false;
    private float nextUpdate = 0f;
    private float interval = 0.25f; 

    void Start()
    {

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            nextUpdate = Time.time;
            isFPSVisible = !isFPSVisible;
            fpsText.gameObject.SetActive(isFPSVisible);
        }

        if (isFPSVisible)
        {
            if(Time.time >= nextUpdate)
            {
                int fps = (int)(1f / Time.deltaTime);
                fpsText.text = fps + " FPS";

                nextUpdate += interval; 
            }
        }
    }
}
