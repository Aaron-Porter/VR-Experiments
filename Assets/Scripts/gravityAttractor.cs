using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using VRTK;

public class gravityAttractor : MonoBehaviour {

    public float pullRadius = 2;
    public float pullForce = 1;
    public bool enableAttractBool = false;
    
    // setup controller
	public GameObject controller;
	public VRTK_ControllerActions controllerActions;

    public void Awake(){
		controller = GameObject.Find("Controller (right)");
	}

	public void Load(){
		controllerActions = controller.GetComponent<VRTK_ControllerActions>();
	}

    public void enableAttract(){
    	enableAttractBool = true;
    }

    public void disableAttract(){
    	enableAttractBool = false;
    }

 
	public void Update() {
		if(enableAttractBool == true){
			// controllerActions.TriggerHapticPulse((ushort)1000);
			attract();
		}
	}

	public void attract(){
		foreach (Collider collider in Physics.OverlapSphere(transform.position, pullRadius)) {
			if(collider.GetComponent<Rigidbody>() != null){
			// calculate direction from target to me
			Vector3 forceDirection = transform.position - collider.transform.position;

			// apply force on target towards me
			collider.GetComponent<Rigidbody>().AddForce(forceDirection.normalized * pullForce * Time.fixedDeltaTime);
			}
		}
	}
}
