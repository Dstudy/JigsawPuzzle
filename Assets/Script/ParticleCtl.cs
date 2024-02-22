using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleCtl : MonoBehaviour
{
    public static ParticleCtl Instance;
    private new ParticleSystem particleSystem;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        particleSystem = GetComponent<ParticleSystem>();
    }
    
    public void PlayParticle()
    {
        particleSystem.Play();
    }
}
