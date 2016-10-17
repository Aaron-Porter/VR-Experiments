using UnityEngine;
using System.Collections;

public class helmetPositonRotation : MonoBehaviour {

	Vector3 rotationVector;

	void Update(){
		rotationVector = transform.rotation.eulerAngles;
	}

	void LateUpdate(){
		rotationVector.x = 0;
		transform.rotation = Quaternion.Euler(rotationVector);
	}

}
