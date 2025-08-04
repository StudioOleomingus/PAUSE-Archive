using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;

public class PortalCamera : MonoBehaviour {
    /*
	public Transform playerCamera;
	public Transform player;
    public GameObject portal;
    public GameObject otherPortal;
    

	public float angleFactor = -1;
    public float diffinangle = 0;

	public bool isInverseCam;

	public float y = -2.78f;

	public bool positionLock;

	public Vector3 startPosition;
	public Transform door;

	public float zOffset = -0.0f;
	public float yOffset = 0.189653f;
	public float xOffset = 0.14325f;

    // Use this for initialization
    void Start () {
		startPosition = transform.position;
	}
	
	// Each frame reposition the camera to mimic the players offset from the other portals position
	void Update () {

		float x = startPosition.x - player.position.x;
		float y = startPosition.y - player.position.y;
		float z = startPosition.z - player.position.z;

		Vector3 newPosition = transform.position;

		newPosition.z = startPosition.z - Mathf.Abs (z) + zOffset;
		newPosition.x = startPosition.x + x - xOffset;
		newPosition.y = startPosition.y - yOffset;

		transform.position = newPosition;

		transform.rotation = Quaternion.Euler(playerCamera.eulerAngles.x, (angleFactor * player.eulerAngles.y) + diffinangle, 0);
    }
    */
    public Transform cameraParent;
    public Transform cameraObj;
    public Transform playerCamera;
	public Transform player;
	public GameObject portal;
	public GameObject otherPortal;


	public float angleFactor = -1;
	public float diffinangle = 0;

	public bool isInverseCam;

	public float y = -2.78f;

	public bool positionLock;

	public Vector3 referenceDoorPosition;
	public Vector3 startPosition;
	public Transform door;

	public Vector3 offsetDirection = new Vector3(1,1,1);

	public float zOffset = -0.0f;
	public float yOffset = 0.189653f;
	public float xOffset = 0.14325f;

	public enum PortalXPosition
	{
		PlayerPosition,Something
	}

	public PortalXPosition portalXPosition;

	// Use this for initialization
	void Start () {
		startPosition = transform.position;
	}

	// Each frame reposition the camera to mimic the players offset from the other portals position
	void Update () {

		//Debug.Log (player.GetComponent<FirstPersonController>().IsJumping);

		//float x = referenceDoorPosition.x - player.position.x;
		//float y = referenceDoorPosition.y - player.position.y;
		//float z = referenceDoorPosition.x - player.position.z;

		Vector3 newPosition = transform.position;

		//newPosition.z = startPosition.z - (Mathf.Abs (z) * offsetDirection.z + zOffset);
		//if(portalXPosition == PortalXPosition.PlayerPosition)
			//newPosition.x = player.position.z;
		//else if(portalXPosition == PortalXPosition.Something)
		//	newPosition.x = startPosition.x - x - xOffset; 


            //newPosition.y = startPosition.y + yOffset;

		transform.position = newPosition;

		transform.rotation = Quaternion.Euler(angleFactor * playerCamera.eulerAngles.x, (angleFactor * player.eulerAngles.y) + diffinangle, 180);
	}
}
