using UnityEngine;
using System.Collections;

public class gravityControl : MonoBehaviour {

	public Rigidbody rb;
	public bool zeroGravityBool = false;

	public void toggleZeroGravity(){
		// zeroGravityBool != zeroGravityBool;
	}

	void zeroGravity(){
		if(zeroGravityBool == true){
			rb.AddForce(-Physics.gravity);
		}
	}

	// Events
	void Start(){
        rb = GetComponent<Rigidbody>();
    }

	void FixedUpdate(){
		zeroGravity();
	}

}
