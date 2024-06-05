using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public GameObject[] objectsToSpawn;
    public GameObject barrier;
    public int objectsDestroyed = 0; // Public counter

    private dialogue dialogueScript;

    void Start()
    {
        dialogueScript = FindObjectOfType<dialogue>();
        InitializeObjects();
    }

    void Update()
    {
        HandleDialogueScript();
    }

    void InitializeObjects()
    {
        if (barrier != null)
        {
            barrier.SetActive(true);
        }
        foreach (GameObject obj in objectsToSpawn)
        {
            obj.SetActive(false);
        }
    }

    void HandleDialogueScript()
    {
        if (dialogueScript != null && dialogueScript.spawn)
        {
            SpawnObjects();
            Destroy(dialogueScript.gameObject);
        }
    }

    void SpawnObjects()
    {
        foreach (GameObject obj in objectsToSpawn)
        {
            obj.SetActive(true);
            obj.AddComponent<DestroyableObject>().objectSpawner = this; // Attach DestroyableObject script
        }
    }
}
