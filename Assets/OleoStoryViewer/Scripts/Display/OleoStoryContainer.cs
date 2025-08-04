using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OleoStoryGenerator {
	
	public class OleoStoryContainer : MonoBehaviour {

		public static OleoStoryContainer instance;
		public GameObject savedStoryItemPrefab;
		public KilnDisplayManager KilnDisplayPanel;
		public Transform container;
		private List<OleoStory> stories = new List<OleoStory> ();

		void Awake () {
			instance = this;
		}

		// Use this for initialization
		void Start () {
			TextAsset[] textAssets = Resources.LoadAll<TextAsset> ("Stories/");
			foreach (TextAsset t in textAssets) {
				stories.Add (JsonUtility.FromJson<OleoStory> (t.text));
			}
			foreach(OleoStory story in stories) {
				GameObject item = Instantiate (savedStoryItemPrefab,Vector3.zero,Quaternion.identity) as GameObject;
				item.transform.SetParent (container,false);
				SavedStoryItem view = item.GetComponent<SavedStoryItem> ();
				view.storyName.text = story.story_Name;
			}
		}
		
		public void LoadStory (string storyName) {
			foreach (OleoStory story in stories) {
				if (story.story_Name == storyName) {
					KilnDisplayPanel.gameObject.SetActive (true);
					KilnDisplayPanel.Init (story);

					break;
				}
			}
		}

		public void OnClick_BackBtn () {
			KilnDisplayPanel.gameObject.SetActive (false);
		}
	}
}