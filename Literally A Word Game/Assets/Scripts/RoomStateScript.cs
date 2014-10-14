using UnityEngine;
using System.Collections;

public class RoomStateScript : MonoBehaviour {

	public static bool treeCreated = false;
	public static bool waterfallCreated = false;
	public bool fellDownWaterfall = false;
	private static bool visitedRoom = false;

	public GUIStyle tempStyle;
	string tempString;
	float timeElapsed;

	// Use this for initialization
	void Start () {
		tempString = "";
		timeElapsed = 0;
		if (!visitedRoom) {
			audio.Play();
			visitedRoom = true;
		}
		if (treeCreated) {
			GameObject tree = (GameObject)Instantiate(Resources.Load("Prefabs/Tree"));
			tree.transform.position = new Vector3(-4.268143f, 6.929121f, 1.886556f);
			tree.transform.localEulerAngles = new Vector3(0f, 180f, 0f);
			
			GameObject treeLadder = (GameObject)Instantiate(Resources.Load("Prefabs/TreeLadder"));
			treeLadder.transform.position = new Vector3(-4.268143f, 6.929121f, 0.2479973f);
		}
		if (waterfallCreated) {
			GameObject waterfallFloor = GameObject.Find ("WaterfallFloor");
			if (waterfallFloor) {
				GameObject.Destroy(waterfallFloor);
			}
			GameObject waterfall = (GameObject)Instantiate(Resources.Load ("Prefabs/Waterfall"));
			waterfall.transform.position = new Vector3(2.100769f, -0.5f, 0.2479973f);
			waterfall.transform.localEulerAngles = new Vector3(0f, 180f, 0f);
		}
	}
	
	void Update() {
		if (fellDownWaterfall) {
			timeElapsed += Time.deltaTime;
			tempString = "This level is not available yet!";
			if (timeElapsed >= 2f) {
				tempString = "";
				fellDownWaterfall = false;
				timeElapsed = 0;
			}
		}
	}
	
	void OnGUI ()
	{
		GUI.BeginGroup (new Rect (0, 0, Screen.width, Screen.height));
		GUI.color = Color.black;
		GUI.Label(new Rect(Screen.width * 0.5f, Screen.height * 0.3f, 
		                   Screen.width * 0.5f, Screen.height * 0.5f), tempString, tempStyle);
		GUI.EndGroup ();	
	}
}
