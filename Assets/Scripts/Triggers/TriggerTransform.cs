using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class TriggerTransform: MonoBehaviour
{
    public GameObject[] delete;
    public GameObject[] move;
    public string triggerTag;

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
        if (collision.gameObject.tag.Equals(triggerTag))
        {
            for (int i = 0; i < delete.Length; i++)
            {
                //
                GameObject current = delete[i];
                if (i < move.Length)
                {
                    move[i].transform.position = current.transform.position;
                }
                Destroy(current);
            }
            Destroy(this);
        }
        
    }

 }
