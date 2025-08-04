using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace OleoStoryGenerator {
	
	public class OleoLayoutElement : MonoBehaviour {

		public RectTransform rectTransform;

		public enum LayoutType
		{
			TEXT,CHOICE,IMAGE
		};

		public LayoutType layoutType;

		[HideInInspector]
		public bool heightUpdated;
		[HideInInspector]
		public bool fadeInDone;
		
		public virtual void UpdateImage (Image image) {

		}

		public virtual void UpdateText (string text) {
			
		}

		public virtual void UpdateText (string text, string choiceText) {

		}

		public virtual void FadeOutTextColor () {

		}

		public virtual void FadeInTextColor (Color color) {

		}
	}

}
