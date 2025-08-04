using System.Collections;
using UnityEngine;

namespace OleoStoryGenerator
{
    public class OleoCutsceneElement : OleoLayoutElement
    {
        public UnityEngine.UI.Image arrowImage;

        private void Start()
        {
            heightUpdated = true;
        }

        public void OnClick_ContinueBtn()
        {
            KilnDisplayManager.instance.ReturnToGame();
        }

        /// <summary>
		/// Fade in the color of the text to certain amount of alpha.
		/// </summary>
		public override void FadeInTextColor(Color color)
        {
            StartCoroutine(FadeInCoroutine());
        }

        IEnumerator FadeInCoroutine()
        {
            float alpha = 0;
            while (alpha < OleoLayout.instance.MAX_FADEIN_ALPHA)
            {
                alpha += Time.deltaTime;
                arrowImage.color = new Color(arrowImage.color.r, arrowImage.color.g, arrowImage.color.b, alpha);
                yield return null;
            }
            fadeInDone = true;
        }
    }

}
