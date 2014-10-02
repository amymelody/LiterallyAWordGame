using UnityEngine;
using System.Collections;

public class LetterScript : MonoBehaviour
{
	void Start ()
    {
	
	}
	
	void Update ()
    {
	
	}

    public void Dropped()
    {
        //How did you cast to "Item" with GetComponent("Item")?
        GameObject[] letters = GameObject.Find("Lee").GetComponent<LetterTrackerScript>().GetLetterArray();

        foreach (GameObject l in letters)
        {
            float x = this.gameObject.transform.position.x;
            float letterX = l.transform.position.x
            if(
        }
    }
}
