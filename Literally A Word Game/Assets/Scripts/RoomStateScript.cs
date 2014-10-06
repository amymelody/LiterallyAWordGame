using UnityEngine;
using System.Collections;

public class RoomStateScript : MonoBehaviour {

	public static bool treeCreated = false;

	// Use this for initialization
	void Start () {
		if (treeCreated) {
			GameObject tree = (GameObject)Instantiate(Resources.Load("Prefabs/Tree"));
			tree.transform.position = new Vector3(-4.268143f, 6.182477f, 1.886556f);
			tree.transform.localEulerAngles = new Vector3(0f, 180f, 0f);
			
			GameObject treeLadder = (GameObject)Instantiate(Resources.Load("Prefabs/TreeLadder"));
			treeLadder.transform.position = new Vector3(-4.268143f, 6.182477f, 0.2479973f);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
