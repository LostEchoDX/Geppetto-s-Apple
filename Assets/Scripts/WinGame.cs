using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WinGame : MonoBehaviour
{
    [SerializeField]
    Animation openGate;

    [SerializeField]
    TextMeshProUGUI success;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.name == "Nolant")
        {
            openGate.Play();
            success.text = " Oh! Thank you so much for the apple, you can leave now, I've opened the door for you";
            success.verticalAlignment = VerticalAlignmentOptions.Middle;
            success.horizontalAlignment = HorizontalAlignmentOptions.Center;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
