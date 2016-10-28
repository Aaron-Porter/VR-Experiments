using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class stickToGround : MonoBehaviour {

	GameObject head;
	GameObject playspace;
	GameObject playspaceFloor;
	float headHeight;
	RaycastHit hit;
	Vector3 targetLocation;



	void Awake(){
		head = GameObject.Find("Camera (eye)");
		playspace = GameObject.Find("[CameraRig]");
		playspaceFloor = GameObject.Find("playspaceFloor");
	}

	void FixedUpdate(){

		var rayPosition = head.transform.position;
		var playspaceCenter = playspace.transform.position;

		RaycastHit[] hits;
        hits = Physics.RaycastAll(rayPosition, -playspace.transform.up, 100.0F);

        for (int i = 0; i < hits.Length; i++) {
            RaycastHit hit = hits[i];

			if(hit.collider.gameObject.name == "walkingPath"){

				Vector3 walkingHit = hit.point - transform.position;

				Debug.DrawLine (rayPosition, hit.point, Color.cyan);
				Debug.DrawLine (transform.position, hit.point, Color.red);


				var angle = Vector3.Angle(transform.up, walkingHit) ;
			    Vector3 cross = Vector3.Cross(transform.up, walkingHit);
			    int sign = Vector3.Cross(transform.up, walkingHit).x < 0 ? -1 : 1;
			    angle = (angle * sign) - (90 * sign);

			    // Debug.Log("Sign: " + sign);
			    // Debug.Log("Angle: " + angle);

				rotatePlayspace(angle);


			}

		}

	}

	public void teleportOrientPlayspace(Vector3 normal){
		playspace.transform.up = normal;
	}

	public void rotatePlayspace(float rotation){
		 playspace.transform.Rotate(transform.right, rotation);
	}

}

