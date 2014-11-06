using UnityEngine;
using System.Collections;
using System.IO;

public class MetricManagerScript : MonoBehaviour {

	string createText = "";

	public float timeInMainRoom;
	public float timeInCloudsTransition;
	public float timeToCompleteCloudsLevel;
	public float timeInForestTransition;
	public float timeToCompleteForestLevel;

	void Start () {
		DontDestroyOnLoad(gameObject);
	}

	void Update () {}
	
	//When the game quits we'll actually write the file.
	void OnApplicationQuit(){
		GenerateMetricsString ();
		string time = System.DateTime.UtcNow.ToString ();string dateTime = System.DateTime.Now.ToString (); //Get the time to tack on to the file name
		time = time.Replace ("/", "-"); //Replace slashes with dashes, because Unity thinks they are directories..
		time = time.Replace (":", "-");
		string reportFile = "LiterallyAWordGame_Metrics_" + time + ".txt"; 
		File.WriteAllText (reportFile, createText);
		//In Editor, this will show up in the project folder root (with Library, Assets, etc.)
		//In Standalone, this will show up in the same directory as your executable
	}

	void GenerateMetricsString(){
		createText = 
			"Time spent in main room before creating something: " + timeInMainRoom + "s\n" +
				"Time spent in clouds transition before going to clouds level: " + timeInCloudsTransition + "s\n" +
				"Time to complete clouds level: " + timeToCompleteCloudsLevel + "s\n" +
				"Time spent in forest transition before going to clouds level: " + timeInForestTransition + "s\n" +
				"Time to complete forest level: " + timeToCompleteForestLevel + "s";
	}
}
