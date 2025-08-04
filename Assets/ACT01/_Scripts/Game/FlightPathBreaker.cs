using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class FlightPathBreaker : MonoBehaviour {

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == StaticStrings.InteractiveObjectTag)
        {
            if (other.GetComponent<InteractiveObjects>().type == E_INTERACTIVE_OBJECT_TYPE.PAPERPLANE)
            {
                if (!other.GetComponent<PaperPlane>().passengerDropped && other.GetComponent<PaperPlane>().canGlide)
                {
                    other.GetComponent<PaperPlane>().passengerDropped = true;
                    InteractionController.instance.player.position = transform.position;
                    InteractionController.instance.playerCamera.gameObject.SetActive(true);
                    InteractionController.instance.player.GetComponent<FirstPersonController>().M_MouseLook.Init(transform, InteractionController.instance.playerCamera);
                    InteractionController.instance.player.rotation = transform.rotation;

                    other.GetComponent<PaperPlane>().p_camera.SetActive(false);
                }
                
            }
        }
    }
}
