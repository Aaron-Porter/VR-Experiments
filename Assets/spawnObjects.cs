using UnityEngine;
using System.Collections;

public class spawnObjects : MonoBehaviour {

	// Expose variables
	public bool onTimer = false;
	public float Timer;
	public GameObject objectToSpawn;
	public GameObject spawner;
	float TimerVal;

	// create empty variables
	GameObject objectClone;

	// Use this for initialization
	void Start () {
		TimerVal = Timer;
	}

	void Spawn(){
		Vector3 spawnerPosition = spawner.transform.position;
		objectClone = Instantiate(objectToSpawn, spawnerPosition, Quaternion.identity) as GameObject;
        objectClone.GetComponent<Rigidbody>().AddForce(spawnerPosition * 50);
		Timer = TimerVal;
	}
	
	// Update is called once per frame
	void Update () {
		
		// spawn object after specified time
		if(onTimer == true){
			Timer -= Time.deltaTime;
			if (Timer <= 0){
				Spawn();
			}
		}

	}
}
