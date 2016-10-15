using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class spawnObjects : MonoBehaviour {

	// Expose variables
	public bool onTimer = false;
	public float Timer;
	float TimerVal;

	public GameObject objectToSpawn;
	public int objectCountLimit;
	GameObject objectClone;
	List<GameObject> objects = new List<GameObject>();

	// Use this for initialization
	void Start () {
		TimerVal = Timer;
	}

	public void Spawn(){
		Vector3 spawnerPosition = this.transform.position;

		objectClone = Instantiate(objectToSpawn, spawnerPosition, Quaternion.identity) as GameObject;
        objectClone.GetComponent<Rigidbody>().AddForce(transform.forward * 1000);
        objects.Add(objectClone);

        if(objects.Count > objectCountLimit){
        	GameObject gameObjectToRemove = objects[0];
        	objects.Remove(gameObjectToRemove );
        	Destroy(gameObjectToRemove);
        }

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
