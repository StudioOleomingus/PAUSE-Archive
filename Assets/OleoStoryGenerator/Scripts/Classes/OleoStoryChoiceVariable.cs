using System.Collections.Generic;
using UnityEngine;

namespace OleoStoryGenerator {

	[System.Serializable]
	public class OleoStoryChoiceVariable {

		public string id;

		public string variable_name;
		public string variable_value;

		public OleoStoryChoiceVariable (string id, string variable_name, string variable_value) {
			this.id = id;
			this.variable_name = variable_name;
			this.variable_value = variable_value;
		}
	}
}
