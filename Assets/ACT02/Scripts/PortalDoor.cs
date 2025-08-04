using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalDoor : MonoBehaviour {

    public PortalCameraController portalController;

    // Use this for initialization
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        portalController.ChangePlayer();
    }
}
