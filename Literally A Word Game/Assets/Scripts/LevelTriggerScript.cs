using UnityEngine;
using System.Collections;

public class LevelTriggerScript : MonoBehaviour {

    public static string currentLevel;

    // Use this for initialization
    void Start()
    {
        currentLevel = Application.loadedLevelName;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
                Application.LoadLevel("OptionsScreen");
        }
    }

	void OnTriggerEnter(Collider collider) {
		if (collider.gameObject.name.Equals("Lee")) {
			if (gameObject.name.Contains("MainRoom")) {
				Application.LoadLevel("MainRoom");
			} else if (gameObject.name.Contains("CloudsTransition")) {
				Application.LoadLevel("CloudsTransition");
			} else if (gameObject.name.Contains("ForestTransition")) {
				Application.LoadLevel("ForestTransition");
			} else if (gameObject.name.Contains("SeaTransition")) {
				collider.gameObject.transform.position = new Vector3(-3.24229f, 3.446644f, 0f);
				GameObject.Find("RoomState").GetComponent<RoomStateScript>().fellDownWaterfall = true;
			} else if (gameObject.name.Contains("AerieTransition")) {
				Application.LoadLevel("AerieTransition");
			} else if (gameObject.name.Contains("ForestLevel")) {
				RoomStateScript.accessedForest = true;
				Application.LoadLevel("ForestLevel");
			} else if (gameObject.name.Contains("CloudLevel")) {
				RoomStateScript.accessedClouds = true;
				Application.LoadLevel("CloudLevel");
            } else if (gameObject.name.Contains("Reset")) {
            Application.LoadLevel(Application.loadedLevelName);
            }
		}
	}
}
