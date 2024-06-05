using UnityEngine;

public class BridgeTrigger : MonoBehaviour
{
    public int requiredDestroyedObjects = 0; // The number of objects that need to be destroyed
    public GameObject raisedBridge;
    public GameObject loweredBridge;
    public GameObject barrier;
    public ObjectSpawner objectSpawner;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && objectSpawner.objectsDestroyed >= requiredDestroyedObjects)
        {
            LowerBridge();
        }
    }

    void LowerBridge()
    {
        if (raisedBridge != null)
        {
            raisedBridge.SetActive(false);
        }
        if (loweredBridge != null)
        {
            loweredBridge.SetActive(true);
        }
        if (barrier != null)
        {
            barrier.SetActive(false);
        }
    }
}
