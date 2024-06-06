
using UnityEngine;

public class TriggerMove: MonoBehaviour
{
    public GameObject[] move;
    public Vector3[] newPosition;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Objectives"))
        {
            for (int i = 0; i < move.Length; i++)
            {
                //
                GameObject current = move[i];
                if (i < newPosition.Length)
                {
                    current.transform.position = newPosition[i];
                }
            }
        }
        Destroy(this);
    }

 }
