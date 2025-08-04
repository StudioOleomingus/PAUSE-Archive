using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paper : InteractiveObjects {

    public Transform anchor1;
    public Transform anchor2;

    public float initialDistanceFromPlayerCamera;
    public float currentDistanceFromPlayerCamera;
    private float anchor2_X_rotation;

    private Vector3 initialPosition;
    private Quaternion initialRotation;

	// Use this for initialization
	void Start () {
        initialPosition = transform.position;
        initialRotation = anchor1.localRotation;
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        if (isInteracting)
        {
            transform.position = InteractionController.instance.playerCamera.position + InteractionController.instance.playerCamera.forward * currentDistanceFromPlayerCamera;
            
            anchor2.localRotation = Quaternion.Lerp(anchor2.localRotation, Quaternion.Euler(anchor2_X_rotation, 0, 0), Time.deltaTime * 5);

            var lookPos = InteractionController.instance.playerCamera.position - anchor1.position;
            lookPos.y = 0;
            var rotation = Quaternion.LookRotation(lookPos);
            anchor1.rotation = Quaternion.Slerp(anchor1.rotation, rotation, Time.deltaTime * 5);

            anchor2_X_rotation -= Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * 550;
            currentDistanceFromPlayerCamera += Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * 20;

            if (currentDistanceFromPlayerCamera >= initialDistanceFromPlayerCamera)
                currentDistanceFromPlayerCamera = initialDistanceFromPlayerCamera;

            if (currentDistanceFromPlayerCamera < 0.8f)
                currentDistanceFromPlayerCamera = 0.8f;

            if (anchor2_X_rotation > 45)
                anchor2_X_rotation = 45;

            if (anchor2_X_rotation < 10)
                anchor2_X_rotation = 10;
            
        }
	}

    public override void OnUnLook()
    {
        base.OnUnLook();
        Debug.Log("Unlook");
        if (isInteracting)
            StartCoroutine(DoResetAnimation());
    }

    public override void OnInteract()
    {
        base.OnInteract();

        if (isResetInProgress)
            return;
        GetComponent<BoxCollider>().enabled = false;
        initialDistanceFromPlayerCamera = Vector3.Distance(transform.position, InteractionController.instance.playerCamera.position);
        currentDistanceFromPlayerCamera = initialDistanceFromPlayerCamera;
        anchor2_X_rotation = 10;
    }

    IEnumerator DoResetAnimation()
    {
        isInteracting = false;
        isResetInProgress = true;
        float currTime = 0;
        while (currTime < 1.5f)
        {
            currTime += Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, initialPosition, Time.deltaTime * 5);
            anchor1.localRotation = Quaternion.Lerp(anchor1.localRotation, initialRotation, Time.deltaTime * 5);
            anchor2.localRotation = Quaternion.Lerp(anchor2.localRotation, Quaternion.Euler(0, 0, 0), Time.deltaTime * 5);
            yield return null;
        }
        
        isResetInProgress = false;
        GetComponent<BoxCollider>().enabled = true;
    }
}
