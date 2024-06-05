using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.TextCore.Text;
using UnityEngine.UIElements;

public class EnemyMotion : MonoBehaviour
{
    public float speed;
    private Vector3 direction;
    private GameObject player;

    void Start()
    {
        speed = speed / 10;
        InvokeRepeating("ChangeDirection", 0, 0.1f);
        player = GameObject.FindGameObjectWithTag("Player");
        //
    }

    void ChangeDirection()
    {
        direction = new Vector3((player.transform.position.x - transform.position.x) * speed * Time.deltaTime, 0, (player.transform.position.z - transform.position.z) * speed * Time.deltaTime);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += direction;
        Quaternion newRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, 4 * Time.deltaTime);

    }
}
