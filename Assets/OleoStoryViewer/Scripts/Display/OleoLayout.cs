using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace OleoStoryGenerator {
	
	public class OleoLayout : MonoBehaviour {

		public static OleoLayout instance;

		[SerializeField]
		private float MARGIN_TOP;
		[SerializeField]
		private float MARGIN_LEFT;
		[SerializeField]
		private float MARGIN_RIGHT;
		[SerializeField]
		private float MARGIN_BOTTOM;
		[SerializeField]
		private float SPACING;
		[SerializeField]
		private RectTransform m_rectTransform;

		public RectTransform canvas;
		public RectTransform viewPort;
		public RectTransform illustrationViewPort;

		public float lastHeight;
		public int lastChildCount;

		[HideInInspector]
		public bool canClick;

		public float Width {
			get { return m_rectTransform.sizeDelta.x-MARGIN_LEFT-MARGIN_RIGHT;}
		}

		public float Height {
			get { return m_rectTransform.sizeDelta.y-MARGIN_TOP-MARGIN_BOTTOM;}
		}

		public float MAX_FADEOUT_ALPHA = 0.4f;
		public float MAX_FADEIN_ALPHA = 1.0f;

		public int GetFontSize () {
			
			int fontSize = 16;
			if (Screen.width >= 1920) {
				fontSize = 26;
			} else if (Screen.width < 1920 && Screen.width >= 1600) {
				fontSize = 26;
			} else if (Screen.width < 1600 && Screen.width >= 1366) {
				fontSize = 21;
			} else if (Screen.width < 1366) {
				fontSize = 18;
			}
			return fontSize;
		}

		[HideInInspector]
		public OleoChoiceElement selectedChoice;

		void Awake () {

			instance = this;
			ResetLayout ();

		}

		void ResetLayout () {
			m_rectTransform.anchoredPosition = Vector2.zero;
			m_rectTransform.sizeDelta = new Vector2 (viewPort.rect.width,viewPort.rect.height);

			lastHeight = m_rectTransform.sizeDelta.y;
			lastChildCount = 0;
		}


		#region DragHandler

		void OnGUI () {
			if(Event.current.type == EventType.MouseDrag 
				&& (m_rectTransform.anchoredPosition.y <= (m_rectTransform.sizeDelta.y-viewPort.rect.height))
				&& (m_rectTransform.anchoredPosition.y >= 0)
			) {
				//Debug.Log (m_rectTransform.anchoredPosition.y+" "+(m_rectTransform.sizeDelta.y-canvas.sizeDelta.y));
                m_rectTransform.anchoredPosition += new Vector2(0,-Event.current.delta.y);
                
				Event.current.Use();
			}

            if (OleoInputManager.instance.iNPUT_TYPE == OleoInputManager.INPUT_TYPE.Joystick)
            {
                m_rectTransform.anchoredPosition += new Vector2(0, -Input.GetAxis("D-Pad Y Axis")*4);
            }

            if ((m_rectTransform.anchoredPosition.y > (m_rectTransform.sizeDelta.y - viewPort.rect.height)))
                m_rectTransform.anchoredPosition = new Vector2(0, (m_rectTransform.sizeDelta.y - viewPort.rect.height));
            if (m_rectTransform.anchoredPosition.y < 0)
                m_rectTransform.anchoredPosition = new Vector2(0, 0);
        }

		#endregion



		#region UI

		public void OnClick_Down () {
			StartCoroutine (ResetPosition());
		}

		#endregion



		#region Public Methods

		public void UpdateView () {
			canClick = false;
			m_rectTransform.sizeDelta = new Vector2(viewPort.rect.width,m_rectTransform.sizeDelta.y);
			SetChildWidths ();
			StartCoroutine(SetHeight ());

		}

		public void SetPreviousChildColors () {
			for (int i = 0; i < transform.childCount; i++) {
				OleoLayoutElement ele = transform.GetChild (i).GetComponent<OleoLayoutElement> ();
				ele.FadeOutTextColor ();
			}
		}

		public void ClearContent () {

			for (int i = 0; i < transform.childCount; i++) {
				Destroy (transform.GetChild(i).gameObject);
			}

			for (int i = illustrationViewPort.transform.childCount - 1; i >= 0; i--) {
				Destroy (illustrationViewPort.transform.GetChild (i).gameObject);
			}

			ResetLayout ();
		}

		public void ClearChoices () {
			for (int i = transform.childCount-1; i >= 0; i--) {
				OleoLayoutElement ele = transform.GetChild (i).GetComponent<OleoLayoutElement> ();
				if (ele.layoutType == OleoLayoutElement.LayoutType.CHOICE) {
					if (selectedChoice.gameObject == ele.gameObject) {
						if (selectedChoice.choiceText2.text [selectedChoice.choiceText2.text.Length - 1] != '-') {
							selectedChoice.choiceText2.fontStyle &= ~TMPro.FontStyles.Underline;
						} else {
							Destroy (ele.gameObject);
							lastChildCount--;
						}

					} else {
						Destroy (ele.gameObject);
						lastChildCount--;
					}

				} else {
					break;
				}
			}
		}

		#endregion



		#region Private Methods
		void SetChildWidths () {
			
			for(int i = 0; i < transform.childCount; i++) {
				RectTransform rectTransform = transform.GetChild (i).GetComponent<RectTransform>();
				Vector2 size = new Vector2 (m_rectTransform.rect.width-MARGIN_LEFT-MARGIN_RIGHT,rectTransform.sizeDelta.y);
				rectTransform.sizeDelta = size;
			}

		}


		void SetChildsAnchors () {

			float x = MARGIN_LEFT;
			float y = -MARGIN_TOP;


			for(int i = 0; i < transform.childCount; i++) {

				RectTransform rectTransform = transform.GetChild (i).GetComponent<RectTransform>();
				rectTransform.anchorMin = new Vector2(0,1);
				rectTransform.anchorMax = new Vector2(0,1);
				rectTransform.anchoredPosition = new Vector2(x,y);

				y -= rectTransform.sizeDelta.y + SPACING;
			}
		}

		void ShiftLayout(float value) {
			m_rectTransform.anchoredPosition = new Vector2 (0,value);
		}

		void OnCompleteShift () {
			StartCoroutine (TextfadeIn());
		}

		#endregion


		#region Coroutine

		IEnumerator SetHeight () {

			float height = MARGIN_TOP;
			int index = 0;
			while (index < transform.childCount) {
				OleoLayoutElement ele = transform.GetChild (index).GetComponent<OleoLayoutElement> ();
				while (!ele.heightUpdated) {
					yield return null;
				}

				height = height + ele.rectTransform.sizeDelta.y  + SPACING;
				index++;
				yield return null;
			}

			height = Mathf.Abs (height) + MARGIN_BOTTOM;

			if (height > viewPort.rect.height) {
				m_rectTransform.sizeDelta = new Vector2 (viewPort.rect.width, height);

				if (height > lastHeight) {

					float increasedHeight = (height - lastHeight);

					if (increasedHeight >= viewPort.rect.height) {
						increasedHeight = viewPort.rect.height - 200;
					}

					float targetY = m_rectTransform.anchoredPosition.y + increasedHeight;

					iTween.ValueTo (gameObject, iTween.Hash (
						"from", m_rectTransform.anchoredPosition.y,
						"to", targetY,
						"delay", 0.3f,
						"easetype", iTween.EaseType.easeInOutSine,
						"onupdate", "ShiftLayout",
						"onupdatetarget", gameObject,
						"oncomplete", "OnCompleteShift",
						"oncompletetarget", gameObject));
				} else {
					OnCompleteShift ();
				}
				lastHeight = height;
			} else {
				OnCompleteShift ();
			}

			SetChildsAnchors ();
		}


		IEnumerator TextfadeIn() {
			
			int i = transform.childCount - (transform.childCount-lastChildCount);
			while(i < transform.childCount) {
				OleoLayoutElement ele = transform.GetChild (i).GetComponent<OleoLayoutElement> ();
				ele.FadeInTextColor (Color.black);
				while (!ele.fadeInDone) {
					yield return null;
				}
				i++;
			}
			lastChildCount = transform.childCount;
            canClick = true;
        }


		IEnumerator ResetPosition() {
			while(m_rectTransform.anchoredPosition.y < (m_rectTransform.sizeDelta.y - viewPort.rect.height) - 0.5f) {
				m_rectTransform.anchoredPosition = Vector2.Lerp (m_rectTransform.anchoredPosition,new Vector2( 0, (m_rectTransform.sizeDelta.y - viewPort.rect.height)), Time.deltaTime * 5);
				yield return null;
			}
		}

		#endregion
	}

}