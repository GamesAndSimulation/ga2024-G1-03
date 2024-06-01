using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class CrystalScript: MonoBehaviour
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
        transform.eulerAngles += new Vector3(0, 1f, 0);
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Magic"))
        {
            for (int i = 0; i < move.Length; i++)
            {
                GameObject current = move[i];
                if (i < newPosition.Length)
                {
                    current.transform.position = newPosition[i];
                }
            }
            Destroy(gameObject);
        }
    }

 }
