using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroyExplosion : MonoBehaviour
{
    private ParticleSystem particleSystem;

    public void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
        Destroy(gameObject, 2.0f);
    }
}
