using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinTrigger : MonoBehaviour {

	public Transform player;

	public FadeManager fade;
	bool slowDownTime;
	public float speed;

	public GameObject WinScreen;
	public GameObject GameOverScreen;

	public bool gameOverState = false;
	// Use this for initialization
	void Start () {
		slowDownTime = false;
		GameController.gameOver = false;
		gameOverState = false;
	}
		
	// Update is called once per frame
	void Update () {
		if (slowDownTime) {
			Time.timeScale -= Time.deltaTime*speed;
		}

		if (GameController.gameOver && gameOverState == false) {
			gameOverState = true;
			slowDownTime = true;
			fade.Fade (true, 1f);
			GameOverScreen.SetActive (true);
			Debug.Log ("GAME OVER");

		}

		if (IsNear (transform.position, player.position)) {
			
			if (slowDownTime == false) {
				slowDownTime = true;
				Debug.Log ("WIN");
				fade.Fade (true, 1f);
				WinScreen.SetActive (true);
			}
		}
	}

	private bool IsNear(Vector3 a, Vector3 b) {
		return Vector3.Distance (a, b) < 1f;
	}
		
	public void MainMenu()
	{
		Time.timeScale = 1f;
		SceneManager.LoadScene (0);
		GameController.gameOver = false;
		gameOverState = false;
		Debug.Log ("RESET GAME!");
	}
		
	public void RestartGame() {
		Time.timeScale = 1f;
		SceneManager.LoadScene (1);
		GameController.gameOver = false;
		gameOverState = false;
		Debug.Log ("MAIN MENU!");
	}
}