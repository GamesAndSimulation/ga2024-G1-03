using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public GameObject[] objectsToSpawn; // Array to hold the objects
    public GameObject door; // The door object
    private int objectsHit = 0;
    private dialogue dialogueScript;

    void Start()
    {
        dialogueScript = FindObjectOfType<dialogue>();

        // Ensure all objects are initially disabled
        foreach (GameObject obj in objectsToSpawn)
        {
            obj.SetActive(false);
        }

        // Initially close the door
        door.SetActive(false);
    }

    void Update()
    {
        if (dialogueScript.spawn)
        {
            SpawnObjects();
            dialogueScript.spawn = false; // Reset the flag to avoid repeated spawning
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
            OpenDoor();
        }
    }

    // Method to open the door
    void OpenDoor()
    {
        door.SetActive(true);
    }
}
