using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {
    public Transform[] chairSpawns = new Transform[8];
    public GameObject[] animals = new GameObject[10];
    int spawnLocation;
    int spawnedanimal;
    float spawntimer;
	void Start () {
        spawntimer = 45;
	}
	
	
	void Update () {
        if (spawntimer > 0)
        {
            spawntimer -= Time.deltaTime;
        }
        else
        {
            Spawn();
        }
	}
    void Spawn()
    {
        spawnLocation = Random.Range(0, 7);
        spawnedanimal = Random.Range(0, 1);

        GameObject obj = Instantiate(animals[spawnedanimal]) as GameObject;
        obj.transform.position = chairSpawns[spawnLocation].position;
        obj.GetComponent<EnemyAI>().spawnPoint= chairSpawns[spawnLocation].position;
        obj.GetComponent<NpcBehavior>().anchor = obj.GetComponent<NpcBehavior>().poi[Random.RandomRange(0, 2)];
        spawntimer = 45;
    }
}
