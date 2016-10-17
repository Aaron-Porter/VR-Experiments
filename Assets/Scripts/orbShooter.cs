using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using VRTK;

public class orbShooter : MonoBehaviour {

	// Expose variables
	public GameObject objectToSpawn; // orb
	public int objectCountLimit; // orb limit
	GameObject ShooterController;
	GameObject Spawner;
	GameObject objectToPosition;

	// setup controller
	private GameObject controller;
	private VRTK_ControllerActions controllerActions;

	// utility variables
	GameObject objectClone; // cloned object
	List<GameObject> objects = new List<GameObject>(); // list for objects

	public void Awake(){
		ShooterController = GameObject.Find("ShooterController");
		Spawner = GameObject.Find("ShooterController/Spawner");
		controller = GameObject.Find("Controller (right)");
	}


	public void Load(){

		// spawn and setup
		Vector3 spawnerPosition = this.transform.position; // spawner for orb
		objectClone = Instantiate(objectToSpawn, spawnerPosition, Quaternion.identity) as GameObject; // spawn
        objects.Add(objectClone); // add to object list

        // position
        objectToPosition = objects[objects.Count - 1]; // get last

		// haptics
		controllerActions = controller.GetComponent<VRTK_ControllerActions>();

        // remove if over count
        if(objects.Count > objectCountLimit){
        	GameObject gameObjectToRemove = objects[0];
        	objects.Remove(gameObjectToRemove );
        	Destroy(gameObjectToRemove);
        }

	}

	public void Update (){
		if( objectToPosition != null){
			controllerActions.TriggerHapticPulse((ushort)1000);
			Vector3 position = Spawner.transform.position;
			// position.y = position.y + 0.15f;
			objectClone.transform.position = position;
		}
	}
	
	public void Shoot(){
		objectClone.GetComponent<Rigidbody>().AddForce(transform.forward * -1000);
		controllerActions.TriggerHapticPulse((ushort)0);
		objectToPosition = null;
	}
}
