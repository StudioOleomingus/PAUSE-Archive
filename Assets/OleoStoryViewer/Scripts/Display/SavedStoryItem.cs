using UnityEngine;

namespace OleoStoryGenerator {
	
	public class SavedStoryItem : MonoBehaviour {

		public UnityEngine.UI.Text storyName;

		public void OnClick_StoryBtn () {
			OleoStoryContainer.instance.LoadStory (storyName.text);
		}
	}
}