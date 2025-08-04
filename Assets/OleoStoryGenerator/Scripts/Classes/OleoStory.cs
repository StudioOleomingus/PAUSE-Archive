using System.Collections.Generic;
using UnityEngine;

namespace OleoStoryGenerator {
	
	[System.Serializable]
	public class OleoStory {
		public string editor_version;
		public string story_Name;
		public string story_Value;
		public string story_Time;

		public List<OleoStoryNode> story_Node = new List<OleoStoryNode> ();
		public List<OleoStoryNodeConnections> story_Node_Connections = new List<OleoStoryNodeConnections> ();

		public OleoStory (string story_Name) {
			this.story_Name = story_Name;
		}

		public int GetStoryNodeIndex (string id) {
			int index = -1;
			for (int i = 0; i < story_Node.Count; i++) {
				if (story_Node [i].id == id) {
					index = i;
					break;
				}
			}
			return index;
		}

		public string story_Comments;
	}
}
