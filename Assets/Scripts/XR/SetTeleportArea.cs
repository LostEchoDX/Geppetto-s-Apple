using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SetTeleportArea : MonoBehaviour
{
    private TeleportationArea tpArea;
    // Start is called before the first frame update
    void Start()
    {
        tpArea = this.GetComponent<TeleportationArea>();
        if (tpArea == null) {
            tpArea = this.gameObject.AddComponent<TeleportationArea>();
        }
        tpArea.colliders.Add(this.GetComponent<MeshCollider>());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
