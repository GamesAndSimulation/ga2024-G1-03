using UnityEngine;

public class DestroyableObject : MonoBehaviour
{
    public ObjectSpawner objectSpawner;
    private bool isDestroyed = false;

    void OnDestroy()
    {
        if (!isDestroyed && transform.parent == null)
        {
            isDestroyed = true;
            if (objectSpawner != null)
            {
                objectSpawner.objectsDestroyed++;
            }
        }
    }
}
