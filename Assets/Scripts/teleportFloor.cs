using UnityEngine;
using System.Collections;

public class teleportFloor : MonoBehaviour {

	public Material defaultMaterial;
	public Material teleportationMaterial;

	public void Start(){
		defaultMaterial = GetComponent<MeshRenderer>().material;
	}

}
