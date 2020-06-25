using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Delivery_UI : MonoBehaviour
{
    //--- Public Variables ---//
    [Header("Target Camera UI")]
    public GameObject m_camViewBlocker;
    public Animator m_animZoneCamView;
    public TextMeshProUGUI m_animZoneLabel;

    [Header("Target Change Indicators")]
    public Button m_btnPrevTarget;
    public Button m_btnNextTarget;



    //--- Private Variables ---//
    private Delivery_Player m_playerDelivery;



    //--- Unity Methods ---//
    private void Awake()
    {
        // Init the private variables
        m_playerDelivery = GameObject.FindObjectOfType<Delivery_Player>();

        // Hook into the player's events
        // This way, we can update the UI anytime the target information has changed
        m_playerDelivery.OnTargetZoneChanged.AddListener(this.OnNewTargetZone);
        m_playerDelivery.OnTargetListChanged.AddListener(this.OnTargetListChanged);
    }

    private void Update()
    {
        // Press the spacebar to show the target image temporarily before having it disappear
        if (Input.GetKeyDown(KeyCode.Space))
            ToggleTargetImage();
    }



    //--- Methods ---//
    public void ToggleTargetImage()
    {
        // Figure out the current state of the animation
        var stateInfo = m_animZoneCamView.GetCurrentAnimatorStateInfo(0);

        // We can't toggle the animation trigger during a transition or it will get stuck for the next go-around
        if (!m_animZoneCamView.IsInTransition(0))
        {
            // Should also only be able to toggle it when it is fully hidden or fully visible, not while animating 
            if (stateInfo.IsName("DeliveryZoneImage_Visible") || stateInfo.IsName("DeliveryZoneImage_Hidden"))
                m_animZoneCamView.SetTrigger("TogglePhoto");
        }
    }

    public void OnNewTargetZone(Delivery_End _newTarget)
    {
        // Toggle the blocker image to black out the viewer or keep it visible, depending on if there is a target or not
        m_camViewBlocker.SetActive(_newTarget == null);

        // If the new zone is actually null, we should simply set the image to black and update the label to say so
        // Otherwise, we need to show the zone camera's view instead
        if (_newTarget == null)
        {
            // Change the label to indicate that there is no target
            m_animZoneLabel.text = "No Deliveries";
        }
        else
        {
            // Get the zone's camera
            Camera zoneCam = _newTarget.m_zoneCam;

            // Force the camera to render one frame so that it updates the render texture
            // This can be done even if the camera is disabled
            zoneCam.Render();

            // Change the label to indicate there is a delivery dropoff available
            m_animZoneLabel.text = "Dropoff Point";
        }
    }

    public void OnTargetListChanged(int _listCount)
    {
        // Toggle the buttons to be interactive or not if there is more than 1 target to switch between
        m_btnPrevTarget.interactable = (_listCount > 0);
        m_btnNextTarget.interactable = (_listCount > 0);
    }
}