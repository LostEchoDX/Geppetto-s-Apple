using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRGrabWeapons : XRGrabInteractable
{
    private Vector3 initialLocalPos;
    private Quaternion initialLocalRot;
    public GameObject weaponGrabZone;
    public GameObject tutoCanvas;
    // Start is called before the first frame update
    void Start()
    {
        if(!attachTransform) {
            GameObject attachPoint = new GameObject("Offset Grab Pivot");
            attachPoint.transform.SetParent(transform, false);
            attachTransform = attachPoint.transform;
        }
        else {
            initialLocalPos = attachTransform.localPosition;
            initialLocalRot = attachTransform.localRotation;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        if (args.interactableObject is XRDirectInteractor) {
            attachTransform.position = args.interactorObject.transform.position;
            attachTransform.rotation = args.interactorObject.transform.rotation;
        } else {
            attachTransform.localPosition = initialLocalPos;
            attachTransform.localRotation = initialLocalRot;
        }
        if (weaponGrabZone != null) {
            tutoCanvas.SetActive(true);
            weaponGrabZone.SetActive(true);
            WeaponGetter weaponGetter = weaponGrabZone.GetComponent<WeaponGetter>();
            if (weaponGetter != null) {
                weaponGetter.GrabWeapon();
                Destroy(this.gameObject);
            }
        }
        base.OnSelectEntered(args);
    }
}
