using UnityEngine;
using System.Collections;

public class LevelTriggerScript : MonoBehaviour {

	void OnTriggerEnter(Collider collider) {
		if (collider.gameObject.name.Equals("Lee")) {
			if (gameObject.name.Contains("MainRoom")) {
				Application.LoadLevel("MainRoom");
			} else if (gameObject.name.Contains("CloudsTransition")) {
				Application.LoadLevel("CloudsTransition");
			} else if (gameObject.name.Contains("SeaTransition")) {
				collider.gameObject.transform.position = new Vector3(-3.24229f, 3.446644f, 0f);
				GameObject.Find("RoomState").GetComponent<RoomStateScript>().fellDownWaterfall = true;
			}
		}
	}
}
