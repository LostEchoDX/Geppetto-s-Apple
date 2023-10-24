using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EzySlice;
using UnityEngine.InputSystem;

public class SliceObject : MonoBehaviour
{
    public Transform startSlicePoint;
    public Transform endSlicePoint;
    public VelocityEstimator velocityEstimator;
    public LayerMask sliceableLayer;

    public AudioSource SwordSlashSound;

    public float sliceCooldown;
    private float sliceCdTimer;

    public float cutForce;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (sliceCdTimer > 0) {
            sliceCdTimer -= Time.deltaTime;
        }
        bool hasHit = Physics.Linecast(startSlicePoint.position, endSlicePoint.position, out RaycastHit hit, sliceableLayer);
        if (hasHit && sliceCdTimer <= 0) {
            GameObject target = hit.transform.gameObject;
            sliceCdTimer = sliceCooldown;
            Slice(target);
        }
    }

    public void Slice(GameObject target) {
        Vector3 velocity = velocityEstimator.GetVelocityEstimate();
        Vector3 planeNormal = Vector3.Cross(endSlicePoint.position - startSlicePoint.position, velocity);
        planeNormal.Normalize();
        SlicedHull hull = target.Slice(endSlicePoint.position, planeNormal);

        if (hull != null) {
            Transform targetParent = target.transform.parent;
            target.transform.SetParent(null);
            GameObject upperHull = hull.CreateUpperHull(target, target.GetComponent<Renderer>().material);
            GameObject lowerHull = hull.CreateLowerHull(target, target.GetComponent<Renderer>().material);
            upperHull.layer = LayerMask.NameToLayer("Sliceable");
            lowerHull.layer = LayerMask.NameToLayer("Sliceable");
            SetupSlicedComponent(upperHull);
            SetupSlicedComponent(lowerHull);

            upperHull.transform.SetParent(targetParent);
            lowerHull.transform.SetParent(targetParent);
            if (SwordSlashSound != null) {
                SwordSlashSound.Play();
            }
            Destroy(target);
        }
    }

    public void SetupSlicedComponent(GameObject slicedObject) {
        Rigidbody rb = slicedObject.AddComponent<Rigidbody>();
        MeshCollider collider = slicedObject.AddComponent<MeshCollider>();
        collider.convex = true;
        rb.AddExplosionForce(cutForce, slicedObject.transform.position, 1);
    }
}
