using System.Collections.Generic;
using UnityEngine;

namespace OleoStoryGenerator {

	[System.Serializable]
	public class OleoStoryNode {
		public string id;
		public string title;
		public List<OleoStoryTag> story_Tags = new List<OleoStoryTag> ();
		public string story_Text;
		public List<OleoStoryInventory> story_inventories = new List<OleoStoryInventory> ();
		public List<OleoStoryCallback> story_callback = new List<OleoStoryCallback> ();
		public List<OleoStoryChoice> story_choices = new List<OleoStoryChoice> ();

		public OleoStoryNode (string id) {
			this.id = id;
		}

		public int GetStoryTagIndex (string id) {
			int index = -1;
			for (int i = 0; i < story_Tags.Count; i++) {
				if (story_Tags [i].id == id) {
					index = i;
					break;
				}
			}
			return index;
		}

		public int GetStoryChoiceIndex (string id) {
			int index = -1;
			for (int i = 0; i < story_choices.Count; i++) {
				if (story_choices [i].id == id) {
					index = i;
					break;
				}
			}
			return index;
		}
	}
}