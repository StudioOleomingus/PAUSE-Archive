using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Telephone : InteractiveObjects {

    public Transform handset;
    public Transform handsetInitPosition;

    private bool initialAnimDone;
    private bool reset;
    private Vector3 handset_initial_position;

    public bool onHold;

    public System.Action onPickRecieverAction;

	// Use this for initialization
	void Start () {
        iTween.ShakePosition(handset.gameObject,iTween.Hash("amount", new Vector3(0.01f, 0, 0.01f), "islocal", false, "looptype", iTween.LoopType.loop));
        handset_initial_position = handset.position;
        
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();        
    }

    public override void OnUnLook()
    {
        base.OnUnLook();
        initialAnimDone = false;
        if(isInteracting)
            StartCoroutine(DoResetAnimation());
    }

    public override void OnInteract()
    {
        base.OnInteract();
        if (!GameManager.instance.startbgmusic)
        {
            GameManager.instance.startbgmusic = true;
        }
        if (isResetInProgress)
            return;
        GameManager.instance.telephoneSource.Stop();
        GameManager.instance.telephonePickupSource.Play(); 
        StartCoroutine(DoInteractionAnimation());
    }

    public void PutOnHold()
    {
        iTween.Stop(handset.gameObject);
        isInteracting = false;
        onHold = true;
        handset.SetParent(transform);
        iTween.MoveTo(handset.gameObject,iTween.Hash("position", handsetInitPosition.position));
    }
    
    private void OnReachRightEar()
    {
        if (onPickRecieverAction != null)
            onPickRecieverAction();
    }

    IEnumerator DoInteractionAnimation()
    {
        iTween.Stop(handset.gameObject);
        float currTime = 0;
        if(onHold)
        {
            currTime = 0.6f;
            onHold = false;
        }
        while (currTime < 0.5f)
        {
            currTime += Time.deltaTime;
            handset.position = Vector3.Lerp(handset.position, handsetInitPosition.position, Time.deltaTime * 2);
            yield return null;
        }
        initialAnimDone = true;
        yield return new WaitForSeconds(0.5f);
        OnReachRightEar();
    }

    IEnumerator DoResetAnimation()
    {
        isResetInProgress = true;
        handset.SetParent(transform);
        handset.localRotation = Quaternion.Euler(0, 0, 0);
        float currTime = 0;
        while (currTime < 0.5f)
        {
            currTime += Time.deltaTime;
            handset.position = Vector3.Lerp(handset.position, handset_initial_position, Time.deltaTime * 5);
            yield return null;
        }
        isInteracting = false;
        isResetInProgress = false;
    }
}
