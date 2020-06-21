using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Particles : MonoBehaviour
{
    //---Setup Variables---//
    public GameObject trailSystems;
    public GameObject bubbleSystems;

    //Start is called before the first frame update
    void Start()
    {
        trailSystems.SetActive(true);
        bubbleSystems.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Water")
        {

            
            trailSystems.SetActive(false);
            bubbleSystems.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Water")
        {

            trailSystems.SetActive(true);
            bubbleSystems.SetActive(false);
        }
    }
}
