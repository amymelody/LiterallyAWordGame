using UnityEngine;
using System.Collections;

public class LevelTriggerScript : MonoBehaviour {

	void OnTriggerEnter(Collider collider) {
		if (collider.gameObject.name.Equals("Lee")) {
			if (gameObject.name.Contains("MainRoom")) {
				Application.LoadLevel("MainRoom");
			} else if (gameObject.name.Contains("CloudsTransition")) {
				Application.LoadLevel("CloudsTransition");
			}
		}
	}
}
