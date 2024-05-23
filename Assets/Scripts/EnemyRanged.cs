using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.TextCore.Text;
using UnityEngine.UIElements;

public class EnemyRanged : EnemyScript
{
    public GameObject projectile;
    public float fireforce;
    private GameObject player;
    public Animator animator;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        animator = gameObject.GetComponent<Animator>();
        Action();
    }

    public override void Action()
    {
        InvokeRepeating("FireProjectile", 0, 5);
    }

    void FireProjectile()
    {
        animator.SetTrigger("Attack");
        StartCoroutine(DelayedProjectile());
    }

    IEnumerator DelayedProjectile()
    {
        yield return new WaitForSeconds(0.5f);
        var fireDirection = player.transform.position - transform.position;
        fireDirection.Normalize();
        fireDirection = new Vector3(fireDirection.x, 0, fireDirection.z);
        GameObject instantiatedBullet =
            Instantiate(projectile, transform.position + new Vector3(0, transform.localScale.y, 0) + fireDirection, transform.rotation);
        instantiatedBullet.GetComponent<Rigidbody>().AddForce(fireDirection * fireforce);
        Destroy(instantiatedBullet, 5);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
