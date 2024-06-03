using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public GameObject[] objectsToSpawn;
    public GameObject raisedBridge;

    public GameObject loweredBridge;

    public GameObject barrier;

    private dialogue dialogueScript;

    void Start()
    {
        dialogueScript = FindObjectOfType<dialogue>();


        foreach (GameObject obj in objectsToSpawn)
        {
            obj.SetActive(false);
        }


        loweredBridge.SetActive(false);
    }

    void Update()
    {
        if(dialogueScript != null)
        {
            if (dialogueScript.spawn)
            {
                SpawnObjects();
                Destroy(dialogueScript.gameObject);
            }
        }
        CheckObjectsDestroyed();
    }

    // Method to spawn the objects
    void SpawnObjects()
    {
        foreach (GameObject obj in objectsToSpawn)
        {
            obj.SetActive(true);
        }
    }

    void CheckObjectsDestroyed()
    {
        bool allDestroyed = true;
        foreach (GameObject obj in objectsToSpawn)
        {
            if (obj != null && obj.activeInHierarchy)
            {
                allDestroyed = false;
                break;
            }
        }

        if (allDestroyed)
        {
            LowerBridge();
        }
    }

    // Method to open the door
    void LowerBridge()
    {
        Debug.Log("LowerBridge called");
        loweredBridge.SetActive(true);
        raisedBridge.SetActive(false);
        barrier.SetActive(false);
    }
}
