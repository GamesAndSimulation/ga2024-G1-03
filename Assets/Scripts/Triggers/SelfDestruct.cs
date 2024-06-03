using UnityEngine;
using System;
using UnityEngine.Events;

public class SelfDestruct : MonoBehaviour
{
    [SerializeField] float lifetime;

    void OnEnable()
    {
        // Schedule the object to be destroyed after 'lifetime' seconds
        Destroy(gameObject, lifetime);
    }
}
