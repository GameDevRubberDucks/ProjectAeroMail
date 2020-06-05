using UnityEngine;
using UnityEngine.UI;

public class Delivery_UI : MonoBehaviour
{
    //--- Public Variables ---//
    [Header("Target Camera UI")]
    public Animator m_animZoneCamView;



    //--- Private Variables ---//
    private Delivery_Controller m_deliveryController;



    //--- Unity Methods ---//
    private void Awake()
    {
        // Init the private variables
        m_deliveryController = GameObject.FindObjectOfType<Delivery_Controller>();

        // Hook into the delivery controller's target zone change event
        // This way, we can update the UI anytime the target has changed
        m_deliveryController.OnTargetZoneChanged.AddListener(this.OnNewTargetZone);
    }

    private void Update()
    {
        // Press the spacebar to show the target image temporarily before having it disappear
        if (Input.GetKeyDown(KeyCode.Space))
            ShowTargetImage();
    }



    //--- Methods ---//
    public void OnNewTargetZone(Delivery_Zone _newZone)
    {
        // Get the zone's camera so we can set it up correctly
        Camera zoneCam = _newZone.m_zoneCamera;

        Debug.Log(_newZone.gameObject.name);
    }

    public void ShowTargetImage()
    {
        // Can only reset the animation if in the end state and not in any transtitions to / from it
        bool canTriggerAnim = m_animZoneCamView.GetCurrentAnimatorStateInfo(0).IsName("DeliveryZoneImage_Exit");
        canTriggerAnim = canTriggerAnim && !m_animZoneCamView.IsInTransition(0);

        // Trigger the photo's animation cycle again to show the photo on the UI
        if (canTriggerAnim)
            m_animZoneCamView.SetTrigger("ShowPhoto");
    }
}
