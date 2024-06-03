using UnityEngine;

public class ObjectInteraction : MonoBehaviour
{
    private ObjectSpawner objectSpawner;

    [SerializeField] private AudioClip hitSFX;
    [SerializeField] private GameObject hitFX; 

    private AudioSource audioSource;

    void Start()
    {
        objectSpawner = FindObjectOfType<ObjectSpawner>();
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void OnTriggerEnter(Collider other)
    {
       HandleObjectHit();
    }

    void HandleObjectHit()
    {
        // Play sound effect
        if (hitSFX != null)
        {
            audioSource.PlayOneShot(hitSFX);
        }

        // Spawn visual effect
        if (hitFX != null)
        {
            Instantiate(hitFX, transform.position, transform.rotation);
        }

        // Notify the object spawner that this object has been hit
        objectSpawner.ObjectHit();

        // Disable the object after it has been hit
        gameObject.SetActive(false);
    }
}
