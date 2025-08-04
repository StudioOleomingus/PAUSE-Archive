using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OleoInputManager : MonoBehaviour {

    public static OleoInputManager instance;

    public enum INPUT_TYPE { Joystick, Keyboard, Leapmotion }
    public INPUT_TYPE iNPUT_TYPE;

    public int dialogueIndex;
    public int dialogueLength;

    public GameObject inputSelectionPanel;
    public GameObject startPanel;

    public List<GameObject> choices = new List<GameObject>();
    // Use this for initialization
    void Awake () {
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

    bool isDown;
    bool isUp;
    bool inputtypeselected;

    private void Start()
    {
        inputSelectionPanel.SetActive(true);
    }

    // Update is called once per frame
    void Update () {

        if (!inputtypeselected)
        {
            if (Input.GetKeyDown(KeyCode.Joystick1Button2))
            {
                iNPUT_TYPE = INPUT_TYPE.Joystick;
                inputtypeselected = true;
                inputSelectionPanel.SetActive(false);
                startPanel.SetActive(true);
                startPanel.SendMessage("Init");
            }
            else if (Input.GetKeyDown(KeyCode.X))
            {
                iNPUT_TYPE = INPUT_TYPE.Keyboard;
                inputtypeselected = true;
                inputSelectionPanel.SetActive(false);
                startPanel.SetActive(true);
                startPanel.SendMessage("Init");
            }
            return;
        }

        if (iNPUT_TYPE != INPUT_TYPE.Joystick)
            return;

        
        if (Input.GetAxis("Vertical") < -0.1f && !isDown)
        {
            dialogueIndex++;
            if(dialogueIndex >= dialogueLength)
            {
                dialogueIndex = 0;
            }
            foreach (GameObject g in choices)
            {
                g.SendMessage("On_PointerExit", SendMessageOptions.DontRequireReceiver);
            }
            if (choices.Count > 0)
            {
                if(choices[dialogueIndex] != null)
                    choices[dialogueIndex].SendMessage("On_PointerEnter", SendMessageOptions.DontRequireReceiver);
            }
            isDown = true;
        }

        if (Input.GetAxis("Vertical") > 0.1f && !isUp)
        {
            dialogueIndex--;
            if (dialogueIndex < 0)
            {
                dialogueIndex = dialogueLength-1;
            }
            foreach(GameObject g in choices)
            {
                g.SendMessage("On_PointerExit", SendMessageOptions.DontRequireReceiver);
            }
            if (choices.Count > 0)
            {
                if (choices[dialogueIndex] != null)
                    choices[dialogueIndex].SendMessage("On_PointerEnter", SendMessageOptions.DontRequireReceiver);
            }
            isUp = true;
        }

        if (Input.GetKeyDown(KeyCode.Joystick1Button2))
        {
            if(choices.Count > 0)
            {
                if (choices[dialogueIndex] != null)
                    choices[dialogueIndex].SendMessage("OnClick_1ChoiceBtn", SendMessageOptions.DontRequireReceiver);
            }
        }

        if(Input.GetAxis("Vertical") > -0.1f && Input.GetAxis("Vertical") < 0.1f)
        {
            isDown = false;
            isUp = false;
        }
    }
}
