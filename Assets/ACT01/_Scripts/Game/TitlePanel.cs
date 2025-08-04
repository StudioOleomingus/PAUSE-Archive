using UnityEngine;

public class TitlePanel : MonoBehaviour {

    public MotionDetection motionDetection;
	
	// Update is called once per frame
	void Update () {
        if(OleoInputManager.instance.iNPUT_TYPE == OleoInputManager.INPUT_TYPE.Keyboard)
        {
            if (Input.anyKeyDown)
            {
                GameManager.instance.OnCompleteTitle();
            }
        }
        else if (OleoInputManager.instance.iNPUT_TYPE == OleoInputManager.INPUT_TYPE.Joystick)
        {
            if (Input.anyKeyDown)
            {
                GameManager.instance.OnCompleteTitle();
            }
        }
        else if (OleoInputManager.instance.iNPUT_TYPE == OleoInputManager.INPUT_TYPE.Leapmotion)
        {
            if (motionDetection != null && motionDetection.GetMouseDown())
            {
                GameManager.instance.OnCompleteTitle();
            }
        }
        
    }
}
