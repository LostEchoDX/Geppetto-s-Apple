using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace UnityEngine.XR.Content.Interaction {
    public class DoorUnlocker : MonoBehaviour 
    {
        XRLockSocketInteractor LockSocketInteractor;
        private Animation openDoorAnimation;
        public AudioSource openDoorSound;
        public AudioSource unlockDoorSound;
        bool keyUsed = false;
        Quaternion openedDoorRotation;
        Quaternion initialDoorRotation;
        // Start is called before the first frame update
        void Start()
        {
            openDoorAnimation = this.GetComponent<Animation>();
            LockSocketInteractor = this.transform.Find("Door").Find("Lock").Find("Keylock").GetComponent<XRLockSocketInteractor>();
        }

        // Update is called once per frame
        void Update()
        {
            if (LockSocketInteractor.firstInteractableSelected != null && keyUsed == false) {
                Transform Door = this.transform.Find("Door");
                keyUsed = true;
                LockSocketInteractor.firstInteractableSelected.transform.SetParent(Door.Find("Lock").Find("Keylock"));
                LockSocketInteractor.firstInteractableSelected.transform.GetComponent<Rigidbody>().useGravity = false;
                initialDoorRotation = Door.rotation;
                openDoorAnimation.Play();
                if (openDoorSound && unlockDoorSound) {
                    unlockDoorSound.Play();
                    openDoorSound.Play();
                }
                LockSocketInteractor.firstInteractableSelected.transform.GetComponent<XRGrabInteractable>().interactionLayers = InteractionLayerMask.GetMask("lock interaction");
                // LockSocketInteractor.firstInteractableSelected.transform.GetComponent<XRGrabInteractable>().enabled = false;
                // openedDoorRotation = new Quaternion(0, 90, 0, 0);
            }
        }
    }
}
