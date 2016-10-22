using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class stickToGround : MonoBehaviour {

	GameObject head;
	GameObject playspace;
	List<Vector3> rayNormals = new List<Vector3>();
	float headHeight;
	RaycastHit hit;
	Vector3 targetLocation;



	void Awake(){
		head = GameObject.Find("Camera (eye)");
		playspace = GameObject.Find("[CameraRig]");
	}

	void Update(){
		headHeight = head.transform.localPosition.y;

		// cast rays
		for(int i = 0; i < 3; i++){

			var seperation = (float)0.5;
			var offset = i * seperation;
			var vectorPosition = head.transform.position;
				vectorPosition.z = (vectorPosition.z - seperation) + offset;
			var rayPosition = vectorPosition;

			if (Physics.Raycast(rayPosition, Vector3.down, out hit)) {
				Debug.DrawLine (transform.position, hit.point, Color.cyan);
				rayNormals.Add(hit.normal);
			};

		}

		var averagedNormals = (rayNormals[0] + rayNormals[1] + rayNormals[2]) / 3;

		rotatePlayspace(averagedNormals);
	}

	void rotatePlayspace(Vector3 normal){

	    // Quaternion targetRotation = Quaternion.FromToRotation(playspace.transform.up, normal);
     //    Quaternion finalRotation = Quaternion.RotateTowards(playspace.transform.rotation, targetRotation, Time.deltaTime);

     //    playspace.transform.rotation = finalRotation;

	    var targetRotation = Quaternion.FromToRotation(playspace.transform.up, hit.normal) * playspace.transform.rotation;
	    playspace.transform.rotation = Quaternion.Slerp(playspace.transform.rotation, targetRotation, Time.deltaTime * 1000);
	}


}
