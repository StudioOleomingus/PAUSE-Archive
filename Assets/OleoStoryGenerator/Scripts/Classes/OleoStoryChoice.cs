using System.Collections.Generic;
using UnityEngine;

namespace OleoStoryGenerator {

	[System.Serializable]
	public class OleoStoryChoice {
		public string id;
		public string choice_Text;
		public string link_Text;

		public List<OleoStoryChoiceVariable> choice_variables = new List<OleoStoryChoiceVariable> ();

		public OleoStoryChoice (string id) {
			this.id = id;
		}

		public OleoStoryChoice (string id, string choice_Text) {
			this.id = id;
			this.choice_Text = choice_Text;
		}

		public OleoStoryChoice (string id, string choice_Text, string link) {
			this.id = id;
			this.choice_Text = choice_Text;
			this.link_Text = link;
		}
	}
}
