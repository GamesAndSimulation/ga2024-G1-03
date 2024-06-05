using UnityEngine;

public class ActivateScript : MonoBehaviour
{
    public GameObject[] objects;

    void Start()
    {

    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            foreach (GameObject obj in objects)
            {
                var scripts = obj.GetComponents<EnemyScript>();

                foreach (EnemyScript script in scripts)
                {
                    script.enabled = true;

                }
            }
            Destroy(this);
        }
    }
}
