using UnityEngine;

public class ObjectInteraction : MonoBehaviour
{
    private ObjectSpawner objectSpawner;
    private EnemyHealth enemyHealth;

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

    void Update()
    {
        CheckObjectHealth();
    }

    void CheckObjectHealth()
    {
        // If enemy health is zero or less, handle object hit
        if (enemyHealth != null && enemyHealth.health <= 0)
        {
            HandleObjectHit();
        }
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
