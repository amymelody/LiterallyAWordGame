using UnityEngine;
using System.Collections;

public class RoomStateScript : MonoBehaviour {

	public static bool treeCreated = false;
	public static bool waterfallCreated = false;
	public static bool mountainCreated = false;
	public static bool accessedClouds = false;
	public static bool accessedForest = false;
	public static bool cloudsCompleted = false;
	public static bool forestCompleted = false;

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
		if (forestCompleted && cloudsCompleted) {
			mountainCreated = true;
		}
		if (treeCreated) {
			CreateTree();
		}
		if (waterfallCreated) {
			CreateWaterfall();
		}
		if (mountainCreated) {
			CreateMountain();
		}
	}
	
	void Update() {
		if (fellDownWaterfall) {
			timeElapsed += Time.deltaTime;
			tempString = "This level has not been\ncreated yet!";
			if (timeElapsed >= 2f) {
				tempString = "";
				fellDownWaterfall = false;
				timeElapsed = 0;
			}
		}
	}

	public static void CreateTree() {
		GameObject leftWall = GameObject.Find ("LeftWall");
		if (leftWall) {
			GameObject.Destroy(leftWall);
		}

		GameObject ceiling = GameObject.Find ("Ceiling");
		if (ceiling) {
			GameObject.Destroy(ceiling);
		}

		GameObject tree = (GameObject)Instantiate(Resources.Load("Prefabs/Tree"));
		tree.transform.position = new Vector3(-3f, 6.929121f, 1.886556f);
		tree.transform.localEulerAngles = new Vector3(0f, 180f, 0f);
		
		GameObject treeLadder = (GameObject)Instantiate(Resources.Load("Prefabs/TreeLadder"));
		treeLadder.transform.position = new Vector3(-3f, 6.929121f, 0.2479973f);

		GameObject grass = (GameObject)Instantiate(Resources.Load ("Prefabs/FadingGrass"));
		grass.transform.position = new Vector3(grass.transform.position.x,
		                                       grass.transform.position.y,
		                                       1.9f);

		GameObject tree2 = (GameObject)Instantiate(Resources.Load("Prefabs/Tree"));
		tree2.transform.position = new Vector3(-7.2f, 6.929121f, 2f);
		tree2.transform.localEulerAngles = new Vector3(0f, 180f, 0f);
	}

	public static void CreateWaterfall() {
		GameObject waterfallFloor = GameObject.Find ("WaterfallFloor");
		if (waterfallFloor) {
			GameObject.Destroy(waterfallFloor);
		}
		GameObject waterfall = (GameObject)Instantiate(Resources.Load ("Prefabs/Waterfall"));
		waterfall.transform.position = new Vector3(2.100769f, -0.5f, 0.2479973f);
		waterfall.transform.localEulerAngles = new Vector3(0f, 180f, 0f);
	}

	public static void CreateMountain() {
		GameObject rightWall = GameObject.Find ("RightWall");
		if (rightWall) {
			GameObject.Destroy(rightWall);
		}
		GameObject mountain = (GameObject)Instantiate(Resources.Load("Prefabs/Mountain"));
		mountain.transform.position = new Vector3(11.63623f, 7.392538f, 4.62085f);
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
