using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingHook : MonoBehaviour
{
    bool isHookMoving = false;
    public int maxDistance = 1000;
    private RaycastHit hit;
    public float speed = 10;
    public GameObject hook;
    private Vector3 hookedLocation;
    private GameObject hookedObject;
    public Transform CanonPosition;
    public LineRenderer lineRenderer;
    // Start is called before the first frame update
    void Start()
    {
        lineRenderer.enabled = true;
    }

    void Grab() {
        print("HELLO");
        if (Physics.Raycast(hook.transform.position, hook.transform.forward, out hit, maxDistance)) {
            print("CA TOUCHE");
            hookedLocation = hit.point;
            hookedObject = hit.collider.gameObject;
            isHookMoving = true;
        }
    }

    void MoveHook() {
        hook.transform.position = Vector3.Lerp(hook.transform.position, hookedLocation, speed* Time.deltaTime/Vector3.Distance(transform.position, hookedLocation));
        if (Vector3.Distance(hook.transform.position, hookedLocation) < 1f) {
            isHookMoving = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.Space)) {
            Grab();
        }
        if(isHookMoving) {
            MoveHook();
        }
        lineRenderer.SetPosition(0, CanonPosition.position);
        lineRenderer.SetPosition(1, hook.transform.position);
    }


}
