using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using OleoStoryGenerator;
using UnityStandardAssets.Characters.FirstPerson;

public class InteractionController : MonoBehaviour {

    public static InteractionController instance;

    public Transform player;
    public Transform playerCamera;
    
    public FirstPersonController player_controller;

    public InteractiveObjects current_InteractiveObject;
    public InteractiveObjects last_InteractiveObject;

    private RaycastHit hit;
    public LayerMask myLayerMask;

    public string[] flightpaths;

    public MotionDetection motionDetection;


    void Awake()
    {
        //Check if instance already exists
        if (instance == null)
            //if not, set instance to this
            instance = this;
        //If instance already exists and it's not this:
        else if (instance != this)
        {
            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);
        }
    }

    // Use this for initialization
    void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
        

        //if (current_InteractiveObject != null && current_InteractiveObject.isInteracting)
        //{
        //    if (Input.GetKeyDown(KeyCode.E) || motionDetection.GetMouseDown())
        //    {
        //        if (current_InteractiveObject.isInteracting)
        //        {
        //            current_InteractiveObject.OnUnLook();
        //        }
        //    }
        //}
        if(playerCamera != null)
        {
            if (GameManager.instance.kilnDisplayManager.gameObject.activeSelf)
                return;

            if (Physics.Raycast(playerCamera.position, playerCamera.forward, out hit, Mathf.Infinity, ~myLayerMask))
            {
                if (hit.collider.tag == StaticStrings.InteractiveObjectTag)
                {
                    if (Input.GetKeyDown(KeyCode.E) || motionDetection.GetMouseDown() || Input.GetKeyDown(KeyCode.Joystick1Button2))
                    {
                        if (!current_InteractiveObject.isInteracting && current_InteractiveObject.IsInsideInteractionRadius())
                        {
                            current_InteractiveObject.OnInteract();
                        }
                    }

                    if (current_InteractiveObject != null && current_InteractiveObject.isInteracting)
                    {
                        //Debug.Log(current_InteractiveObject.name+" isInteracting "+ current_InteractiveObject.isInteracting);
                        return;
                    }


                    current_InteractiveObject = hit.collider.gameObject.GetComponent<InteractiveObjects>();

                    if (last_InteractiveObject != null && current_InteractiveObject != last_InteractiveObject)
                    {
                        //Debug.Log(" last interactive object is same as current ");
                        last_InteractiveObject.OnUnLook();
                    }

                    if (!current_InteractiveObject.IsInsideInteractionRadius())
                    {
                        //Debug.Log(current_InteractiveObject.name + " not in radius " + current_InteractiveObject.isInteracting);
                        return;
                    }

                    current_InteractiveObject.OnLook();
                    
                    last_InteractiveObject = current_InteractiveObject;
                }
                else
                {
                    if (last_InteractiveObject != null && !last_InteractiveObject.isInteracting)
                    {
                        //Debug.Log(last_InteractiveObject.name + " last interactive object not interacting " + last_InteractiveObject.isInteracting);
                        last_InteractiveObject.OnUnLook();
                    }
                }
            }
        }
        
    }

    public void DisablePlayerMovement ()
    {
        player_controller.canMove = false;
    }

    public void EnablePlayerMovement()
    {
        player_controller.canMove = true;
    }
}
