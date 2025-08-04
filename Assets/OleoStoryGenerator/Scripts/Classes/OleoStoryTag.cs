using System.Collections.Generic;
using UnityEngine;

namespace OleoStoryGenerator {

	[System.Serializable]
	public class OleoStoryTag {
		public string id;
		public string tag_Text;

		public OleoStoryTag (string id) {
			this.id = id;
		}

		public OleoStoryTag (string id, string tag_Text) {
			this.id = id;
			this.tag_Text = tag_Text;
		}
	}
}
