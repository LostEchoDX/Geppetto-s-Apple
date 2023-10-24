using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;

public class Grappling : MonoBehaviour
{
    // REFERENCES
    public GameObject hook;
    public Transform hookBaseLocation;
    // public PlayerMovementGrappling playerMovementGrappling;
    public Transform gunTip;
    public LayerMask whatIsGrappleable;
    public LineRenderer lr;
    private LineRenderer hookLr;
    private Rigidbody interactableRigidbody;
    public InputActionProperty leftActivate;
    public InputActionProperty rightActivate;

    public AudioSource StartGrabSound;
    public AudioSource MoveGrabSound;
    public AudioSource TouchGrabSound;
    public AudioSource PullGrabSound;

    // GRAPPLING
    public float velocityThreshold = 2;
    public float jumpAngleInDegree = 60;
    public float maxGrappleDistance;
    public float grappleDelayTime;
    public float overshootYAxis;
    private Vector3 grapplePoint;
    private Quaternion hookLockedRotation;
    private GameObject hookedObject;
    //The distance vector between the center of the object grabbed and the grapple point.
    private Vector3 grabPointCenterDistance;
    public float hookSpeed = 10;
    bool isHookMoving = false;
    bool isHooking = false;

    //COOLDOWN

    public float grapplingCd;
    private float grapplingCdTimer;

    //INPUT
    
    public KeyCode grappleKey = KeyCode.Space;

    private bool grappling;
    private bool isPulling = false;
    private Vector3 previousPos;
    private bool canJump = true;

    // Start is called before the first frame update
    void Start()
    {
        hookLr = hook.GetComponent<LineRenderer>();
    }

    private void StartGrapple() {
        if (grapplingCdTimer > 0) return;

        // playerMovementGrappling.freeze = true;

        RaycastHit hit;
        if(Physics.Raycast(hook.transform.position, hook.transform.forward, out hit, maxGrappleDistance)) {
            print(LayerMask.LayerToName(hit.transform.gameObject.layer));
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Interactable") || hit.transform.gameObject.layer == LayerMask.NameToLayer("Sliceable")) {
                grappling = true;
                grapplePoint = hit.point;
                hookedObject = hit.transform.gameObject;
                grabPointCenterDistance = grapplePoint - hookedObject.transform.position;
                isHookMoving = true;
                lr.enabled = true;
                if(StartGrabSound != null) {
                    StartGrabSound.Play();
                }
                if (MoveGrabSound != null) {
                    MoveGrabSound.Play();
                }
            }
        } else {
            // grapplePoint = hook.transform.position + hook.transform.forward * maxGrappleDistance;
            // Invoke(nameof(StopGrapple), grappleDelayTime);
        }
    }

    private void ExecuteGrapple() {
        // playerMovementGrappling.freeze = false;

        // Vector3 lowestPoint = new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z);
        // float grapplePointRelativeYPos = grapplePoint.y - lowestPoint.y;
        // float highestPointOnArc = grapplePointRelativeYPos + overshootYAxis;

        // if (grapplePointRelativeYPos < 0) highestPointOnArc = overshootYAxis;

        // playerMovementGrappling.JumpToPosition(grapplePoint, highestPointOnArc);
        Rigidbody hookedRigidbody = hookedObject.GetComponent<Rigidbody>();
        if (hookedRigidbody != null) {
            hookedRigidbody.velocity = ComputeVelocity(hookedObject.transform.position);
        }
        // hook.GetComponent<Rigidbody>().useGravity = true;
        // hook.GetComponent<Rigidbody>().isKinematic = false;
        // hook.GetComponent<Rigidbody>().velocity = ComputeVelocity(grapplePoint);
        isHooking = false;
        isPulling = true;
        print("PULL");
        // hook.transform.position = hookBaseLocation.position;
        // StopGrapple();
        if(PullGrabSound != null) {
            PullGrabSound.Play();
        }
        Invoke(nameof(StopGrapple), 1f);
    }

    public void StopGrapple() {
        // playerMovementGrappling.freeze = false;
        print("STOP NOW");
        isHooking = false;
        grappling = false;
        grapplingCdTimer = grapplingCd;
        lr.enabled = false;
        // hook.GetComponent<Rigidbody>().isKinematic = true;
        // hook.GetComponent<Rigidbody>().useGravity = false;
        hook.transform.position = hookBaseLocation.position;
        hook.transform.rotation = hookBaseLocation.rotation;
        // hook.transform.SetParent(this.transform);
        isPulling = false;
    }

    void MoveHook() {
        hook.transform.position = Vector3.Lerp(hook.transform.position, grapplePoint, hookSpeed* Time.deltaTime/Vector3.Distance(transform.position, grapplePoint));
        if (Vector3.Distance(hook.transform.position, grapplePoint) < 0.1f) {
            hookLockedRotation = hook.transform.rotation;
            isHookMoving = false;
            previousPos = this.transform.position;
            isHooking = true;
            if (MoveGrabSound != null) {
                MoveGrabSound.Stop();
            }
            if (TouchGrabSound != null) {
                TouchGrabSound.Play();
            }
            // Invoke(nameof(ExecuteGrapple), grappleDelayTime);
        }
    }

    void manageHookRaycast() {
        RaycastHit hit;
        if(Physics.Raycast(hook.transform.position, hook.transform.forward, out hit, maxGrappleDistance)) {
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Interactable") || hit.transform.gameObject.layer == LayerMask.NameToLayer("Sliceable")) {
                hookLr.SetPosition(0, hook.transform.position);
                hookLr.SetPosition(1, hit.point);
                hookLr.enabled = true;
            } else {
                hookLr.enabled = false;
            }
        } else {
            hookLr.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        manageHookRaycast();
        if (rightActivate.action.ReadValue<float>() > 0.1 && !grappling) {
            StartGrapple();
        }

        if(isHookMoving) {
            MoveHook();
        }

        if (grapplingCdTimer > 0) {
            grapplingCdTimer -= Time.deltaTime;
        }
    }

    public Vector3 ComputeVelocity(Vector3 objectVectorToMove) {
        Vector3 diff = this.transform.position - objectVectorToMove;
        Vector3 diffXZ = new Vector3(diff.x, 0, diff.z);
        float diffXZLength = diffXZ.magnitude;
        float diffYLength = diff.y;

        float angleInRadian = jumpAngleInDegree * Mathf.Deg2Rad;

        float jumpSpeed = Mathf.Sqrt(-Physics.gravity.y * Mathf.Pow(diffXZLength, 2) / (2 * Mathf.Cos(angleInRadian)*Mathf.Cos(angleInRadian)*(diffXZ.magnitude * Mathf.Tan(angleInRadian) - diffYLength)));
        Vector3 jumpVelocityVector = diffXZ.normalized * Mathf.Cos(angleInRadian) * jumpSpeed + Vector3.up * Mathf.Sin(angleInRadian) * jumpSpeed;
        
        return jumpVelocityVector;
    }

    void LateUpdate() {
        if (isHookMoving || isPulling) {
            lr.SetPosition(0, gunTip.position);
            lr.SetPosition(1, hook.transform.position);
        }
        if (isPulling) {
            hook.transform.position = hookedObject.transform.position + grabPointCenterDistance;
        }
        if (isHooking) {
            lr.SetPosition(0, gunTip.position);
            Vector3 velocity = (this.transform.position - previousPos) / Time.deltaTime;
            previousPos = this.transform.position;
            hook.transform.position = grapplePoint;
            hook.transform.rotation = hookLockedRotation;
            if(velocity.magnitude > velocityThreshold) {
                ExecuteGrapple();
            }
            if (rightActivate.action.ReadValue<float>() < 0.1) {
                StopGrapple();
            }
        }
    }
}
