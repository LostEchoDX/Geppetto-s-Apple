using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;

public class WeaponGetter : MonoBehaviour
{
    bool rightHandTriggered = false;
    bool canInteract = false;
    GameObject weapon = null;

    // REFERENCES

    public AudioSource grabWeaponSound;
    public PlayerManager playerManager;
    public GameObject rightGrabRay;
    public GameObject objectPrefab;
    public GameObject RightHandObject;
    public GameObject TutoCanvas;

    // INPUTS
    public InputActionProperty rightSelect;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (rightHandTriggered == true && canInteract == true) {
            if (rightSelect.action.ReadValue<float>() > 0.8) {
                if (weapon == null && !playerManager.holdWeapon) {
                    GrabWeapon();
                } else { 
                    if (weapon != null && playerManager.holdWeapon) {
                        StoreWeapon();
                    }
                }
                canInteract = false;
            }
        }
    }

    public void GrabWeapon() {
        weapon = GameObject.Instantiate(objectPrefab);
        weapon.transform.position = RightHandObject.transform.position;
        weapon.transform.rotation = RightHandObject.transform.rotation;
        // GrapplingHook.transform.Rotate(0f, 0f, 90f);
        weapon.transform.SetParent(RightHandObject.transform);
        playerManager.holdWeapon = true;
        rightGrabRay.SetActive(false);
        grabWeaponSound.Play();
    }

    void StoreWeapon() {
        if (TutoCanvas.activeSelf) {
            TutoCanvas.SetActive(false);
        }
        Destroy(weapon);
        weapon = null;
        playerManager.holdWeapon = false;
        rightGrabRay.SetActive(true);
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Right Hand") {
            if (!playerManager.holdWeapon || weapon != null) {
                collider.gameObject.GetComponent<ActionBasedController>().SendHapticImpulse(0.7f, 0.7f);
            }
            canInteract = true;
            rightHandTriggered = true;
        }
    }

    void OnTriggerExit(Collider collider) {
        if (collider.gameObject.tag == "Right Hand") {
            rightHandTriggered = false;
            canInteract = false;
        }
    }
}
