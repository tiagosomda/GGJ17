using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WaveControl : MonoBehaviour {

	[Range(0, 10)]
	public float range= 4f;
	public bool waving = false;
	public bool beckoning = false;
	public bool isPlayer = false;


	private int waveButton, beckonButton;
	public bool waiting= false;
	private GameObject target= null;
	private bool forceMovement= false;
	private float speed;
	private PlayerMove moveScript;

	private SpriteRenderer renderer;

	private Vector3 dir;
	private RaycastHit hit;
	private GameObject[] characters;
	private NavMeshAgent nav;



	void Awake() {

		renderer = transform.FindChild ("WaveAnim").GetComponent<SpriteRenderer>();
		renderer.enabled = false;

		if (isPlayer) {
			moveScript = GetComponent<PlayerMove> ();
			speed = moveScript.moveSpeed;
			dir = moveScript.movement;
		} else {
			characters = GameObject.FindGameObjectsWithTag ("worker");
			nav = GetComponent<NavMeshAgent> ();
			dir = nav.velocity;
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

			waving = false;
			beckoning = false;

			if (moveScript.movement != Vector3.zero) {
				dir = moveScript.movement;
			}


			if (Input.GetMouseButton (waveButton)) {
				Wave ();
				renderer.enabled = true;

			} else if (Input.GetMouseButton (beckonButton)) {
				Beckon ();
			} else {
				renderer.enabled = false;
			}


		} else {

			//if worker is moving save the direction for when it is not moving
			if (nav.velocity != Vector3.zero) {
				dir = nav.velocity;
			}

			//optimized Raycast which only fires if a worker is within LOS range
			bool workerIsNear = false;

			foreach (GameObject worker in characters) {

				if (Vector3.Distance (worker.transform.position, transform.position) < range) {

					workerIsNear = true;
				}
			}

			if (workerIsNear) {
				checkLOS ();
			}
		}
	}

	void FixedUpdate() {

		/*if (target != null) {

			if (forceMovement) {
				
				Vector3.MoveTowards (transform.position, target.transform.position, speed * Time.fixedDeltaTime);
			}

			if (Vector3.Distance (transform.position, target.transform.position) <= 1) {

				if (isPlayer) {
					moveScript.hasControl = true;
				}
				forceMovement = false;
			}

		}*/

	}


	void Wave() {

		Debug.Log (gameObject.name + " waves");

		if (waiting && target != null) {

			waving = true;
			beckoning = false;

			target.GetComponent<WaveControl> ().Response ("wave", gameObject);

		}

		if (!waving && !beckoning) {

			waving = true;

			Debug.DrawLine (transform.position, transform.position + (dir * range));

			if( Physics.Raycast(transform.position, dir, out hit, range) ) {

				Debug.Log ("hit " + hit.collider.tag);
				Debug.DrawLine (transform.position, hit.point, Color.red);

				if (hit.collider.tag == "worker") {

					target = hit.collider.gameObject.transform.parent.gameObject;
						
					target.GetComponent<WaveControl> ().Initiate("wave", gameObject);

					target.GetComponent<NpcBehavior> ().setHeading (transform);

				}
			}
		}
	}

	void Beckon() {

		Debug.Log(gameObject.name + " beckons");


		if (waiting && target != null) {

			beckoning = true;
			waving = false;
			target.GetComponent<WaveControl> ().Response ("beckoning", gameObject);
		}

		if (!waving && !beckoning) {

			beckoning = true;

			Debug.DrawLine (transform.position, transform.position + dir * range);

			if( Physics.Raycast(transform.position, dir, out hit, range) ) {

				Debug.Log ("hit " + hit.collider.tag);
				Debug.DrawLine (transform.position, hit.point, Color.red);

				if (hit.collider.tag == "worker") {

					target = hit.collider.gameObject.transform.parent.gameObject;

					target.GetComponent<WaveControl> ().Initiate("beckon", gameObject);

				}
			}
		}
	}

	public void Initiate(string waveType, GameObject initiator) {

		Debug.Log ("Wave initiated by " + initiator.name);

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
				//waiting = true;
			}

		} else if (waveType == "beckon") {

			if (waving) {
				target.GetComponent<WaveControl> ().Response ("wave", gameObject);
				Lose ();
			} else if (beckoning) {
				target.GetComponent<WaveControl> ().Response ("beckoning", gameObject);
				Tie ();
			} else {
				//waiting = true;
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

	void checkLOS() {

		Debug.DrawLine (transform.position, transform.position + (dir * range));

		if (Physics.Raycast (transform.position, dir, out hit, range)) {

			Debug.Log ("hit " + hit.collider.tag);
			Debug.DrawLine (transform.position, hit.point, Color.red);

			if (hit.collider.tag == "worker") {

				int rand = Random.Range (0, 1);
				string waveType;

				renderer.enabled = true;

				if (rand == 0) {
					waveType = "wave";
				} else {
					waveType = "beckon";
				}

				target = hit.collider.gameObject.transform.parent.gameObject;

				target.GetComponent<WaveControl> ().Initiate (waveType, gameObject);

			} else {
				renderer.enabled = false;
			}
		}
	}
}
