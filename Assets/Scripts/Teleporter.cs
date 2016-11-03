using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using VRTK;

public class Teleporter : MonoBehaviour {

	// instiate variables
	VRTK_ControllerEvents controllerEvents;
	LineRenderer lineRenderer;
	public Color disabledColor;
	public Color activeColor;
	public float width;
	public bool rayVisible;
	RaycastHit teleportPoint;
	GameObject playspace;
	GameObject head;
	GameObject floor;
	GameObject projector;
	bool readyToTeleport;

	Material defaultFloorMaterial;
	public Material floorMaterial;

	// STATES
		// default -- defualt state before ever using teleportation
		// beforeTeleporting --- activated after when user choosing position
		// teleporting --- during teleportation, after choosing position
		// doneTeleporting --- after teleportation is finished
		
		string State = "default";


	void Start(){
		// get objects
		playspace = GameObject.Find("[CameraRig]");
		projector = GameObject.Find("teleporterProjector");
		floor = GameObject.Find("Floor");
		head = GameObject.Find("Camera (eye)");

		defaultFloorMaterial = floor.GetComponent<MeshRenderer>().material;

		// Set up line renderer for teleport line
		lineRenderer = gameObject.AddComponent<LineRenderer>() as LineRenderer;
		lineRenderer.material = new Material (Shader.Find("Particles/Additive"));
		lineRenderer.enabled = false;
		lineRenderer.SetVertexCount(2);
		lineRenderer.SetColors(disabledColor, disabledColor);
	}


	void Update(){

		stateStyle();

		if(rayVisible == true){
			Ray();
		}

	}


	public void castRay(bool value){

		rayVisible = value;
		if(value == false){
			State = "doneTeleporting";
			lineRenderer.SetWidth(0,0);
			lineRenderer.enabled = false;
		}else{
			State = "beforeTeleporting";
			lineRenderer.SetWidth(width,width);
			lineRenderer.enabled = true;
		}

	}

	public void Ray () {

		var rayOrigin = transform.position;
		var rayDirection = transform.TransformDirection(Vector3.forward);
        RaycastHit hit;

        // setup visible ray
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, transform.position + (transform.TransformDirection(Vector3.forward) * 30));
        lineRenderer.SetColors(disabledColor, disabledColor);
        readyToTeleport = false;


        // cast physics ray to detect floor
        if (Physics.Raycast(rayOrigin, rayDirection, out hit) && hit.collider.gameObject.name == "Floor"){
        	readyToTeleport = true;
        	
        	// Debug.DrawRay(origin, direction, Color.green);
        	lineRenderer.SetColors(activeColor, activeColor);
			teleportPoint = hit;

			// position teleporter projector
			projector.transform.position = hit.point + (hit.normal * 5);
			projector.transform.LookAt(hit.point);

        }

    }

    public void teleport(){

    	if(readyToTeleport){
	    	// re-orient
	    	playspace.GetComponent<stickToGround>().rotatePlayspace(-teleportPoint.normal);


	     	var offset = head.transform.localPosition;
	     		offset.y = 0;

	    	playspace.transform.position = teleportPoint.point;
	    	playspace.transform.Translate(-offset, Space.Self);
	    }

    }

    public void stateStyle(){

    	if(State == "default" || State == "doneTeleporting"){

    		floor.GetComponent<MeshRenderer>().material = floor.GetComponent<teleportFloor>().defaultMaterial;

    	}else if(State == "beforeTeleporting"){

    		floor.GetComponent<MeshRenderer>().material = floor.GetComponent<teleportFloor>().teleportationMaterial;

    	}

    }


}