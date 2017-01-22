using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum Rank{
	Coworker,
	Manager
}
public enum aiState{
	Patrol,
	Working,
	Chasing, 
	OnBreak
}
public class EnemyAI : MonoBehaviour {
	public aiState state;
	//Lists
	public List<GameObject> likedPeople=new List<GameObject>(); // People I will chat up a storm with in there presenc
	public List<GameObject> hatedPeople =new List<GameObject>(); // People I will punch if in presence

	//Refs
	FieldOfView fieldView;
	Rank rank;

	Vector3 spawnLocation; //Random

	[HideInInspector]
	public Vector3 lastKnownPosit;//last known position you saw player

	//Vars

	[HideInInspector]
	public Transform player;

	[Range(0,10)]
	public float speed;

	//To Control draw
	[HideInInspector]
	public float disttoPlayer;

	[Range(0,10)]
	public float closestDistToDraw; //Closest distance you have to be to player for line of sight to pop up

	public LayerMask terrainMask; //For way points
	//For Patrol
	Vector3[] waypoints;

	public bool doPatrol=false;
	//Raycast
	RaycastHit hit;
	Ray ray;


	void Start () {
		fieldView = GetComponent<FieldOfView> ();

		//Find player to kep track of distance
		player = GameObject.FindWithTag ("Player").transform;
	}
	

	void Update () {
		disttoPlayer = Vector3.Distance (transform.position, player.position);

		//If within distance draw line of sight
		if(disttoPlayer<closestDistToDraw){
			fieldView.DrawFOW ();
			}

		if(state==aiState.Patrol){
			//Create way points and patrol
			Patrol (RandomSpherePoints(transform.position,Random.Range(5,8.5f),terrainMask));
			}
		}
		
	public void SetDestination(Vector3 posit, float speed){
		if(fieldView.target!=null){
		if (transform.position != posit) {
			//Move to posit
		} else {
			//do som
			switch(rank){
				case Rank.Coworker:
				Coworker ();
				break;

				case Rank.Manager:
					Boss ();
				break;
				}
			}
		}
		else{
			//if target dissappears then patrol
			state = aiState.Patrol;
		}
	}
	void Coworker(){
		//Moves towards,
		//when Close enough interact
	}

	void Boss(){
		//Moves towards,
		//when Close enough interact
	}
		
	void Patrol(Vector3[] wayP){
		float runDown = 45;
		runDown -= Time.deltaTime;
		if(runDown>0){
		waypoints = wayP;
		int curWayPoint=0;
		//Set last known position as starting position

			if (curWayPoint < waypoints.Length) {
				transform.position = Vector2.MoveTowards (transform.position, waypoints [curWayPoint], speed);
				if (Vector2.Distance (transform.position, waypoints [curWayPoint]) < 5) {
					curWayPoint++;
				}
			} else if (curWayPoint == (waypoints.Length - 1)) {
				ResetPath ();
				//Make new path
				curWayPoint = 0;
				}
			}
		else{
			//once time's run down quit patrol
			state=aiState.Working;
			}
		}
	
	void ResetPath(){
		//Makes a new path
		Patrol(RandomSpherePoints(transform.position,Random.Range(5,8.5f),terrainMask));
		return;
	}

	public Vector3[] RandomSpherePoints(Vector3 origin, float distance, int layerMask){
		Vector3[] point= new Vector3[3];
		Vector3 randomDirection = Random.insideUnitSphere * distance;
	
		for(int r=0;r<2;r++){
			point[r]=randomDirection += origin;
	
			//NavMeshHit navHit;
			//NavMesh.SamplePosition (randomDirection, out navHit, distance, layerMask);
			//point [r] = navHit.point;
		}
		//Middle posit is last known posit
		point[1]=lastKnownPosit;
		return point;
	}
	void OnDrawGizmos(){
		Gizmos.color = Color.white;
		if(waypoints.Length>0){
		//Visual Way point debug
		foreach(Vector2 wayWay in waypoints ){
				Gizmos.DrawSphere (wayWay,1);
			}
		}
	}

}
