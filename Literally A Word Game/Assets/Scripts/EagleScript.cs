using UnityEngine;
using System.Collections;

public class EagleScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		if (!RoomStateScript.cloudsCompleted && Application.loadedLevelName.Contains("AerieTransition")) {
			Destroy (gameObject);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
