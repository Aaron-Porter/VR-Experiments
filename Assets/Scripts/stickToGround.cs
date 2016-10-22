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
		headHeight = head.transform.localPosition.y;

		var rayPosition = head.transform.position;
			rayPosition.y = rayPosition.y - 100;

		if (Physics.Raycast(rayPosition, Vector3.up, out hit)) {
			if(hit.collider.gameObject.name == "walkingPath"){
				Debug.DrawLine (transform.position, hit.point, Color.cyan);
				
				var normal = hit.normal;
					normal.y = -normal.y;
					normal.x = -normal.x;
					normal.z = -normal.z;
				Debug.Log(normal);
				rotatePlayspace(normal);
			}
		};





	}

	void rotatePlayspace(Vector3 normal){

	    // Quaternion targetRotation = Quaternion.FromToRotation(playspace.transform.up, normal);
     //    Quaternion finalRotation = Quaternion.RotateTowards(playspace.transform.rotation, targetRotation, Time.deltaTime);

     //    playspace.transform.rotation = finalRotation;

	    var targetRotation = Quaternion.FromToRotation(playspace.transform.up, normal) * playspace.transform.rotation;
	    playspace.transform.rotation = targetRotation;
	}


}
