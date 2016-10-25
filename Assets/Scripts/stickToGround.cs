using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class stickToGround : MonoBehaviour {

	GameObject head;
	GameObject playspace;
	float headHeight;
	RaycastHit hit;
	Vector3 targetLocation;



	void Awake(){
		head = GameObject.Find("Camera (eye)");
		playspace = GameObject.Find("[CameraRig]");
	}

	void Update(){

		var rayPosition = head.transform.position;

		if (Physics.Raycast(rayPosition, -playspace.transform.up, out hit)) {
			if(hit.collider.gameObject.name == "walkingPath"){
				Debug.DrawLine (transform.position, hit.point, Color.cyan);
				
				var normal = hit.normal;
				// 	normal.y = -normal.y;
				// 	normal.x = -normal.x;
				// 	normal.z = -normal.z;
				// Debug.Log(normal);
				rotatePlayspace(normal);
			}
		};
	}

	public void rotatePlayspace(Vector3 normal){

		playspace.transform.up = normal;
	}


}
