using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Centrifuge : MonoBehaviour {

	public Vector3 eulerAngleVelocity;
    Rigidbody rb;
    public List<GameObject> objects;
    public float gravitationalPull;
    bool onGround;
 

    void Start() {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate() {

        // Quaternion deltaRotation = Quaternion.Euler(eulerAngleVelocity * Time.deltaTime); // degrees a second - RPS
        // this.transform.Rotate (deltaRotation);

        Gravity();
    }

    void Gravity(){

        foreach (GameObject o in UnityEngine.Object.FindObjectsOfType<GameObject>()) {
            if(o.GetComponent<Rigidbody>()){
                Debug.DrawLine (this.transform.position, this.transform.position - o.transform.position, Color.cyan);

                var force = (this.transform.position - o.transform.position).normalized * gravitationalPull * o.GetComponent<Rigidbody>().mass;
                    force.x = 0;
                    // force.z = 0;
                // Debug.Log(force);
                o.GetComponent<Rigidbody>().AddForce(force, ForceMode.Acceleration);

            }
        }
        
    }

    // void OnCollisionEnter (Collision col){
    //     col.gameObject.GetComponent<Rigidbody>().velocity = 0;
    // }
}