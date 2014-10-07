using UnityEngine;
using System.Collections;

public class CloudsTransitionStateScript : MonoBehaviour {

	private static bool visitedCloudsTransition = false;
	
	// Use this for initialization
	void Start () {
		if (!visitedCloudsTransition) {
			audio.Play();
			visitedCloudsTransition = true;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
