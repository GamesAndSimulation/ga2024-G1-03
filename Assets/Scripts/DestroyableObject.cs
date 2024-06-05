using UnityEngine;

public class DestroyableObject : MonoBehaviour
{
    public ObjectSpawner objectSpawner;
    private bool hasBeenDestroyed = false;

    void OnDestroy()
    {
        Debug.Log("Object destroyed: " + gameObject.name);
        if (transform.parent != null && transform.parent.CompareTag("Objectives"))
        {
            if (objectSpawner != null)
            {
                objectSpawner.IncrementDestroyedObjectsCount();
            }
        }
    }
}
