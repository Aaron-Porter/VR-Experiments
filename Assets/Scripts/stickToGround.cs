using Unityusing UnityEngine;
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

	void Update(){

		var rayPosition = head.transform.position;
		var playspaceCenter = playspace.transform.position;

		RaycastHit[] hits;
        hits = Physics.RaycastAll(rayPosition, -playspace.transform.up, 100.0F);

        for (int i = 0; i < hits.Length; i++) {
            RaycastHit hit = hits[i];

			if(hit.collider.gameObject.name == "walkingPath"){
				// Debug.Log ("walkingPath" + hit.point);
				Debug.DrawLine (rayPosition, hit.point, Color.cyan);
				Debug.DrawLine (playspace.transform.position, hit.point, Color.red);
				Debug.DrawLine (playspace.transform.position, playspace.transform.up, Color.green);
				Debug.DrawLine (transform.position, playspace.transform.right, Color.yellow);

				Vector3 walkingHit = hit.point - transform.position;
				var angle = Vector3.Angle(transform.up,walkingHit);

				

				if(hit.transform.position.z > transform.position.z){
					angle = angle - 90;
					rotatePlayspace(angle);
				}else{
					angle = -(angle - 90);
					rotatePlayspace(angle);
				}
				
				Debug.Log(angle);

			}

		}

		// if(hit.collider.gameObject.name == "walkingPath"){
		// 	Debug.DrawLine (transform.position, hit.point, Color.cyan);
			
		// 	var normal = hit.normal;
		// 	// 	normal.y = -normal.y;
		// 	// 	normal.x = -normal.x;
		// 	// 	normal.z = -normal.z;
		// 	// Debug.Log(normal);
		// 	// rotatePlayspace(normal);
		// }
	}

	public void teleportOrientPlayspace(Vector3 normal){
		playspace.transform.up = normal;
	}

	public void rotatePlayspace(float rotation){
		 playspace.transform.Rotate(transform.right, rotation);
	}
	
	float SignedAngleBetween(Vector3 a, Vector3 b, Vector3 n){
	    // angle in [0,180]
	    float angle = Vector3.Angle(a,b);
	    float sign = Mathf.Sign(Vector3.Dot(n,Vector3.Cross(a,b)));

	    // angle in [-179,180]
	    float signed_angle = angle * sign;

	    // angle in [0,360] (not used but included here for completeness)
	    //float angle360 =  (signed_angle + 180) % 360;

	    return signed_angle;
	}
}

