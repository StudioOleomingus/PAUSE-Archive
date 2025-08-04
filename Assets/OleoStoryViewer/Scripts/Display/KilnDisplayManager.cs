using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace OleoStoryGenerator {
	
	public class KilnDisplayManager : MonoBehaviour {

		public static KilnDisplayManager instance;

		public OleoLayoutElement storyDialoguePrefab;
		public OleoLayoutElement storyChoicePrefab;
        public OleoLayoutElement storyCutscenePrefab;
        public OleoLayoutElement storyInventoryPrefab;
		public OleoIllustrationImage storyIllustrationImagePrefab;

		public GameObject exitBtn;

		public AudioSource otherAudioSource;
        public AudioSource dialogueAudioSource;

        private Dictionary<string, string> inventoryContainer = new Dictionary<string, string>();

		OleoStory story;

		public System.Action OnStoryEnd;
		public System.Action<string,string> StoryCallBacks;

        public bool cutscene;
        private List<OleoStoryChoice> cutscene_choices = new List<OleoStoryChoice>();

        public MotionDetection motionDetection;
        private int motionDetectionChoiceIndex;

        public UnityEngine.UI.Toggle musicToggle;

        void Awake () {
			instance = this;
		}

        void Update()
        {
            if(OleoInputManager.instance.iNPUT_TYPE == OleoInputManager.INPUT_TYPE.Joystick)
            {
                if (Input.GetKeyDown(KeyCode.Joystick1Button0))
                {
                    musicToggle.isOn = !musicToggle.isOn;
                }
            }
        }

        // Initializing story
        public void Init (OleoStory story, string startTitle = "") {
			inventoryContainer.Clear();
			this.story = story;
			StartCoroutine(StartStory (startTitle));
		}

        public void InitCutscene()
        {
            cutscene = false;
            InteractionController.instance.player_controller.DeActivatePlayer();
            LoadNextNode(loadnextnodetitle, _choiceElement);
            /*
            OleoLayout.instance.ClearChoices();

            SetCursorLock();

            foreach (OleoStoryChoice choice in cutscene_choices)
            {

                bool canShowThisChoice = true;

                // checking through all the list in the array of choice variable and comparing with inventory array
                // if the variable in choice array does not match to any element in inventory array then we wont show the choice
                // even if the variable matches a element in inventory array we are check if its value matches or not
                foreach (OleoStoryChoiceVariable variable in choice.choice_variables)
                {

                    if (!inventoryContainer.ContainsKey(variable.variable_name))
                    {
                        canShowThisChoice = false;
                    }
                    else
                    {
                        if (inventoryContainer[variable.variable_name] != variable.variable_value)
                        {
                            canShowThisChoice = false;
                        }
                    }
                }

                if (canShowThisChoice)
                {
                    OleoLayoutElement eleC = Instantiate(storyChoicePrefab) as OleoLayoutElement;
                    eleC.transform.SetParent(OleoLayout.instance.transform, false);
                    eleC.UpdateText(choice.link_Text, choice.choice_Text);
                }
            }

            OleoLayout.instance.UpdateView();
            */
        }

        IEnumerator StartStory (string startTitle = "") {
			yield return new WaitForEndOfFrame ();
			OleoLayout.instance.ClearContent ();
			yield return new WaitForSeconds (0.7f);
			if(string.IsNullOrEmpty(startTitle))
				CreateStoryNode (story.story_Node[0].title);
			else
				CreateStoryNode (startTitle);

            if(OleoInputManager.instance.iNPUT_TYPE != OleoInputManager.INPUT_TYPE.Joystick)
                SetCursorLock();
        }

        public void SetCursorLock()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        /// <summary>
        /// Creates the story node.
        /// </summary>
        /// <param name="title">title is used to search specific node in the node list.</param>
        /// <param name="initialString">initialString is added to the story node if there is - at the end of a choice.</param>
        public void CreateStoryNode (string title, string initialString = "") {
			OleoLayout.instance.ClearChoices ();
            OleoInputManager.instance.dialogueIndex = 0;
            OleoInputManager.instance.dialogueLength = 0;
            OleoInputManager.instance.choices.Clear();
            motionDetectionChoiceIndex = 0;

            foreach (OleoStoryNode node in story.story_Node) {
				if (node.title == title) {
					OleoLayout.instance.SetPreviousChildColors ();

					foreach(OleoStoryInventory inv in node.story_inventories) {
						if (!inventoryContainer.ContainsKey (inv.key_Text)) {
							inventoryContainer.Add (inv.key_Text, inv.value_Text);
						} else {
							inventoryContainer [inv.key_Text] = inv.value_Text;
						}
					}

					foreach(OleoStoryCallback callback in node.story_callback) {
                        if (callback.function_Text == "ShowIllustration" && File.Exists(Application.streamingAssetsPath + "/" + callback.argument_Text + ".png"))
                        {
                            OleoIllustrationElement eleI = Instantiate(storyInventoryPrefab) as OleoIllustrationElement;
                            eleI.transform.SetParent(OleoLayout.instance.transform, false);


                            OleoIllustrationImage eleImg = Instantiate(storyIllustrationImagePrefab) as OleoIllustrationImage;
                            eleImg.transform.SetParent(OleoLayout.instance.illustrationViewPort.transform, false);
                            Texture2D texture = LoadPNG(Application.streamingAssetsPath + "/" + callback.argument_Text + ".png");
                            Sprite sprite = Sprite.Create(texture as Texture2D, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
                            eleImg.image.sprite = sprite;
                            eleImg.SetSize();
                            eleImg.target = eleI.transform;
                            eleI.UpdateImage(eleImg.image);
                        }
                        else if (callback.function_Text == "PlaySound")
                        {
                            bool dialogueSound = false;
                            if (callback.argument_Text.Contains("dialogue"))
                                dialogueSound = true;
                            StartCoroutine(LoadClipCoroutine("file:///" + Application.streamingAssetsPath + "/" + callback.argument_Text + ".ogg", dialogueSound));
                        }
                        else if (callback.function_Text == "Cutscene")
                        {
                            cutscene = true;
                            cutscene_choices = node.story_choices;
                            GameManager.instance.cutscenevalue = callback.argument_Text;
                        }
                        else if (callback.function_Text == "GoToGame")
                        {
                            cutscene = true;
                            cutscene_choices = node.story_choices;
                            GameManager.instance.cutscenevalue = callback.argument_Text;
                            string[] split = callback.argument_Text.Split(',');
                            UnityEngine.SceneManagement.SceneManager.LoadScene("Scene_"+ split[0]);
                        }

                        if (StoryCallBacks != null) {
							StoryCallBacks(callback.function_Text,callback.argument_Text);
						}
					}

					OleoLayoutElement eleT = Instantiate (storyDialoguePrefab) as OleoLayoutElement;
					eleT.transform.SetParent (OleoLayout.instance.transform,false);
					eleT.UpdateText (initialString+node.story_Text);

                    //if (cutscene)
                    //{
                    //    OleoLayoutElement eleC = Instantiate(storyCutscenePrefab) as OleoLayoutElement;
                    //    eleC.transform.SetParent(OleoLayout.instance.transform, false);
                    //    eleC.UpdateText(initialString + node.story_Text);

                    //    goto Callback;
                    //}
                    

                    foreach (OleoStoryChoice choice in node.story_choices) {

						bool canShowThisChoice = true;

						// checking through all the list in the array of choice variable and comparing with inventory array
						// if the variable in choice array does not match to any element in inventory array then we wont show the choice
						// even if the variable matches a element in inventory array we are check if its value matches or not
						foreach(OleoStoryChoiceVariable variable in choice.choice_variables) {
							
							if (!inventoryContainer.ContainsKey (variable.variable_name)) {
								canShowThisChoice = false;
							}
							else {
								if (inventoryContainer [variable.variable_name] != variable.variable_value) {
									canShowThisChoice = false;
								}
							}
						}

						if(canShowThisChoice) {
                            OleoLayoutElement eleC = Instantiate (storyChoicePrefab) as OleoLayoutElement;
							eleC.transform.SetParent (OleoLayout.instance.transform, false);
							eleC.UpdateText (choice.link_Text, choice.choice_Text);
                            eleC.GetComponent<OleoChoiceElement>().SetMotionDetection(motionDetection, motionDetectionChoiceIndex);
                            OleoInputManager.instance.choices.Add(eleC.gameObject);
                            eleC.name = eleC.name + ""+ eleC.transform.childCount;
                            motionDetectionChoiceIndex++;
                            if (OleoInputManager.instance.dialogueLength == 0)
                            {
                                eleC.GetComponent<OleoChoiceElement>().On_PointerEnter();
                            }
                            OleoInputManager.instance.dialogueLength++;
                        }
					}

					if(node.story_choices.Count <= 0) {
						if(OnStoryEnd != null)
							OnStoryEnd();
					}
				}
			}

            //Callback:

            OleoLayout.instance.UpdateView ();
		}

        public void ReturnToGame()
        {
            GameManager.instance.OnReturnToGameModeAfterCall();
            CutScenePositionHandler.instance.HandlePosition(GameManager.instance.cutscenevalue);
            //TelephoneController.instance.PutRecieverOnHold();
            gameObject.SetActive(false);
        }

        private string loadnextnodetitle = "";
        private OleoChoiceElement _choiceElement;

        public void LoadNextNode (string title, OleoChoiceElement obj) {
            if (cutscene)
            {
                loadnextnodetitle = title;
                _choiceElement = obj;
                ReturnToGame();
                return;
            }
			OleoLayout.instance.selectedChoice = obj;

			if(obj.choiceText2.text[obj.choiceText2.text.Length-1] == '-')
				CreateStoryNode (title,obj.choiceText2.text+" ");
			else
				CreateStoryNode (title);
		}

		#region UI
		public void OnClick_1QuitButton() {
			OleoLayout.instance.ClearContent ();
			gameObject.SetActive(false);
		}
		#endregion

		public Texture2D LoadPNG(string filePath) {

			Texture2D tex = null;
			byte[] fileData;
			if (File.Exists(filePath))     {
				fileData = File.ReadAllBytes(filePath);
				tex = new Texture2D(2, 2);
				tex.LoadImage(fileData); //..this will auto-resize the texture dimensions.
			}
			return tex;
		}

		IEnumerator LoadClipCoroutine(string file, bool isDialogueSound = false)
		{
			WWW request = new WWW(file);
			yield return request;

			if (string.IsNullOrEmpty (request.error)) {

				AudioClip loadedclip = request.GetAudioClip ();

                if (otherAudioSource.clip != null && !otherAudioSource.isPlaying)
                {
                    otherAudioSource.Stop();
                    AudioClip clip = otherAudioSource.clip;
                    otherAudioSource.clip = null;
                    clip.UnloadAudioData();
                    DestroyImmediate(clip, false); // This is important to avoid memory leak
                }

                if (dialogueAudioSource.clip != null && !dialogueAudioSource.isPlaying)
                {
                    dialogueAudioSource.Stop();
                    AudioClip clip = dialogueAudioSource.clip;
                    dialogueAudioSource.clip = null;
                    clip.UnloadAudioData();
                    DestroyImmediate(clip, false); // This is important to avoid memory leak
                }

                if (loadedclip != null) {
                    if (isDialogueSound)
                    {
                        musicToggle.gameObject.SetActive(true);
                        if (musicToggle.isOn)
                        {
                            dialogueAudioSource.clip = loadedclip;
                            dialogueAudioSource.Play();
                        }
                    }
                    else
                    {
                        otherAudioSource.clip = loadedclip;
                        otherAudioSource.Play();
                    }
                    
				}


			} else {
				Debug.LogWarning ("Music file not found "+request.error);
			}
			request.Dispose();
		}

        public void OnValueChange_MusicToggle()
        {
            Debug.Log("OnValueChange_MusicToggle");
            if(musicToggle.isOn == false)
            {
                musicToggle.GetComponentInChildren<UnityEngine.UI.Text>().text = "off";
                if (dialogueAudioSource.clip != null)
                    dialogueAudioSource.Pause();
            }
            else
            {
                musicToggle.GetComponentInChildren<UnityEngine.UI.Text>().text = "on";
                if (dialogueAudioSource.clip != null)
                    dialogueAudioSource.Play();
            }
        }

    }
}
