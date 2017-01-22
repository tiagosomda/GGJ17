using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinTrigger : MonoBehaviour {
	private FadeManager fade;
	bool slowDownTime;
	public float speed;
	public GameObject UIScreen;
	// Use this for initialization
	void Start () {
		slowDownTime = false;
		fade = GetComponent<FadeManager> ();
	}
		

	void OnTriggerEnter2D(Collider2D other) {
		if (slowDownTime == false) {
			if (other.tag == "Player") {
				slowDownTime = true;
				fade.Fade (true, 1f);
				UIScreen.SetActive (true);
			}
		}
	}		
	// Update is called once per frame
	void Update () {
		if (slowDownTime) {
			Time.timeScale -= Time.deltaTime*speed;
		}
		
	}

	public void ResetGame()
	{
		Time.timeScale = 1f;
		SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
		Debug.Log ("RESET GAME!");
	}
}
