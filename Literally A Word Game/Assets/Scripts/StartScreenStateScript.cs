using UnityEngine;
using System.Collections;

public class StartScreenStateScript : MonoBehaviour {

	public GUIStyle pressEnterStyle;
	public GUIStyle controlsStyle;
	string pressEnter;
	string controls;

	// Use this for initialization
	void Start () {
		pressEnter = "Press enter to begin.";
		controls = "\u2194  -  Walk       Space  -  Jump\n" +
			"\u2195  -  Climb       C  -  Pickup/Drop Object";
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Return) ||
		    Input.GetKeyDown(KeyCode.KeypadEnter)) {
			Application.LoadLevel("mainRoom");
		}
	}

	void OnGUI ()
	{
		GUI.BeginGroup (new Rect (0, 0, Screen.width, Screen.height));
		GUI.color = Color.black;
		GUI.Label(new Rect(0, Screen.height * 0.5f, 
		                   Screen.width, Screen.height * 0.2f), pressEnter, pressEnterStyle);
		GUI.Label(new Rect(Screen.width * 0.17f, Screen.height * 0.7f, 
		                   Screen.width * 0.8f, Screen.height * 0.3f), controls, controlsStyle);
		GUI.EndGroup ();	
	}
}
