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
}
