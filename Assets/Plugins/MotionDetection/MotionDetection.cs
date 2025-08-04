using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class MotionDetection : MonoBehaviour {

    public Transform thumbTracker;
    public Transform indexTracker;
    public Transform middleTracker;
    public Transform ringTracker;
    public Transform pinkyTracker;
    public Transform palm;

    public Transform angleTracker;

    public bool thumbstatus;
    public bool indexstatus;
    public bool middlestatus;
    public bool ringstatus;
    public bool pinkystatus;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //Debug.Log(angleTracker.eulerAngles);
        //Debug.Log(CrossPlatformInputManager.GetAxis("Mouse X"));
        //return;
        //Debug.Log("thumb "+ Vector3.Distance(thumbTracker.position, palm.position) +" index "+ Vector3.Distance(indexTracker.position,palm.position) + " middle " + Vector3.Distance(middleTracker.position, palm.position) + " ring " + Vector3.Distance(ringTracker.position, palm.position) + " pinky " + Vector3.Distance(pinkyTracker.position, palm.position));
        //return;
        if (Vector3.Distance(thumbTracker.position, palm.position) < 0.08f)
        {
            //Debug.Log("CLOSE");
            thumbstatus = true;
        }
        else
        {
            //Debug.Log("OPEN");
            thumbstatus = false;
        }

        if (Vector3.Distance(indexTracker.position, palm.position) < 0.08f)
        {
            //Debug.Log("CLOSE");
            indexstatus = true;
        }
        else
        {
            //Debug.Log("OPEN");
            indexstatus = false;
        }

        if (Vector3.Distance(middleTracker.position, palm.position) < 0.08f)
        {
            //Debug.Log("CLOSE");
            middlestatus = true;
        }
        else
        {
            //Debug.Log("OPEN");
            middlestatus = false;
        }

        if (Vector3.Distance(ringTracker.position, palm.position) < 0.08f)
        {
            //Debug.Log("CLOSE");
            ringstatus = true;
        }
        else
        {
            //Debug.Log("OPEN");
            ringstatus = false;
        }

        if (Vector3.Distance(pinkyTracker.position, palm.position) < 0.07f)
        {
            //Debug.Log("CLOSE");
            pinkystatus = true;
        }
        else
        {
            //Debug.Log("OPEN");
            pinkystatus = false;
        }

        if(thumbstatus && indexstatus && middlestatus && ringstatus && pinkystatus)
        {
            //Debug.Log("CLOSE");
            getmouseDown = false;
            getChoice1KeyDown = false;
            getChoice2KeyDown = false;
        }
        else if (!thumbstatus && indexstatus && middlestatus && ringstatus && pinkystatus)
        {
            //Debug.Log("THUMB OPEN");
            getChoice1KeyDown = false;
            getChoice2KeyDown = false;
        }
        else if (thumbstatus && indexstatus && !middlestatus && ringstatus && pinkystatus)
        {
            //Debug.Log("MIDDLE OPEN");
            getmouseDown = false;
            getChoice1KeyDown = false;
            getChoice2KeyDown = false;
        }
        else if (thumbstatus && !indexstatus && middlestatus && ringstatus && pinkystatus)
        {
            //Debug.Log("INDEX OPEN");
            getmouseDown = false;
            getChoice1KeyDown = false;
            getChoice2KeyDown = false;

        }
        else if (thumbstatus && !indexstatus && !middlestatus && ringstatus && pinkystatus)
        {
            //Debug.Log("VICTORY");
            getmouseDown = false;
            getChoice1KeyDown = false;
            getChoice2KeyDown = false;

        }
        else if (thumbstatus && !indexstatus && !middlestatus && !ringstatus && !pinkystatus)
        {
            //Debug.Log("SLAP GESTURE");
            getmouseDown = false;
            getChoice1KeyDown = false;
            getChoice2KeyDown = false;

        }
        else if (!thumbstatus && !indexstatus && !middlestatus && !ringstatus && !pinkystatus)
        {
            //Debug.Log("OPEN");
            getmouseDown = false;
            getChoice1KeyDown = false;
            getChoice2KeyDown = false;
        }
        else if (!thumbstatus && !indexstatus && !middlestatus && ringstatus && pinkystatus)
        {
            //Debug.Log("2L");
            getmouseDown = false;
            getChoice1KeyDown = false;
        }
        else if (!thumbstatus && !indexstatus && middlestatus && ringstatus && pinkystatus)
        {
            //Debug.Log("L");
            getmouseDown = false;
            getChoice2KeyDown = false;
        }
        else
        {
            //Debug.Log("OPEN");
            getmouseDown = false;
            getChoice1KeyDown = false;
            getChoice1KeyDown = false;
        }
    }

    bool getmouseDown;
    public bool GetMouseDown()
    {
        if (!thumbstatus && indexstatus && middlestatus && ringstatus && pinkystatus)
        {
            if (!getmouseDown)
            {
                getmouseDown = true;
                return true;
            }
            
        }
        return false;
    }

    bool getChoice1KeyDown;
    public bool GetChoice1KeyDown()
    {
        if (!thumbstatus && !indexstatus && middlestatus && ringstatus && pinkystatus)
        {
            if (!getChoice1KeyDown)
            {
                getChoice1KeyDown = true;
                return true;
            }

        }
        return false;
    }

    bool getChoice2KeyDown;
    public bool GetChoice2KeyDown()
    {
        if (!thumbstatus && !indexstatus && !middlestatus && ringstatus && pinkystatus)
        {
            if (!getChoice2KeyDown)
            {
                getChoice2KeyDown = true;
                return true;
            }
        }
        return false;
    }

    public float GetAxis(string axisName)
    {
        if(axisName == "Vertical")
        {
            if (thumbstatus && !indexstatus && middlestatus && ringstatus && pinkystatus)
            {
                return 1f;
            }
            else if (thumbstatus && !indexstatus && !middlestatus && ringstatus && pinkystatus)
            {
                return -1f;
            }
            else
            {
                return 0f;
            }
        }
        else if (axisName == "Horizontal")
        {
            if (!thumbstatus && indexstatus && middlestatus && ringstatus && pinkystatus)
            {
                return -1f;
            }
            else if (!thumbstatus && !indexstatus && middlestatus && ringstatus && pinkystatus)
            {
                return 1f;
            }
            else
            {
                return 0f;
            }
        }

        return 0f;
    }

    public float GetMouseAxis(string axisName)
    {
        if (axisName == "Vertical")
        {
                if(angleTracker.eulerAngles.x < 90)
                {
                    if (angleTracker.eulerAngles.x > 12f)
                    {
                        return -0.4f;
                    }
                    else if (angleTracker.eulerAngles.x < 12f)
                    {
                        return 0f;
                    }
                }
                else if(angleTracker.eulerAngles.x > 90)
                {
                    float angle = angleTracker.eulerAngles.x - 360;
                    if (angle < -12 && angle > -90)
                    {
                        return 0.4f;
                    }
                    else if(angle < -12)
                    {
                        return 0f;
                    }
                }
        }
        else if (axisName == "Horizontal")
        {
                if (angleTracker.eulerAngles.y < 90)
                {
                    if (angleTracker.eulerAngles.y > 10f)
                    {
                        return 1f;
                    }
                    else if (angleTracker.eulerAngles.y < 10f)
                    {
                        return 0f;
                    }
                }
                else if (angleTracker.eulerAngles.y > 90)
                {
                    float angle = angleTracker.eulerAngles.y - 360;
                    if (angle < -10 && angle > -90)
                    {
                        return -1f;
                    }
                    else if (angle < -10)
                    {
                        return 0f;
                    }
                }
        }

        return 0f;
    }
}
