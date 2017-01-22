using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveControl : MonoBehaviour {

	[Range(0, 10)]
	public float range= 4f;
	public bool waving = false;
	public bool beckoning = false;
	public bool isPlayer = false;


	private int waveButton, beckonButton;
	private bool waiting= false;
	private GameObject target= null;
	private bool forceMovement= false;
	private float speed;
	private PlayerMove moveScript;


	void Awake() {

		if (isPlayer) {
			moveScript = GetComponent<PlayerMove> ();
			speed = moveScript.moveSpeed;
		}

		waveButton = 0;

		beckonButton = 1;
			
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		if (isPlayer) {

			if (Input.GetMouseButton (waveButton)) {
				Wave ();
			} else if (Input.GetMouseButton (beckonButton)) {
				Beckon ();
			}




		} else {

			RaycastHit hit;

			if (Physics.Raycast (transform.position, transform.forward, out hit, range)) {

				if (hit.collider.tag == "player") {

					int rand = Random.Range (0, 1);
					string waveType;

					if (rand == 0) {
						waveType = "wave";
					} else {
						waveType = "beckon";
					}

					hit.collider.gameObject.GetComponent<WaveControl> ().Initiate (waveType, gameObject);

				}
			}
		}
	}

	void FixedUpdate() {

		if (target != null) {

			if (forceMovement) {
				
				Vector3.MoveTowards (transform.position, target.transform.position, speed * Time.fixedDeltaTime);
			}

			if (Vector3.Distance (transform.position, target.transform.position) <= 1) {

				moveScript.hasControl = true;
				forceMovement = false;
			}

		}

	}


	void Wave() {

		if (waiting) {

			waving = true;
			beckoning = false;
			target.GetComponent<WaveControl> ().Response ("wave", gameObject);

		}

		if (!waving && !beckoning) {

			RaycastHit hit;

			waving = true;

			if( Physics.Raycast(transform.position, transform.forward, out hit, range) ) {

				if (hit.collider.tag == "worker") {

					target = hit.collider.gameObject;
						
					target.GetComponent<WaveControl> ().Initiate("wave", gameObject);
				}
			}
		}
	}

	void Beckon() {

		if (waiting) {

			beckoning = true;
			waving = false;
			target.GetComponent<WaveControl> ().Response ("beckoning", gameObject);
		}

		if (!waving && !beckoning) {

			RaycastHit hit;

			beckoning = true;

			if( Physics.Raycast(transform.position, transform.forward, out hit, range) ) {

				if (hit.collider.tag == "worker") {

					target = hit.collider.gameObject;

					target.GetComponent<WaveControl> ().Initiate("beckon", gameObject);

				}
			}
		}
	}

	public void Initiate(string waveType, GameObject initiator) {

		target = initiator;

		if (isPlayer) {
			moveScript.hasControl = false;
		}

		if (waveType == "wave") {

			if (waving) {
				target.GetComponent<WaveControl> ().Response ("wave", gameObject);
				Pass ();
			} else if (beckoning) {
				target.GetComponent<WaveControl> ().Response ("beckoning", gameObject);

				Win ();
			} else {
				waiting = true;
			}

		} else if (waveType == "beckon") {

			if (waving) {
				target.GetComponent<WaveControl> ().Response ("wave", gameObject);
				Lose ();
			} else if (beckoning) {
				target.GetComponent<WaveControl> ().Response ("beckoning", gameObject);
				Tie ();
			} else {
				waiting = true;
			}
		}
	}

	void Response(string waveType, GameObject responder) {

		target = responder;

		if (waveType == "wave") {

			if (waving) {
				Pass ();
			} else if (beckoning) {
				Win ();
			}

		} else if (waveType == "beckon") {

			if (waving) {
				Lose ();
			} else if (beckoning) {
				Tie ();
			}
		}
	}


	void Pass() {

		if (isPlayer) {
			moveScript.hasControl = true;
		}

		target = null;
		waving = false;
		beckoning = false;
	}

	void Win() {

		if (isPlayer) {
			moveScript.hasControl = false;
		}

		waving = false;
		beckoning = false;
	}

	void Lose() {
		forceMovement = true;

		waving = false;
		beckoning = false;
	}

	void Tie() {
		forceMovement = true;

		waving = false;
		beckoning = false;
	}
}
