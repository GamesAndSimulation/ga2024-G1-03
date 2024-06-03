using UnityEngine;

public class Jumpscare : MonoBehaviour
{
    public Transform characterModel; // The character model to move
    public float speed = 20f; // Speed of the jumpscare movement
    public Vector3 startPoint; // Starting position
    public Vector3 endPoint; // Ending position
    public float waitTimeBeforeStart = 2f; // Time before the jumpscare starts
    public float waitTimeAfterEnd = 2f; // Time to wait after the jumpscare ends
    public AudioClip jumpscareSound; // Sound to play during the jumpscare

    private AudioSource audioSource;
    private bool jumpscareTriggered = false;

    void Start()
    {
        if (characterModel == null)
        {
            Debug.LogError("Character Model is not assigned.");
            return;
        }

        characterModel.position = startPoint; 

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Update()
    {
        if (jumpscareTriggered) 
        {
            PerformJumpscare();
        }
    }

    public void PerformJumpscare()
    {
        

        // Play the jumpscare sound
        if (jumpscareSound != null)
        {
            audioSource.PlayOneShot(jumpscareSound);
        }

        // Move the character model from start to end
        float startTime = Time.time;
        float journeyLength = Vector3.Distance(startPoint, endPoint);
        while (Vector3.Distance(characterModel.position, endPoint) > 0.1f)
        {
            float distCovered = (Time.time - startTime) * speed;
            float fractionOfJourney = distCovered / journeyLength;
            characterModel.position = Vector3.Lerp(startPoint, endPoint, fractionOfJourney);

        }

        characterModel.position = endPoint;

        characterModel.gameObject.SetActive(false);
    }
}
