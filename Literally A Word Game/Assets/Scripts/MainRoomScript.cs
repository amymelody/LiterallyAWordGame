using UnityEngine;
using System.Collections;

public class MainRoomScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider collider) {
		if (collider.gameObject.name.Equals("Lee")) {
			Application.LoadLevel("MainRoom");
		}
	}
}
