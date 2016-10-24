using UnityEngine;
using System.Collections;

public class teleport : MonoBehaviour {

	void Update () {
        Vector3 forward = transform.TransformDirection(Vector3.forward) * 10;
        RaycastHit hit;

        if (Physics.Raycast(transform.position, forward, out hit)){
        	Debug.DrawRay(transform.position, forward, Color.green);
        }
    }

}