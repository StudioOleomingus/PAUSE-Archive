using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OleoStoryGenerator {
	
	[System.Serializable]
	public class OleoStoryInventory {

		public string id;
		public string key_Text;
		public string value_Text;

		public OleoStoryInventory (string id) {
			this.id = id;
		}

		public OleoStoryInventory (string id, string key_Text, string value_text) {
			this.id = id;
			this.key_Text = key_Text;
			this.value_Text = value_text;
		}
	}
}