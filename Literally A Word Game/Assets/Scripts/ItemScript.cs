using UnityEngine;
using System.Collections;

public class ItemScript : MonoBehaviour {

	public bool canBeDropped;

	// Use this for initialization
	void Start () {
		canBeDropped = true;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter(Collision collision) {
		canBeDropped = false;
	}

	void OnCollisionExit(Collision collision) {
		canBeDropped = true;
	}
}
