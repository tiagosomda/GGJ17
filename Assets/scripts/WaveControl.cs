using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveControl : MonoBehaviour {

	string waveButton, beckonButton;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void Awake() {

		//initialize player key bindings
		if (PlayerPrefs.HasKey("Wave")) {
			waveButton = PlayerPrefs.GetString ("Wave");

		}
		else {
			waveButton = "0";
		}

		if (PlayerPrefs.HasKey("Beckon")) {
			beckonButton = PlayerPrefs.GetString ("Beckon");
		}
		else {
			beckonButton = "1";
		}

	}

	void CheckInput() {

		if ( Input.GetButton (waveButton) || Input.GetKey (waveButton) ) {
			Debug.Log ("Player Waves");
			//execute wave method
		}

		if ( Input.GetButton(waveButton) || Input.GetKey(waveButton) ) {
			Debug.Log ("Player Beckons");
			//execute beckon method
	
	}
}
