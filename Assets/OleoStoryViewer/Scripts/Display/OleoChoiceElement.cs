using System.Collections;
using UnityEngine;
using TMPro;

namespace OleoStoryGenerator {
	
	public class OleoChoiceElement : OleoLayoutElement {

		/// <summary>
		/// Temp text to set the height and width of the rect transform
		/// </summary>
		public TMP_Text choiceText1;
		/// <summary>
		/// Contains the choice text
		/// </summary>
		public TMP_Text choiceText2;

		public RectTransform choiceBtn;

		public string linked_Node;

        public GameObject knob;

        public bool clicked;

        public int choiceIndex;
        private MotionDetection motionDetection;

        public void SetMotionDetection(MotionDetection obj, int _index)
        {
            choiceIndex = _index;
            motionDetection = obj;
        }

        private void Update()
        {
            if (motionDetection.GetChoice1KeyDown() && choiceIndex == 0 && !clicked)
            {
                knob.SetActive(true);
                OnClick_1ChoiceBtn();
            }
            else if (motionDetection.GetChoice1KeyDown() && choiceIndex == 1 && !clicked)
            {
                knob.SetActive(true);
                OnClick_1ChoiceBtn();
            }
        }

        /// <summary>
        /// Updates the text and height also stores the value of next linked node.
        /// </summary>
        public override void UpdateText (string linked_Node, string choice_text)
		{
			rectTransform.sizeDelta = new Vector2(OleoLayout.instance.Width,rectTransform.sizeDelta.y);

			choiceText1.fontSize = OleoLayout.instance.GetFontSize ();
			choiceText2.fontSize = OleoLayout.instance.GetFontSize ();

			choiceText1.text = choice_text;
			this.linked_Node = linked_Node;
			StartCoroutine (UpdateHeightCoroutine());
		}

		/// <summary>
		/// Fade out the color of the text to certain amount of alpha.
		/// </summary>
		public override void FadeOutTextColor ()
		{
			choiceBtn.GetComponent<UnityEngine.UI.Button> ().interactable = false;
			StartCoroutine (FadeOutCoroutine ());
		}

		/// <summary>
		/// Fade in the color of the text to certain amount of alpha.
		/// </summary>
		public override void FadeInTextColor (Color color)
		{
			StartCoroutine (FadeInCoroutine());
		}

		#region UI.OnClick
		public void OnClick_1ChoiceBtn () {
			if(OleoLayout.instance.canClick && !clicked)
            {
                clicked = true;
                KilnDisplayManager.instance.LoadNextNode(linked_Node, this);
                this.enabled = false;
            }
				
		}

        private bool pointerEntered;

        public void On_PointerEnter()
        {
            pointerEntered = true;
            if (fadeInDone && !clicked)
                knob.SetActive(true);
        }

        public void On_PointerExit()
        {
            pointerEntered = false;
            if (!clicked)
                knob.SetActive(false);
        }
        #endregion


        #region Coroutine
        IEnumerator UpdateHeightCoroutine () {
			yield return new WaitForEndOfFrame ();
			yield return new WaitForSeconds (0.1f);
			choiceBtn.anchoredPosition = Vector2.zero;
			choiceBtn.sizeDelta = new Vector2 (rectTransform.rect.width,rectTransform.rect.height);

			choiceText2.text = choiceText1.text;
			heightUpdated = true;
		}

		IEnumerator FadeInCoroutine() {
			float alpha = 0;
			while (alpha < OleoLayout.instance.MAX_FADEIN_ALPHA) {
				alpha += Time.deltaTime * 2;
				choiceText2.color = new Color (choiceText2.color.r,choiceText2.color.g,choiceText2.color.b,alpha);
				yield return null;
			}
			fadeInDone = true;
            if(pointerEntered)
                knob.SetActive(true);
        }

		IEnumerator FadeOutCoroutine () {
			Color c = choiceText2.color;
			while (choiceText2.color.a > OleoLayout.instance.MAX_FADEOUT_ALPHA) {
				choiceText2.color = Color.Lerp (choiceText2.color,new Color(c.r,c.g,c.b,OleoLayout.instance.MAX_FADEOUT_ALPHA), Time.deltaTime * 5);
				yield return null;
			}
		}

		#endregion
	}
}