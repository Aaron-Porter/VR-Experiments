using UnityEngine;
using System.Collections;

public class helmetPositonRotation : MonoBehaviour {

	Vector3 rotationVector;
	Vector3 headPosition;
	Quaternion headRotation;
	GameObject head;


	void Awake(){
		head = GameObject.Find("Camera (eye)");
	}

	void Update(){


		// rotation
		headRotation = head.transform.rotation;
		transform.rotation = headRotation;
		rotationVector = transform.rotation.eulerAngles;
	}

	// void LateUpdate(){
	// 	// rotationVector.x = head.transform.rotation.x;
	// 	// transform.rotation = Quaternion.Euler(rotationVector);
	// }

 //    void FixedUpdate() {
 //        Vector3 turn = head.transform.up;
 //        Vector3.RotateTowards(Vector3.up, turn)(turn);
 //    }

}
