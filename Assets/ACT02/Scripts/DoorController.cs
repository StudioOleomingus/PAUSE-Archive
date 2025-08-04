using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour {

    public float from;
    public float to;

    private Transform player;

    public bool doorOpened;
    public bool doorOpenedCompleted;

    public float offsetAngleY;

    
    public void OpenDoor (Transform player)
    {
        
        this.player = player;
        gameObject.GetComponent<BoxCollider>().enabled = false;
        iTween.ValueTo(this.gameObject, iTween.Hash("from",from,"to",to,"easetype",iTween.EaseType.easeInSine,"onupdate","OnUpdateAngle","oncomplete", "OnCompleteOpen"));
    }

    public void OnUpdateAngle(float value)
    {
        transform.rotation = Quaternion.Euler(transform.eulerAngles.x, value + offsetAngleY, transform.eulerAngles.z);
    }

    public void OnCompleteOpen()
    {
        doorOpened = true;
        doorOpenedCompleted = true;
    }

    public void OnCompleteClose()
    {
        gameObject.GetComponent<BoxCollider>().enabled = true;
        doorOpened = false;
    }

    private void Update()
    {
        if (!doorOpened)
            return;

        if((Vector3.Distance(transform.position, player.position) > 4 || !player.gameObject.activeSelf) && doorOpened)
        {
            gameObject.GetComponent<BoxCollider>().enabled = true;
            iTween.ValueTo(this.gameObject, iTween.Hash("from", to, "to", from, "easetype", iTween.EaseType.easeInSine, "onupdate", "OnUpdateAngle", "oncomplete", "OnCompleteClose"));
        }
    }
}
