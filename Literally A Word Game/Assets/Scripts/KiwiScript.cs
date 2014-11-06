using UnityEngine;
using System.Collections;

public class KiwiScript : MonoBehaviour {

	public GUIStyle dialogueStyle;
	string dialogueString;
	float timeElapsed;

	// Use this for initialization
	void Start () {
		dialogueString = "";
		if (RoomStateScript.forestCompleted) {
			if (Application.loadedLevelName.Contains("ForestLevel")) {
				Destroy (gameObject);
			}
		} else if (Application.loadedLevelName.Contains("AerieTransition")) {
			Destroy (gameObject);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Application.loadedLevelName.Contains("ForestLevel") && RoomStateScript.forestCompleted) {
			timeElapsed += Time.deltaTime;
			if (timeElapsed >= 0.7f) {
				transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, 180f, transform.localEulerAngles.z);
				transform.rigidbody.velocity = new Vector3(-5f, 0f, 0f);
			}
		}
	}

	void OnGUI ()
	{
		GUI.BeginGroup (new Rect (0, 0, Screen.width, Screen.height));
		GUI.color = Color.white;
		GUI.Label(new Rect(0f, 0f, Screen.width * 0.3f, Screen.height * 0.5f), dialogueString, dialogueStyle);
		GUI.EndGroup ();	
	}

	void OnTriggerEnter(Collider collider) {
		if (Application.loadedLevelName.Contains("ForestLevel") && collider.gameObject.name.Equals("Lee") && !RoomStateScript.forestCompleted) {
			RoomStateScript.forestCompleted = true;
			dialogueString = "I am a kiwi.\n Thank you for saving me!\n Follow me, Lee.";
		}
	}

	void OnTriggerExit(Collider collider) {
		if (Application.loadedLevelName.Contains("ForestLevel") && collider.gameObject.tag.Equals("Floor")) {
			Destroy (gameObject);
		}
	}
}
