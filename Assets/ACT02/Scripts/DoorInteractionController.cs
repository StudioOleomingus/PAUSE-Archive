using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteractionController : MonoBehaviour {

    private RaycastHit hit;
    public LayerMask myLayerMask;

    public Transform door;

    public bool interactionImageEnabled;
	// Update is called once per frame
	void Update () {
        
        if (Physics.Raycast(transform.position, transform.forward, out hit, 4, ~myLayerMask))
        {
            if (hit.collider.tag == "door")
            {
                interactionImageEnabled = true;
                GameManager.instance.inetractionImage.localScale = Vector3.Lerp(GameManager.instance.inetractionImage.localScale, new Vector3(1, 1, 1), Time.deltaTime * 3);
                //Debug.Log(Vector3.Distance(door.position, transform.position));
                if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Joystick1Button2))
                {
                    GameManager.instance.inetractionImage.localScale = new Vector3(0, 0, 0);
                    hit.collider.GetComponent<DoorController>().OpenDoor(transform.parent.parent);
                }
            }
            else
            {
                if (interactionImageEnabled)
                {
                    interactionImageEnabled = false;
                    GameManager.instance.inetractionImage.localScale = new Vector3(0, 0, 0);
                }
            }
        }

    }
}
