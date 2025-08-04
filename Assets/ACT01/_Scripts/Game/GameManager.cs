using UnityEngine;
using OleoStoryGenerator;
using UnityStandardAssets.Characters.FirstPerson;
using System.Collections;
using UnityEngine.Audio;
using UnityStandardAssets.CrossPlatformInput;

public class GameManager : MonoBehaviour {

    public static GameManager instance;
    public KilnDisplayManager kilnDisplayManager;

    public TextAsset story_1;

    public string cutscenevalue;

    public StartingPanel startingPanel;
    public GameObject titlePanel;

    public float initialSecquenceTime = 5;

    public Transform inetractionImage;

    public AudioMixer mainMixture;

    public AudioSource telephoneSource;
    public AudioSource telephonePickupSource;

    public GameObject escape_panel;
    public GameObject pc_panel;
    public GameObject controller_panel;

    public float maxbgvolume = 0f;
    public float minbgvolume = -80f;

    private int helpSpriteIndex;
    private float currentHealthActiveTime;
    public float maxHelpPanelDisplayTime = 5f;
    private bool startHelpTimer;
    public GameObject helpPanel;
    public Sprite[] helpSpritesKeyboard;
    public Sprite[] helpSpritesJoystick;

    public UnityEngine.UI.Slider slider;
    public UnityEngine.UI.Slider slider_rotation_sensitivity;

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

        DontDestroyOnLoad(this.gameObject);
    }

    [HideInInspector]
    public bool startbgmusic;
    float loudMusicVolume;
    float softMusicVolume;

    bool frameCheck;

    private void Update()
    {
        if (startbgmusic)
        {
            if (kilnDisplayManager.gameObject.activeSelf)
            {
                mainMixture.GetFloat("loudbgvolume", out loudMusicVolume);
                mainMixture.SetFloat("loudbgvolume", Mathf.Lerp(loudMusicVolume, minbgvolume, Time.deltaTime * 0.1f));
                mainMixture.GetFloat("sofbgvolume", out softMusicVolume);
                mainMixture.SetFloat("sofbgvolume", Mathf.Lerp(softMusicVolume, maxbgvolume, Time.deltaTime * 0.4f));
            }
            else
            {
                mainMixture.GetFloat("loudbgvolume", out loudMusicVolume);
                mainMixture.SetFloat("loudbgvolume", Mathf.Lerp(loudMusicVolume, maxbgvolume, Time.deltaTime * 0.4f));
                mainMixture.GetFloat("sofbgvolume", out softMusicVolume);
                mainMixture.SetFloat("sofbgvolume", Mathf.Lerp(softMusicVolume, minbgvolume, Time.deltaTime * 0.1f));
            }
        }
        else
        {
            mainMixture.SetFloat("loudbgvolume", minbgvolume);
            mainMixture.SetFloat("sofbgvolume", minbgvolume);
        }

        

        if ((Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton6)) && !escape_panel.activeSelf)
        {
            frameCheck = true;
            StartCoroutine(SetFrameCheck());
            escape_panel.SetActive(true);
            if(OleoInputManager.instance.iNPUT_TYPE == OleoInputManager.INPUT_TYPE.Keyboard)
            {
                pc_panel.SetActive(true);
            }
            else if (OleoInputManager.instance.iNPUT_TYPE == OleoInputManager.INPUT_TYPE.Joystick)
            {
                controller_panel.SetActive(true);
            }
            FirstPersonController[] fps = GameObject.FindObjectsOfType<FirstPersonController>();
            foreach (FirstPersonController player in fps)
            {
                if (player.gameObject.activeSelf)
                {
                    player.DeActivatePlayer();
                }
            }
        }

        if (!frameCheck)
        {
            if (escape_panel != null && escape_panel.activeSelf)
            {
                if (controller_panel != null && controller_panel.activeSelf)
                {
                    if (Input.GetKeyDown(KeyCode.Joystick1Button2))
                    {
                        OnClick_QuitBtn();
                    }
                    if (Input.GetKeyDown(KeyCode.Joystick1Button0))
                    {
                        OnClick_RestartBtn();
                    }
                    if (Input.GetKeyDown(KeyCode.Joystick1Button1) || Input.GetKeyDown(KeyCode.JoystickButton6))
                    {
                        OnClick_CloseQuitBtn();
                    }
                    if (CrossPlatformInputManager.GetAxis("Horizontal") > 0.1f)
                    {
                        slider.value += Time.deltaTime;
                    }
                    else if (CrossPlatformInputManager.GetAxis("Horizontal") < -0.1f)
                    {
                        slider.value -= Time.deltaTime;
                    }

                    if (Input.GetAxis("JoystickMouse X") > 0.1f)
                    {
                        slider_rotation_sensitivity.value += Time.deltaTime;
                    }
                    else if (Input.GetAxis("JoystickMouse X") < -0.1f)
                    {
                        slider_rotation_sensitivity.value -= Time.deltaTime;
                    }
                }
                if (pc_panel != null && pc_panel.activeSelf)
                {
                    if (Input.GetKeyDown(KeyCode.Escape))
                    {
                        OnClick_CloseQuitBtn();
                    }
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.H) || Input.GetKeyDown(KeyCode.Joystick1Button1) && !escape_panel.activeSelf && !startingPanel.gameObject.activeSelf)
        {
            ProcessHelp();
        }

        if (startHelpTimer)
        {
            currentHealthActiveTime += Time.deltaTime;
            if (currentHealthActiveTime >= maxHelpPanelDisplayTime)
            {
                helpPanel.gameObject.SetActive(false);
                helpSpriteIndex = 0;
                currentHealthActiveTime = 0;
                startHelpTimer = false;
            }
        }


    }

    void ProcessHelp()
    {
        currentHealthActiveTime = 0;
        startHelpTimer = true;
        helpPanel.gameObject.SetActive(true);

        if (OleoInputManager.instance.iNPUT_TYPE == OleoInputManager.INPUT_TYPE.Joystick)
        {
            if (helpSpriteIndex < helpSpritesJoystick.Length)
                helpPanel.GetComponent<UnityEngine.UI.Image>().sprite = helpSpritesJoystick[helpSpriteIndex];
            helpSpriteIndex++;
            if (helpSpriteIndex > helpSpritesJoystick.Length)
            {
                helpPanel.gameObject.SetActive(false);
                helpSpriteIndex = 0;
                currentHealthActiveTime = 0;
            }
        }
        else if (OleoInputManager.instance.iNPUT_TYPE == OleoInputManager.INPUT_TYPE.Keyboard)
        {
            if (helpSpriteIndex < helpSpritesKeyboard.Length)
                helpPanel.GetComponent<UnityEngine.UI.Image>().sprite = helpSpritesKeyboard[helpSpriteIndex];
            helpSpriteIndex++;
            if (helpSpriteIndex > helpSpritesKeyboard.Length)
            {
                helpPanel.gameObject.SetActive(false);
                helpSpriteIndex = 0;
                currentHealthActiveTime = 0;
            }
        }
    }

    IEnumerator SetFrameCheck()
    {
        yield return new WaitForEndOfFrame();
        frameCheck = false;
    }
    public void OnCompleteStartingPanel ()
    {
        Invoke("ShowTitle", initialSecquenceTime);
        startingPanel.gameObject.SetActive(false);
        GameObject.Find("FPSController").GetComponent<FirstPersonController>().ActivatePlayer();
    }

    public void ShowTitle()
    {
        titlePanel.SetActive(true);
        GameObject.Find("FPSController").GetComponent<FirstPersonController>().DeActivatePlayer();
    }

    public void OnCompleteTitle()
    {
        titlePanel.SetActive(false);
        GameObject.Find("FPSController").GetComponent<FirstPersonController>().ActivatePlayer();
    }

    public void HookCallBackToTelephones(TelephoneController telephoneController)
    {
        foreach (Telephone t in telephoneController.telephones)
        {
            t.onPickRecieverAction = OnPickRecieverAction;
        }
    }

    void OnPickRecieverAction()
    {
        Debug.Log("ReturnToCallAction");
        if (KilnDisplayManager.instance != null && KilnDisplayManager.instance.cutscene)
        {
            Debug.Log("ReturnToCallAction 1");
            kilnDisplayManager.gameObject.SetActive(true);
            kilnDisplayManager.InitCutscene();
        }
        else
        {
            if(KilnDisplayManager.instance == null)
            {
                Debug.Log("ReturnToCallAction 2");
                kilnDisplayManager.gameObject.SetActive(true);
                OleoStory story = JsonUtility.FromJson<OleoStory>(story_1.text);
                kilnDisplayManager.Init(story);
            }
            
        }
    }

    public void OnReturnToGameModeAfterCall()
    {
        string[] split = cutscenevalue.Split(',');
        if(split[0] == "1")
        {
            StartCoroutine(FindPlayer("FPSController_Bottom"));
        }
        else if(split[0] == "0")
        {
            StartCoroutine(FindPlayer("FPSController"));
        }
        else if (split[0] == "2")
        {
            StartCoroutine(FindPlayer("FPSController_Bottom"));
        }
    }

    public void OnClick_QuitBtn()
    {
        Application.Quit();
    }

    public void OnClick_RestartBtn()
    {
        DestroyImmediate(gameObject);
        UnityEngine.SceneManagement.SceneManager.LoadScene("Scene_0");
    }

    public void OnClick_CloseQuitBtn()
    {
        FirstPersonController[] fps = GameObject.FindObjectsOfType<FirstPersonController>();
        if (!kilnDisplayManager.gameObject.activeSelf)
        {
            foreach (FirstPersonController player in fps)
            {
                if (player.gameObject.activeSelf)
                {
                    player.ActivatePlayer();
                }
            }
        }
        
        escape_panel.SetActive(false);
        pc_panel.SetActive(false);
        controller_panel.SetActive(false);
    }

    
    public void OnValueChange()
    {
        AudioSource[] a_sources = GameObject.FindObjectsOfType<AudioSource>();
        foreach(AudioSource a_s in a_sources)
        {
            a_s.volume = slider.value;
        }
    }
    public float rotationSensitivity = 1;
    public void OnValueChange_RotationSensitivity()
    {
        rotationSensitivity = slider_rotation_sensitivity.value;
    }

    public void GetRotationSensitivity()
    {
        FirstPersonController fps = GameObject.FindObjectOfType<FirstPersonController>();
        fps.M_MouseLook.slider_rotation_sensitivity = slider_rotation_sensitivity;
    }

    IEnumerator FindPlayer(string playerName)
    {
        yield return new WaitForSeconds(0.5f);
        GameObject player = GameObject.Find(playerName);
        player.GetComponent<FirstPersonController>().ActivatePlayer();
        InteractionController.instance.player = player.transform;
        InteractionController.instance.playerCamera = player.GetComponent<FirstPersonController>().m_Camera.transform;
        InteractionController.instance.player_controller = player.GetComponent<FirstPersonController>();
        InteractionController.instance.current_InteractiveObject = null;
        InteractionController.instance.last_InteractiveObject = null;
    }

    
}
