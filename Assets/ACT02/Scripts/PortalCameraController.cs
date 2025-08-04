using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class PortalCameraController : MonoBehaviour {

    public Transform portalCamera;
    public Transform player;
    public GameObject invertPlayer;
    public Transform playerCamera;
    public Transform renderPlane;
    public Transform door;

    public GameObject visibleDoor;

    public float angleFactor = -1;
    public float diffinangle = 0;

    public Vector3 referenceDoorPosition;
    public Vector3 startPosition;

    public Vector3 offsetDirection = new Vector3(1, 1, 1);
    public Vector3 offsetPosition = new Vector3(0, 0, 0);
    public Vector3 offsetAngle = new Vector3(0, 0, 0);

    public bool isXForward;
    public bool isXForwardIsInverted;
    public bool isZForward;
    public bool isZForwardIsInverted;

    public bool isSamePlayer;

    public float invertedPlayerInitialYpos;

    // Use this for initialization
    void Start()
    {
        startPosition = transform.position;
    }

    //10.156 14.87 10.346
    // Update is called once per frame
    void Update () {

        // renderplane is always in front of the player, so we are check how far player is in left or right or forward and backward from renderplane,
        // and then applying these values to the camera with reference to door.
        float x = renderPlane.position.x - player.position.x;
        float z = renderPlane.position.z - player.position.z;
        

        Vector3 newPosition = transform.position;

        // in some cases player forward direction is x axis, in this case we need to move the camera forward and backward in x axis
        if (isXForward)
        {
            // when player is inverted the x axis movement also need to be inverted
            if (isXForwardIsInverted)
            {
                newPosition.x = door.position.x - Mathf.Abs(x);
            }
            else
            {
                newPosition.x = door.position.x + Mathf.Abs(x);
            }
            
            newPosition.z = startPosition.z + z * offsetDirection.z;
        }
        else if (isZForward)
        {
            if (isZForwardIsInverted)
            {
                newPosition.z = door.position.z - Mathf.Abs(z);
            }
            else
            {
                newPosition.z = door.position.z + Mathf.Abs(z);
            }
            
            newPosition.x = startPosition.x + x * offsetDirection.x;
        }

        transform.position = newPosition;
        portalCamera.rotation = Quaternion.Euler(angleFactor * playerCamera.eulerAngles.x, (angleFactor * player.eulerAngles.y) + offsetAngle.y, offsetAngle.z);
        
    }

    // in some case we need to invert the camera x angle
    public int xAngleDir = 1;
    public void ChangePlayer()
    {
        visibleDoor.SetActive(false);
        Debug.Log(name);
        float x = 0;
        if (isXForward && isXForwardIsInverted || isSamePlayer)
            x += 1;
        if (isSamePlayer && isXForward && !isXForwardIsInverted)
            x -= 3;
        float z = 0;
        if (isZForward && isZForwardIsInverted)
            z += 1;
        invertPlayer.transform.position = new Vector3(portalCamera.position.x+x, invertedPlayerInitialYpos, portalCamera.position.z+z);
        invertPlayer.transform.rotation = Quaternion.Euler(0,portalCamera.eulerAngles.y, invertPlayer.transform.eulerAngles.z);
        invertPlayer.GetComponent<FirstPersonController>().m_Camera.transform.localRotation = Quaternion.Euler(playerCamera.eulerAngles.x * xAngleDir, 0, 0);
        invertPlayer.GetComponent<FirstPersonController>().M_MouseLook.Init(invertPlayer.transform, invertPlayer.GetComponent<FirstPersonController>().m_Camera.transform);
        if (!isSamePlayer)
        {
            invertPlayer.SetActive(true);
            player.gameObject.SetActive(false);
        }
        StartCoroutine(ActivateVisibleDoor());
    }

    IEnumerator ActivateVisibleDoor()
    {
        yield return new WaitForSeconds(0.5f);
        visibleDoor.SetActive(true);
    }
}
