using UnityEngine;
using System.Collections;

public class audioOnCollision : MonoBehaviour {

	void OnCollisionEnter(Collision collisionInfo){
		if(collisionInfo.relativeVelocity.magnitude >= 0.5){
			GetComponent<AudioSource>().volume = collisionInfo.relativeVelocity.magnitude / 30;
			GetComponent<AudioSource>().Play();
		}
	}
}
