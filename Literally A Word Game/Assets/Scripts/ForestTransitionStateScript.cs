using UnityEngine;
using System.Collections;

public class ForestTransitionStateScript : MonoBehaviour {

	private static bool visitedForestTransition = false;
	
	// Use this for initialization
	void Start () {
		if (!visitedForestTransition) {
			audio.Play();
			visitedForestTransition = true;
		}
	}
}
