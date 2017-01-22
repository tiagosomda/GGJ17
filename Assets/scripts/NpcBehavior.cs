using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NpcBehavior : MonoBehaviour
{
	public float speed;
	public NavMeshAgent nav;

	public Transform anchor;
	public float minAchorStay;
	public float maxAchorStay;

	public List<Transform> poi;
	public float minPoiStay;
	public float maxPoiStay;
    public EnemyAI eAI;
    public bool goAfter = false;

    public GameObject obj;
	// Use this for initialization
	void Start ()
	{
        eAI.GetComponent<EnemyAI>();
       // anchor = poi[Random.RandomRange(0, 2)];

		nav = GetComponent<NavMeshAgent> ();
        nav.speed = 1.75f;

		heading = anchor;
		nav.destination = heading.position;

		timeout = Random.Range (minAchorStay, maxAchorStay);
	}
	
	// Update is called once per frame
	void Update ()
	{
 
        if (!goAfter)
        {
            DetermineHeading();
        }
	}

	// ---

	private Transform heading;
	private float timeout;

	private void DetermineHeading ()
	{
		var isAtHeading = IsNear(transform.position, heading.position);

		if (isAtHeading) {
            nav.destination = transform.position;
			// Determine heading
			if (timeout <= 0f) {
				// If at anchor, head to POI
				if (IsNear(transform.position, anchor.position)) {
					heading = poi [Random.Range (0, poi.Count)];
					timeout = Random.Range (minPoiStay, maxPoiStay);
     
					nav.destination = heading.position;
					
					return;
				}

				// If at POI, head to anchor
				if (poi.Find (item => IsNear(item.position, transform.position)) != null) {
					heading = anchor;
					timeout = Random.Range (minAchorStay, maxAchorStay);
					nav.destination = heading.position;
					return;
				}
			}

			timeout -= Time.deltaTime;
		}
	}

	private bool IsNear(Vector3 a, Vector3 b) {
		return Vector3.Distance (a, b) < 2.5f;
	}
}
