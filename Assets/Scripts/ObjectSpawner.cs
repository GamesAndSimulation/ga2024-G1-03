using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public GameObject[] objectsToSpawn;
    public GameObject barrier;

    private GameObject raisedBridge;
    private GameObject loweredBridge;
    private dialogue dialogueScript;
    private int objectsToDestroy;
    private GameObject RaisedBridge;
    private GameObject LoweredBridge;

    void Start()
    {
        InitializeComponents();
        InitializeBridgeAndBarrierStates();
        SetObjectsToSpawnInactive();
        objectsToDestroy = objectsToSpawn.Length; // Initialize the counter
    }

    void Update()
    {
        HandleDialogueScript();
        CheckIfAllObjectsDestroyed();
    }

    
    void InitializeComponents()
    {
        dialogueScript = FindObjectOfType<dialogue>();

        raisedBridge = GameObject.Find("RaisedBridge");
        loweredBridge = GameObject.Find("LoweredBridge");
    }

    
    void InitializeBridgeAndBarrierStates()
    {
        if (raisedBridge != null)
        {
            raisedBridge.SetActive(true);
        }
        if (loweredBridge != null)
        {
            loweredBridge.SetActive(false);
        }
        if (barrier != null)
        {
            barrier.SetActive(true);
        }
    }

    
    void SetObjectsToSpawnInactive()
    {
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
        }
    }

   
    void CheckIfAllObjectsDestroyed()
    {
        int activeObjectsCount = 0;
        foreach (GameObject obj in objectsToSpawn)
        {
            if (obj != null && obj.activeInHierarchy)
            {
                activeObjectsCount++;
            }
        }

       
        if (activeObjectsCount == 0 && objectsToDestroy > 0)
        {
            objectsToDestroy = 0; 
            LowerBridge();
        }
    }

    void LowerBridge()
    {
        Debug.Log("LowerBridge called");
        if (loweredBridge != null)
        {
            loweredBridge.SetActive(true);
        }
        if (raisedBridge != null)
        {
            raisedBridge.SetActive(false);
        }
        if (barrier != null)
        {
            barrier.SetActive(false);
        }
    }
}
