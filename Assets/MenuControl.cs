using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuControl : MonoBehaviour {

	private GameObject nowBinding = null;

	private string[] mouseButtons= new string[]{"Left", "Right", "Middle"};

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnGUI() {

		if (nowBinding != null) {

			Bind (Event.current);

		}

	}

	public void Hide () {
		gameObject.SetActive (false);
	}

	public void Unhide() {
		gameObject.SetActive (true);
	}

	public void StartMenu(GameObject startMenu) {
		startMenu.transform.SetAsLastSibling ();
	}

	public void ControlMenu (GameObject controlMenu) {
		controlMenu.transform.SetAsLastSibling ();
	}
		

	public void StartGame() {
		SceneManager.LoadScene ("main");
	}

	public void NewBinding (GameObject obj) {

		//the game is now looking for input from the player
		nowBinding = obj;
		nowBinding.transform.GetChild(0).GetComponent<Text>().color= Color.red;

	}

	public void Bind (Event evt) {

		if (evt.type == EventType.KeyDown || evt.type == EventType.MouseDown) {

			string key, value;


			//key holds the name of the input, value holds the binding that the player has selected
			key = nowBinding.GetComponent<Text> ().text;
			value = null;

			if (evt.isKey) {

				value = evt.keyCode.ToString ();

				//save the key, value pair for the input code later (during game)
				PlayerPrefs.SetString (key, value);

			} else if (evt.isMouse) {

				value = evt.button.ToString();

				//save the key, value pair for the input code later (during game)
				PlayerPrefs.SetInt (key, evt.button);

			}
		 


			//change mouse input value to be human readable
			if (evt.isMouse) {
				value = mouseButtons [evt.button] + " Click";
			}


			//display text for player
			nowBinding.transform.GetChild (0).GetComponent<Text> ().text = value;
			nowBinding.transform.GetChild (0).GetComponent<Text> ().color = Color.black;

			//game is no longer looking for player input
			nowBinding = null;
		}
		
	}


}
