using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SeekerBehavior : MonoBehaviour {

	public Transform target;
	public float proximity;
	private NavMeshAgent nav;

	// Use this for initialization
	void Start () {
		nav = GetComponent<NavMeshAgent> ();

		nav.speed = 3f;
		nav.SetDestination (target.position);

		proximity = 2f;
	}

	// Update is called once per frame
	void Update () {
		var d = Vector3.Distance (transform.position, target.position);
		//Debug.Log (d);

		if (d < proximity)
			nav.Stop ();
	}
}
