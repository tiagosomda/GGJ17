using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharQuirks : MonoBehaviour {
	float maxBoredome;
	float boredDom;

	Transform personToseek;
	EnemyAI aI;
	public bool onBreak;//says if on break, coffee machines and etc can access this to start activity
	public bool chatting; //if not chatting with someone

	void Start () {
		aI = GetComponent<EnemyAI> ();
		maxBoredome= Random.Range (50, 200);
		boredDom = maxBoredome;



	}
	

	void Update () {
		boredDom -= Time.deltaTime;
		if(boredDom<=0){
			if(!chatting){
			onBreak = true;
			}
		}
		if(onBreak){
			MovetoActivity (SelectActivity());
		}
	}
	int SelectActivity (){
		int randomChoice= Random.Range (1, 5);
		return randomChoice;
		
	}
	void MovetoActivity(int i){
		switch(i){
		case 1:
			//Coffee
			break;
		case 2:
			//Eat food
			break;
		case 3:
			//water cooler
			break;
		case 4:
			//steal office supplies
			break;
		case 5:
			//chat with someone
			personToseek = aI.likedPeople [Random.Range (0, aI.likedPeople.Count)].transform;
			Chat ();
			break;
		}
	}
	void BreakTime(){
		if (boredDom < maxBoredome) {
			maxBoredome += Time.deltaTime * 1.5f;
			} else {
			BackToWork ();
		}
	}
	void Chat(){
		//move to person, and chat with the,
		if (Vector3.Distance (transform.position, personToseek.transform.position ) < 3) {
			BreakTime ();
			personToseek.GetComponent<CharQuirks> ().chatting = true;
		} else {
			//move to player
		}
	}
	void BackToWork(){
		if(personToseek!=null){
			personToseek.GetComponent<CharQuirks> ().chatting = false;
			personToseek = null;
		}
		//show clock thought bubble, head back to desk
	}
}
