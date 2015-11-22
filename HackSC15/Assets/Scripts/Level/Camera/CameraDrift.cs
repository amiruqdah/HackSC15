using UnityEngine;
using System.Collections;
using DG.Tweening;

[RequireComponent(typeof(Camera))]
public class CameraDrift : MonoBehaviour {

	private Camera cameraObj;
	private float initialFOV;
	public float amplitude;
	public float speed; 
	private float initalX;

	// Use this for initialization
	void Start () {
		cameraObj = this.GetComponent<Camera>();
		initialFOV = this.GetComponent<Camera>().fieldOfView;
		initalX = this.transform.position.x;
	}
	
	// Update is called once per frame
	void Update () {
		// create a slight varianec in their time scales and linear relations
		cameraObj.fieldOfView = initialFOV + amplitude * Mathf.Sin(speed * Time.time);
		this.transform.position = new Vector3(initalX + amplitude * Mathf.Cos (speed * Time.time), this.transform.position.y, this.transform.position.z);
	}
}
