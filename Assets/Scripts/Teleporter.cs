using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using VRTK;

public class Teleporter : MonoBehaviour {

	VRTK_ControllerEvents controllerEvents;
	LineRenderer lineRenderer;
	public Color disabledColor;
	public Color activeColor;
	public float width;
	public bool rayVisible;
	RaycastHit teleportPoint;
	GameObject playspace;
	GameObject head;

	void Start(){
		playspace = GameObject.Find("[CameraRig]");
		head = GameObject.Find("Camera (eye)");
		lineRenderer = gameObject.AddComponent<LineRenderer>() as LineRenderer;
		lineRenderer.material = new Material (Shader.Find("Particles/Additive"));
		lineRenderer.enabled = false;
		lineRenderer.SetVertexCount(2);
		lineRenderer.SetColors(disabledColor, disabledColor);


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
        lineRenderer.SetPosition(1, transform.position + (transform.TransformDirection(Vector3.forward) * 30));
        lineRenderer.SetColors(disabledColor, disabledColor);

        if (Physics.Raycast(rayOrigin, rayDirection, out hit) && hit.collider.gameObject.name == "walkingPath"){
        	
        	// Debug.DrawRay(origin, direction, Color.green);
        	lineRenderer.SetColors(activeColor, activeColor);
			teleportPoint = hit;

        }

    }

    public void teleport(){

    	// re-orient
    	playspace.GetComponent<stickToGround>().rotatePlayspace(teleportPoint.normal);

    	// re-position
    	// var teleportPosition = teleportPoint.point;
    	// teleportPosition.x = (teleportPosition.x - (head.transform.position.x - playspace.transform.position.x));
     	// teleportPosition.z = (teleportPosition.z - (head.transform.position.z - playspace.transform.position.z));

     	var offset = head.transform.localPosition;
     		offset.y = 0;
    	playspace.transform.position = teleportPoint.point;
    	playspace.transform.Translate(-offset, Space.Self);


    }


}