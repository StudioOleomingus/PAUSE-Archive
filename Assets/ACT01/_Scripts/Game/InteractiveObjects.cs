using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveObjects : MonoBehaviour {

    public E_INTERACTIVE_OBJECT_TYPE type;

    public float interationRadius = 3;

    [HideInInspector]
    public bool isHilighted;

    [HideInInspector]
    public bool isInteracting;

    [HideInInspector]
    public bool isResetInProgress;

    public virtual void Update()
    {
        if (isHilighted && !isInteracting)
        {
            GameManager.instance.inetractionImage.localScale = Vector3.Lerp(GameManager.instance.inetractionImage.localScale, new Vector3(1, 1, 1), Time.deltaTime * 3);
        }
    }

    public virtual bool IsInsideInteractionRadius()
    {
        bool flag = false;
        if(Vector3.Distance(InteractionController.instance.playerCamera.position, transform.position) < interationRadius)
        {
            flag = true;
        }
        return flag;
    }

    public virtual void OnLook ()
    {
        if (isHilighted)
            return;

        isHilighted = true;
    }

    public virtual void OnUnLook()
    {
        GameManager.instance.inetractionImage.localScale = new Vector3(0, 0, 0);
        isHilighted = false;
        InteractionController.instance.EnablePlayerMovement();
    }

    public virtual void OnInteract()
    {
        isInteracting = true;
        InteractionController.instance.DisablePlayerMovement();
        GameManager.instance.inetractionImage.localScale = new Vector3(0, 0, 0);
    }
}
