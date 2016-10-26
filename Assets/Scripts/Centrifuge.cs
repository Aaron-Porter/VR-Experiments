using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Centrifuge : MonoBehaviour {

	public Vector3 eulerAngleVelocity;
    Rigidbody rb;
    public List<GameObject> objects;
    public float gravitationalPull;
 

    void Start() {
        rb = GetComponent<Rigidbody>();
    }

    void Update() {

        Quaternion deltaRotation = Quaternion.Euler(eulerAngleVelocity * Time.deltaTime); // degrees a second - RPS
        this.transform.Rotate (360,0,0);

        //or apply gravity to all game objects with rigidbody
        foreach (GameObject o in UnityEngine.Object.FindObjectsOfType<GameObject>()) {
            if(o.GetComponent<Rigidbody>() && o != this){
                o.GetComponent<Rigidbody>().AddForce((this.transform.position - o.transform.position).normalized * gravitationalPull);
            }
        }
    }
}