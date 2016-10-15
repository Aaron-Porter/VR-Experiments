using UnityEngine;
using System.Collections;

public class gravityAttractor : MonoBehaviour {

    public float pullRadius = 2;
    public float pullForce = 1;
    public bool enableAttractBool = false;

    public void enableAttract(){
    	enableAttractBool = true;
    }

    public void disableAttract(){
    	enableAttractBool = false;
    }

 
	public void FixedUpdate() {
		if(enableAttractBool == true){
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
