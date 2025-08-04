using UnityEngine;
using UnityEngine.UI;

public class StartingPanel : MonoBehaviour {
    
    public Sprite keyboardControl_sprite;
    public Sprite joystickControl_sprite;

    private int index = 0;
    public RectTransform[] images;

    public AudioSource startingSound;
    float wide;

    float screenposition;

    public MotionDetection motionDetection;

    // Use this for initialization
    public void Init () {
        gameObject.SetActive(true);
        wide = 1920;
        for (int i = 0; i < images.Length; i++)
        {
            images[i].offsetMin = new Vector2(wide * i, 0);
            images[i].offsetMax = new Vector2(wide * i, 0);
        }
        startingSound.Play();
        if(OleoInputManager.instance.iNPUT_TYPE == OleoInputManager.INPUT_TYPE.Joystick)
        {
            images[0].GetComponent<Image>().sprite = joystickControl_sprite;
        }
        else if (OleoInputManager.instance.iNPUT_TYPE == OleoInputManager.INPUT_TYPE.Keyboard)
        {
            images[0].GetComponent<Image>().sprite = keyboardControl_sprite;
        }
    }

    private void Update()
    {
        if ((Input.anyKeyDown || motionDetection.GetMouseDown()) && !GameManager.instance.escape_panel.activeSelf)
        {
            OnClick_Continue();
        }

        for (int i = 0; i < images.Length; i++)
        {
            images[i].offsetMin = Vector2.Lerp(images[i].offsetMin, new Vector2(screenposition + (wide * i), 0), Time.deltaTime * 10);
            images[i].offsetMax = Vector2.Lerp(images[i].offsetMax, new Vector2(screenposition + (wide * i), 0), Time.deltaTime * 10);
        }
    }

    public void OnClick_Continue()
    {
        screenposition -= 1920;
        index++;
        if(index < images.Length)
        {
            startingSound.Play();
        }
        else
        {
            GameManager.instance.OnCompleteStartingPanel();
            GameManager.instance.telephoneSource.Play();
        }
    }
}
