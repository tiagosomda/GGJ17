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
		FadeImage.color = Color.Lerp (new Color (1, 1, 1, 0), Color.black, transition);

		if (transition > 1 || transition < 0)
			isInTransition = false;
	}


}