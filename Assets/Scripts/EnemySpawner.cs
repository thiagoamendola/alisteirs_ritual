using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour {

	public float SpawnDistance = 6;
	public GameObject enemy1;
	public Vector3[] spawnPoints;

	public bool active;
	float runningTime;
	float lastSpawn;
	Pentagram pentagram;

	void Start () {
		active = true;
		runningTime = 0;
		lastSpawn = 0;
		pentagram = GameObject.Find("Pentagrama").GetComponent<Pentagram>();
	}
	
	void Update () {
		if(active){
			runningTime += Time.deltaTime;

			//if(runningTime < 15f){
			if(pentagram.gatheredSouls < 3){
				//Debug.Log(0);
				if(runningTime - lastSpawn >= 3f){
					SpawnRandom();
					lastSpawn = runningTime;
				}
			//}else if(runningTime < 45f){
			}else if(pentagram.gatheredSouls < 7){
				//Debug.Log(1);
				if(runningTime - lastSpawn >= 2.4f){
					SpawnRandom();
					lastSpawn = runningTime;
				}
			}else {
				//Debug.Log(2);
				if(runningTime - lastSpawn >= 1.75f){
					SpawnRandom();
					lastSpawn = runningTime;
				}
			}
		}
	}

	void SpawnRandom (){
		int n = Random.Range(0,6);
		Instantiate(enemy1,spawnPoints[n], Quaternion.identity);
	}

	void SpawnDirect(Vector3 v){
		
	}
}
