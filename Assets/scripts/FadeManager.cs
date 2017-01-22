using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeManager : MonoBehaviour {
	//float currentTime;
	public Image FadeImage;
	private bool isInTransition;
	private float  transition;
	private bool isShowing;
	private float duration;

	public static FadeManager Instance{set; get;}

	private void Awake ()
	{
		Instance = this;

	}

	public void Fade(bool showing,float duration)
	{
		isShowing = showing;
		isInTransition = true;
		this.duration = duration;
		transition = (isShowing) ? 0 : 1;
	}

	private void Update(){
			
		if (!isInTransition)
			return;
		//trying to show fade
		transition += (isShowing) ? Time.deltaTime * (1 / duration) : -Time.deltaTime * (1 / duration);
		FadeImage.color = Color.Lerp (new Color (1, 1, 1, 0), Color.white, transition);

		if (transition > 1 || transition < 0)
			isInTransition = false;
	}

}


//		void Start() {
//
//		}
//
//		// Update is called once per frame
//		void update () {
//			currentTime += Time.deltaTime;
//			if (currentTime >= 10f) {
//				Application.CancelQuit;
//				//int alphaFadeValue -= Mathf.Clamp01(Time.deltaTime / 5);
//				//GUI.color = new Color(0, 0, 0, alphaFadeValue);
//				GUI.DrawTexture (new Rect(0, 0, Screen.width, Screen.height), );
//
//			}
//
//		}
