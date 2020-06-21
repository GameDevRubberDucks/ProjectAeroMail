using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Camera_Collider : MonoBehaviour
{
    public GameObject postProcessing;
    public Volume postPro;
    //public TextureParameter[] lookUp;

    // Start is called before the first frame update


    void Start()
    {
        //postPro.GetComponent<ColorLookup>().texture = lookUp[0];
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag=="Water")
        {
            //postProcessing.SetActive(true);
            postProcessing.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Water")
        {
            //postProcessing.SetActive(false);
            postProcessing.gameObject.SetActive(false);
        }
    }
}
