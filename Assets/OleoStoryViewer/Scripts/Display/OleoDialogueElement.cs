using System.Collections;
using UnityEngine;
using TMPro;

namespace OleoStoryGenerator {

	public class OleoDialogueElement : OleoLayoutElement {

		/// <summary>
		/// Contains the dialogue text
		/// </summary>
		public TMP_Text dialogueText;

		/// <summary>
		/// Updates the text and height.
		/// </summary>
		public override void UpdateText (string data)
		{
			rectTransform.sizeDelta = new Vector2(OleoLayout.instance.Width,rectTransform.sizeDelta.y);
			dialogueText.fontSize = OleoLayout.instance.GetFontSize ();

			dialogueText.text = data;
			StartCoroutine (UpdateHeightCoroutine());
		}

		/// <summary>
		/// Fade out the color of the text to certain amount of alpha.
		/// </summary>
		public override void FadeOutTextColor ()
		{
			StartCoroutine (FadeOutCoroutine ());
		}

		/// <summary>
		/// Fade in the color of the text to certain amount of alpha.
		/// </summary>
		public override void FadeInTextColor (Color color)
		{
			StartCoroutine (FadeInCoroutine());
		}

		#region Coroutine
		IEnumerator UpdateHeightCoroutine () {
			yield return new WaitForEndOfFrame ();
			yield return new WaitForSeconds (0.1f);
			heightUpdated = true;
		}
		IEnumerator FadeInCoroutine() {
			float alpha = 0;
			while (alpha < OleoLayout.instance.MAX_FADEIN_ALPHA) {
				alpha += Time.deltaTime;
				dialogueText.color = new Color (dialogueText.color.r,dialogueText.color.g,dialogueText.color.b,alpha);
				yield return null;
			}
			fadeInDone = true;
		}

		IEnumerator FadeOutCoroutine () {
			Color c = dialogueText.color;
			while (dialogueText.color.a > OleoLayout.instance.MAX_FADEOUT_ALPHA) {
				dialogueText.color = Color.Lerp (dialogueText.color,new Color(c.r,c.g,c.b,OleoLayout.instance.MAX_FADEOUT_ALPHA), Time.deltaTime * 5);
				yield return null;
			}
		}
		#endregion
	}
}