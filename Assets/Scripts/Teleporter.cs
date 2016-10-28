using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using VRTK;

public class Teleporter : MonoBehaviour {

	VRTK_ControllerEvents controllerEvents;
	LineRenderer lineRenderer;
	public Color color;
	public float width;
	public bool rayVisible;
	RaycastHit teleportPoint;
	GameObject playspace;

	void Start(){
		playspace = GameObject.Find("[CameraRig]");
		lineRenderer = gameObject.AddComponent<LineRenderer>() as LineRenderer;
		lineRenderer.enabled = false;
		lineRenderer.SetColors(color, color);
		lineRenderer.SetVertexCount(2);

	}

	void Update(){

		if(rayVisible == true){
			Ray();
		}

	}

	public void castRay(bool value){
		rayVisible = value;
		if(value == false){
			lineRenderer.SetWidth(0,0);
			lineRenderer.enabled = false;
		}else{
			lineRenderer.SetWidth(width,width);
			lineRenderer.enabled = true;
		}
	}

	public void Ray () {

		var rayOrigin = transform.position;
			rayOrigin = rayOrigin + (transform.TransformDirection(Vector3.forward) * 30);
		var rayDirection = -transform.TransformDirection(Vector3.forward);
        RaycastHit hit;

        lineRenderer.SetPosition(0, transform.position);

        if (Physics.Raycast(rayOrigin, rayDirection, out hit) && hit.collider.gameObject.name == "walkingPath"){
        	
        	// Debug.DrawRay(origin, direction, Color.green);
			lineRenderer.SetPosition(1, hit.point);
			teleportPoint = hit;

        }

    }

    public void teleport(){
    	playspace.GetComponent<stickToGround>().rotatePlayspace(teleportPoint.normal);
    	playspace.transform.position = teleportPoint.point;

    }

}