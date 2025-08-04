using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalCameraController : MonoBehaviour {

    public Transform portalCamera;
    public Transform player;
    public GameObject invertPlayer;
    public Transform playerCamera;

    public float angleFactor = -1;
    public float diffinangle = 0;

    public Vector3 referenceDoorPosition;
    public Vector3 startPosition;
    public Transform door;

    public Vector3 offsetDirection = new Vector3(1, 1, 1);
    public Vector3 offsetPosition = new Vector3(0, 0, 0);
    public Vector3 offsetAngle = new Vector3(0, 0, 0);


    public Camera cameraA;
    //public Camera cameraB;

    public Material cameraMatA;
    //public Material cameraMatB;

    // Use this for initialization
    void Start()
    {
        startPosition = transform.position;

        if (cameraA.targetTexture != null)
        {
            cameraA.targetTexture.Release();
        }
        cameraA.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
        cameraMatA.mainTexture = cameraA.targetTexture;

        //if (cameraB.targetTexture != null)
        //{
        //    cameraB.targetTexture.Release();
        //}
        //cameraB.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
        //cameraMatB.mainTexture = cameraB.targetTexture;
    }

    //10.156 14.87 10.346
    // Update is called once per frame
    void Update () {
        float x = player.position.x - referenceDoorPosition.x;
        float y = player.position.y - referenceDoorPosition.y;
        float z = player.position.z - referenceDoorPosition.z;

        
        Vector3 newPosition = transform.position;
       // newPosition.x = startPosition.x - (Mathf.Abs(x) * offsetDirection.x + offsetPosition.x);
       // newPosition.y = startPosition.y + (Mathf.Abs(y) * offsetDirection.y + offsetPosition.y);
       //newPosition.z = startPosition.z - (z * offsetDirection.y + offsetPosition.z);
        newPosition.z = player.position.z;
        newPosition.z += (startPosition.z - newPosition.z);
        transform.position = newPosition;
        portalCamera.rotation = Quaternion.Euler(angleFactor * playerCamera.eulerAngles.x, (angleFactor * player.eulerAngles.y) + offsetAngle.y, 180);
        portalCamera.GetComponent<Camera>().aspect = Screen.width/Screen.height;
    }

    public void ChangePlayer()
    {
        invertPlayer.SetActive(true);
        player.gameObject.SetActive(false);
    }
}
