using UnityEngine;
using UnityEngine.UI;

namespace OleoStoryGenerator {
	
	public class OleoIllustrationImage : MonoBehaviour {

		public Image image;
		public Transform target;

		void Update () {
			if(target != null)
				transform.position = new Vector3(transform.position.x, target.position.y, transform.position.z);
		}

		public void SetSize () {
			GetComponent<RectTransform>().sizeDelta = new Vector2(OleoLayout.instance.illustrationViewPort.rect.width,OleoLayout.instance.illustrationViewPort.rect.height);
		}
	}

}