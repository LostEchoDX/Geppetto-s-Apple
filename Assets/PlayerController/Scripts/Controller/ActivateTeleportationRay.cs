using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;

public class ActivateTeleportationRay : MonoBehaviour
{
    public GameObject leftTeleportation;
    public GameObject rightTeleportation;
    public bool leftEnabled = false;
    public bool rightEnabled = false;
    public InputActionProperty leftActivate;
    public InputActionProperty rightActivate;

    public InputActionProperty leftSelect;
    public InputActionProperty rightSelect;

    public XRRayInteractor leftRay;
    public XRRayInteractor rightRay;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        bool isLeftRayHovering = leftRay.TryGetHitInfo(out Vector3 leftPos, out Vector3 leftNormal, out int leftNumber, out bool leftValid);
        bool isRightRayHovering = leftRay.TryGetHitInfo(out Vector3 rightPos, out Vector3 rightNormal, out int rightNumber, out bool rightValid);

        if (!isLeftRayHovering && leftEnabled) {
            leftTeleportation.SetActive(leftActivate.action.ReadValue<float>() > 0.1f);
            if (leftSelect.action.ReadValue<float>() > 0.1)
                leftTeleportation.SetActive(false);
        } else leftTeleportation.SetActive(false);
        if (!isRightRayHovering && rightEnabled) {
            rightTeleportation.SetActive(rightActivate.action.ReadValue<float>() > 0.1f);
            if (rightSelect.action.ReadValue<float>() > 0.1)
                rightTeleportation.SetActive(false);
        } else rightTeleportation.SetActive(false);
    }
}
