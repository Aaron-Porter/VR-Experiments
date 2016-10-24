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
		}else{
			lineRenderer.SetWidth(width,width);
		}
	}

	public void Ray () {

		var origin = transform.position;
		var direction = transform.TransformDirection(Vector3.forward);
		var endPoint = origin + direction * 1000000;
        RaycastHit hit;

        lineRenderer.SetPosition(0, origin);

        if (Physics.Raycast(origin, direction, out hit) && hit.collider.gameObject.name == "walkingPath"){
        	
        	endPoint = hit.point;

        	// Debug.DrawRay(origin, direction, Color.green);
			lineRenderer.SetPosition(1, endPoint);
			teleportPoint = hit;

        }

    }

    public void teleport(){
    	playspace.GetComponent<stickToGround>().rotatePlayspace(teleportPoint.normal);
    	playspace.transform.position = teleportPoint.point;

    }

}