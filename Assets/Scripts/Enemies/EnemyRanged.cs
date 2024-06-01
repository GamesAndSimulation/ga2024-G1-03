using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.TextCore.Text;
using UnityEngine.UIElements;

public class EnemyRanged : MonoBehaviour
{
    public GameObject projectile;
    public float fireforce;
    private GameObject player;

    void Start()
    {
        InvokeRepeating("FireProjectile", 0, 5);
        player = GameObject.FindGameObjectWithTag("Player");
        //
    }

    void FireProjectile()
    {
        var fireDirection = player.transform.position - transform.position;
        fireDirection.Normalize();
        fireDirection = new Vector3(fireDirection.x, 0, fireDirection.z);
        GameObject instantiatedBullet =
            Instantiate(projectile, transform.position + fireDirection, transform.rotation);
        instantiatedBullet.GetComponent<Rigidbody>().AddForce(fireDirection * fireforce);
        Destroy(instantiatedBullet, 5);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
