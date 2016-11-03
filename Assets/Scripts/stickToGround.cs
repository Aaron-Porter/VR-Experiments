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

	void FixedUpdate(){

		var rayPosition = head.transform.position;
			rayPosition = rayPosition + (-playspace.transform.up * 30);

		RaycastHit[] hits;
        hits = Physics.RaycastAll(rayPosition, playspace.transform.up, 100.0F);

        for (int i = 0; i < hits.Length; i++) {
            RaycastHit hit = hits[i];

			if(hit.collider.gameObject.name == "walkingPath"){

				Debug.DrawLine(rayPosition, hit.point, Color.cyan);
				
				var normal = hit.normal;

				// Debug.Log(normal);
				rotatePlayspace(normal);


			}

		}

	}

	public void rotatePlayspace(Vector3 normal){
		normal.y = -normal.y;
		normal.x = -normal.x;
		normal.z = -normal.z;

		playspace.transform.up = normal;
		// var targetRotation = Quaternion.FromToRotation(playspace.transform.up, normal) * playspace.transform.rotation;
	 //    playspace.transform.rotation = targetRotation;
	}


}