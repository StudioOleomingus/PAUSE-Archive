using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OleoStoryGenerator {

	[System.Serializable]
	public class OleoStoryCallback {

		public string id;
		public string function_Text;
		public string argument_Text;

		public OleoStoryCallback (string id) {
			this.id = id;
		}

		public OleoStoryCallback (string id, string function_Text, string argument_Text) {
			this.id = id;
			this.function_Text = function_Text;
			this.argument_Text = argument_Text;
		}
	}
}
