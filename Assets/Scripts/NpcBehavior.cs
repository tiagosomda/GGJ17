using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NpcBehavior : MonoBehaviour
{
	public float speed;
	private NavMeshAgent nav;

	public Transform anchor;
	public float minAchorStay;
	public float maxAchorStay;

	public List<Transform> poi;
	public float minPoiStay;
	public float maxPoiStay;

	// Use this for initialization
	void Start ()
	{
		nav = GetComponent<NavMeshAgent> ();
		nav.speed = speed;

		heading = anchor;
		nav.destination = heading.position;

		timeout = Random.Range (minAchorStay, maxAchorStay);
	}
	
	// Update is called once per frame
	void Update ()
	{
		DetermineHeading ();
	}

	// ---

	private Transform heading;
	private float timeout;

	private void DetermineHeading ()
	{
		var isAtHeading = IsNear(transform.position, heading.position);

		if (isAtHeading) {
			nav.Stop ();
			// Determine heading
			if (timeout <= 0f) {
				// If at anchor, head to POI
				if (IsNear(transform.position, anchor.position)) {
					heading = poi [Random.Range (0, poi.Count)];
					timeout = Random.Range (minPoiStay, maxPoiStay);

					nav.destination = heading.position;
					nav.Resume ();
					return;
				}

				// If at POI, head to anchor
				//if (poi.Find (item => IsNear(item.position, transform.position)) != null) {
					
					heading = anchor;
					timeout = Random.Range (minAchorStay, maxAchorStay);

					nav.destination = heading.position;
					nav.Resume ();
					return;
				//}
					
			}

			timeout -= Time.deltaTime;
		}
	}

	public void setHeading(Transform _heading) {

		heading = _heading;
		nav.SetDestination(heading.position);
	}

	private bool IsNear(Vector3 a, Vector3 b) {
		return Vector3.Distance (a, b) < 1;
	}
}
