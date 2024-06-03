using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public GameObject[] objectsToSpawn;
    public GameObject raisedBridge;

    public GameObject loweredBridge;

    public GameObject barrier;
    private int objectsHit = 0;
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
        if(dialogueScript != null){
            if (dialogueScript.spawn)
            {
                SpawnObjects();
                Destroy(dialogueScript.gameObject);
            }
        }
    }

    // Method to spawn the objects
    void SpawnObjects()
    {
        foreach (GameObject obj in objectsToSpawn)
        {
            obj.SetActive(true);
        }
    }

    // This method will be called when an object is hit
    public void ObjectHit()
    {
        objectsHit++;
        if (objectsHit >= objectsToSpawn.Length)
        {
            LowerBridge();
        }
    }

    // Method to open the door
    void LowerBridge()
    {
        loweredBridge.SetActive(true);
        raisedBridge.SetActive(false);
        barrier.SetActive(false);
    }
}
