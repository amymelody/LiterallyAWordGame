using UnityEngine;
using System.Collections;

public class LetterTrackerScript : MonoBehaviour
{ 
    private GameObject[] letters;

    //Should replace with a "enter new area" function
	void Start ()
    {
        letters = GameObject.FindGameObjectsWithTag("Letter");
	}

	void Update ()
    {
	
	}

    public GameObject[] GetLetterArray()
    {
        return letters;
    }

    public void WordFind(string word, Vector3 wordCenterPosition)
    {
        switch (word)
        {
	    case "TREE":
			if (!RoomStateScript.treeCreated) {
				RoomStateScript.treeCreated = true;
				GameObject tree = (GameObject)Instantiate(Resources.Load("Prefabs/Tree"));
				tree.transform.position = new Vector3(-4.268143f, 6.182477f, 1.886556f);
				tree.transform.localEulerAngles = new Vector3(0f, 180f, 0f);

				GameObject treeLadder = (GameObject)Instantiate(Resources.Load("Prefabs/TreeLadder"));
				treeLadder.transform.position = new Vector3(-4.268143f, 6.182477f, 0.2479973f);
			}
            break;
		case "UP":
			if (!GameObject.Find("UPBalloons(Clone)")) {
				GameObject upBalloons = (GameObject)Instantiate(Resources.Load("Prefabs/UPBalloons"));
				upBalloons.transform.position = new Vector3(wordCenterPosition.x,
				                                            wordCenterPosition.y + upBalloons.transform.localScale.y / 2.0f,
				                                            wordCenterPosition.z);
				upBalloons.transform.localEulerAngles = new Vector3(0f, 180f, 0f);
				GameObject.Destroy(GameObject.Find("LetterU"));
				GameObject.Destroy(GameObject.Find("LetterP"));
				gameObject.GetComponent<LeeScript>().closeObjects.Clear();
			}
			break;
	    default:
	        break;
        }
    }
}
