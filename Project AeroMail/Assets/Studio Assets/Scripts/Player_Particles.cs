using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Particles : MonoBehaviour
{
    //---Setup Variables---//
    private TrailRenderer[] trailSystems;
    private PlayerControllerScript pController;


    //---Particle Variables---//
    [Tooltip("Controls how long the trail is on speed (Higher the shorter)")]
    public float timeFraction = 1000.0f;

    //Start is called before the first frame update
    void Start()
    {
        trailSystems = GetComponentsInChildren<TrailRenderer>();
        pController = GetComponent<PlayerControllerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        trailSystems[0].time = pController.currentSpeed / timeFraction;
        trailSystems[1].time = pController.currentSpeed / timeFraction;
    }
}
