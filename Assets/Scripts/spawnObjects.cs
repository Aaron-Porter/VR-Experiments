using UnityEngine;
using System.Collections;

public class spawnObjects : MonoBehaviour {

	// Expose variables
	public bool onTimer = false;
	public float Timer;
	float TimerVal;

	public GameObject objectToSpawn;

	// create empty variables
	GameObject objectClone;

	// Use this for initialization
	void Start () {
		TimerVal = Timer;
	}

	public void Spawn(){
		Debug.Log ("spawn");
		Vector3 spawnerPosition = this.transform.position;

		objectClone = Instantiate(objectToSpawn, spawnerPosition, Quaternion.identity) as GameObject;
        objectClone.GetComponent<Rigidbody>().AddForce(transform.forward * 1000);
	}
	
	// Update is called once per frame
	void Update () {
		
		// spawn object after specified time
		if(onTimer == true){
			Timer -= Time.deltaTime;
			if (Timer <= 0){
				Spawn();
				Timer = TimerVal;
			}
		}

	}
}
