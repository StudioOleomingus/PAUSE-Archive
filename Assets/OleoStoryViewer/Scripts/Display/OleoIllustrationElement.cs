using System.Collections;
using UnityEngine;

namespace OleoStoryGenerator {

	public class OleoIllustrationElement : OleoLayoutElement {

		private UnityEngine.UI.Image image;

		/// <summary>
		/// Updates the image size.
		/// </summary>
		public override void UpdateImage (UnityEngine.UI.Image image)
		{
			this.image = image;
			rectTransform.sizeDelta = new Vector2 (OleoLayout.instance.viewPort.rect.width,OleoLayout.instance.illustrationViewPort.rect.height);
			heightUpdated = true;
		}

		/// <summary>
		/// Fade in the color of the text to certain amount of alpha.
		/// </summary>
		public override void FadeInTextColor (Color color)
		{
			StartCoroutine (FadeInCoroutine());
		}

		#region Coroutine
		IEnumerator FadeInCoroutine() {
			float alpha = 0;
			while (alpha < OleoLayout.instance.MAX_FADEIN_ALPHA) {
				alpha += Time.deltaTime;
				image.color = new Color (image.color.r,image.color.g,image.color.b,alpha);
				yield return null;
			}
			fadeInDone = true;
		}
		#endregion
	}
}
