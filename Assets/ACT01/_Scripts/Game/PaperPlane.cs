using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperPlane : InteractiveObjects {

    public Transform anchor1;
    public Transform anchor2;

    public GameObject p_camera;

    public Transform player_Right_Ear;

    public float initialDistanceFromPlayerCamera;
    public float currentDistanceFromPlayerCamera;
    private float anchor2_X_rotation;

    private Vector3 initialPosition;
    private Quaternion initialRotation;

    private bool onFlightPath;
    public bool passengerDropped;
    public bool canGlide;

    public bool flyonstart;

    // Use this for initialization
    void Start()
    {
        initialPosition = transform.position;
        initialRotation = anchor1.localRotation;

        if (flyonstart)
        {
            onFlightPath = true;
            isInteracting = false;
            OnCompleteReachPlayerRightEar();
        }
    }

    // Update is called once per frame
    public override void Update () {
        base.Update();

        if (isInteracting && !onFlightPath)
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

            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Joystick1Button2))
            {
                onFlightPath = true;
                isInteracting = false;
                OnCompleteReachPlayerRightEar();
                //iTween.MoveTo(gameObject, iTween.Hash("position", player_Right_Ear, "speed", 3.5f, "easetype", iTween.EaseType.linear, "oncomplete", "OnCompleteReachPlayerRightEar", "oncompletetarget", this.gameObject));
            }

        }
    }

    public override void OnUnLook()
    {
        base.OnUnLook();

        if (onFlightPath)
            return;

        if (isInteracting)
            StartCoroutine(DoResetAnimation());
    }

    public override void OnInteract()
    {
        base.OnInteract();

        if (isResetInProgress)
            return;
        GetComponent<BoxCollider>().enabled = false;
        initialDistanceFromPlayerCamera = Vector3.Distance(transform.position, InteractionController.instance.playerCamera.position)/2;
        currentDistanceFromPlayerCamera = initialDistanceFromPlayerCamera;
        anchor2_X_rotation = 10;
    }

    private void OnCompleteReachPlayerRightEar()
    {
        if(canGlide)
            StartCoroutine(EnableCamera());
        anchor1.localRotation = Quaternion.Euler(0, 180, 0);
        anchor2.localRotation = Quaternion.Euler(0, 0, 0);
        GetComponent<BoxCollider>().enabled = true;
        if (flyonstart)
        {
            iTween.MoveTo(gameObject, iTween.Hash("path", iTweenPath.GetPath(InteractionController.instance.flightpaths[Random.Range(0, InteractionController.instance.flightpaths.Length)]), "movetopath", true, "orienttopath", true, "time", 10, "easetype", iTween.EaseType.linear, "oncomplete", "OnCompletePath", "oncompletetarget", this.gameObject));
        }
        else
        {
            iTween.MoveTo(gameObject, iTween.Hash("path", iTweenPath.GetPath("flightpath1"), "movetopath", true, "orienttopath", true, "time", 10, "easetype", iTween.EaseType.linear, "oncomplete", "OnCompletePath", "oncompletetarget", this.gameObject));
        }
        
    }

    private void OnCompletePath()
    {
        //Debug.Log("OnCompletePath");
        iTween.MoveTo(gameObject, iTween.Hash("path", iTweenPath.GetPath(InteractionController.instance.flightpaths[Random.Range(0, InteractionController.instance.flightpaths.Length)]), "movetopath", true, "orienttopath", true, "time", 10, "easetype", iTween.EaseType.linear, "oncomplete", "OnCompletePath", "oncompletetarget", this.gameObject));
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



    IEnumerator EnableCamera()
    {
        yield return new WaitForSeconds(1.2f);
        p_camera.SetActive(true);
        InteractionController.instance.playerCamera.gameObject.SetActive(false);
    }
}
