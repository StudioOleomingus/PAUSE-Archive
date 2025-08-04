using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TelephoneController : MonoBehaviour {

    public static TelephoneController instance;

    public Telephone[] telephones;

    private void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    void Start () {
        GameManager.instance.HookCallBackToTelephones(this);
	}

    public void PutRecieverOnHold()
    {
        foreach (Telephone t in telephones)
        {
            t.PutOnHold();
        }
    }
}
