using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VentScript : MonoBehaviour
{
    //---Property Variables---//
    [Header("Vent Properties")]
    [Tooltip("Controls how hard the vent pushs the plane")]
    public float ventPower = 50.0f;
    [Tooltip("Controls how fast the particles moves")]
    public float ventSpeed = 2.5f;


    private ParticleSystem pSystem;

    private void Awake()
    {
        pSystem = GetComponentInChildren<ParticleSystem>();
        var main = pSystem.main;
        main.startSpeed = ventPower * ventSpeed;
        //Makes the particles fit the triggerbox
        main.startLifetime = (1.25f * (125.0f / ventPower))/2.0f; 
    }
}
