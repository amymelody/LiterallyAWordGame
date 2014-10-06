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

    public void WordFind(string word)
    {
        switch (word)
        {
            case "TREE":
                //SUMMON TREE
                print("SUMMON TREE");
                break;
            default:
                break;
        }
    }
}
