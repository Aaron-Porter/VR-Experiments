using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Centrifuge : MonoBehaviour {

	public Vector3 eulerAngleVelocity;
    public List<GameObject> objects;
    public float gravitationalPull;
    bool onGround;
    Mesh mesh;
    Vector3[] vertices;
    Vector2[] uvs;
    Bounds bounds;
 

    void Start() {
        mesh = GetComponent<MeshFilter>().mesh;
        vertices = mesh.vertices;
        uvs = new Vector2[vertices.Length];
        bounds = mesh.bounds;
    }

    void FixedUpdate() {

        // Quaternion deltaRotation = Quaternion.Euler(eulerAngleVelocity * Time.deltaTime); // degrees a second - RPS
        // this.transform.Rotate (deltaRotation);

        Gravity();
    }

    void Gravity(){

        foreach (GameObject o in UnityEngine.Object.FindObjectsOfType<GameObject>()) {
            if(o.GetComponent<Rigidbody>()){

                var objVector = (this.transform.position - o.transform.position);
                var objDist = objVector.magnitude;
                var gravDist = (bounds.extents.x * transform.localScale.x);
                var powerOfGravity = gravitationalPull * (objDist / gravDist);

                var force = objVector.normalized * powerOfGravity;
                    force.x = 0;

                // Debug.Log("objDist " + objDist);
                // Debug.Log("gravDist " + gravDist);
                // Debug.Log(powerOfGravity);
                // Debug.Log(force);

                o.GetComponent<Rigidbody>().AddForce(force, ForceMode.Acceleration);

            }
        }
        
    }

    // void OnCollisionEnter (Collision col){
    //     col.gameObject.GetComponent<Rigidbody>().velocity = 0;
    // }
}