using UnityEngine;
using System.Collections;

public class HatchlingScript : MonoBehaviour {

	public Sprite hatchedSprite;
	public GUIStyle dialogueStyle;
	string dialogueString;

	// Use this for initialization
	void Start () {
		if (RoomStateScript.cloudsCompleted && RoomStateScript.forestCompleted) {
			dialogueString = "Thank you, Lee,\n for saving my mom and dad.\n Now let me take you somewhere\n that is sure to make you glad.";
		} else {
			dialogueString = "\"Oh no!\" Lee said with a worried tone.\n This poor little egg is all alone.";
		}
		if (RoomStateScript.cloudsCompleted && RoomStateScript.forestCompleted) {
			GetComponent<SpriteRenderer>().sprite = hatchedSprite;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI ()
	{
		GUI.BeginGroup (new Rect (0, 0, Screen.width, Screen.height));
		GUI.color = Color.black;
		GUI.Label(new Rect(0f, 0f, Screen.width * 0.5f, Screen.height * 0.5f), dialogueString, dialogueStyle);
		GUI.EndGroup ();	
	}

	void OnTriggerEnter(Collider collider) {
		if (collider.gameObject.name.Equals("Lee") && 
		    RoomStateScript.cloudsCompleted && 
		    RoomStateScript.forestCompleted) {
			Application.LoadLevel("EndScene");
		}
	}
}
