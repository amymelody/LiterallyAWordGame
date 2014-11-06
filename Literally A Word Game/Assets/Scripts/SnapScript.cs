using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SnapScript : MonoBehaviour
{
    GameObject[] letters;
    bool snapped;
    bool wordfound;

    //Do not allow a letter with this script to be by itself starting off
	void Start ()
    {
        snapped = false;
        wordfound = false;
	}

    void Update()
    {
        if(!snapped)
        {
            string lLetter = gameObject.GetComponent<LetterScript>().letter;

            letters = GameObject.FindGameObjectsWithTag("Letter");
            LetterScript ls = gameObject.GetComponent<LetterScript>();
            ls.word.Add(gameObject);

            GameObject nextLetter = AddNextLetter(ls);
            while (nextLetter != null)
            {
                nextLetter = AddNextLetter(nextLetter.GetComponent<LetterScript>());
            }

            print("Letter: " + gameObject.GetComponent<LetterScript>().letter + ", Word: " + gameObject.GetComponent<LetterScript>().GetWord());
            snapped = true;
        }
    }

    GameObject AddNextLetter(LetterScript ls)
    {
        foreach (GameObject l in letters)
        {
            if (l != ls.gameObject)
            {
                string lLetter = l.GetComponent<LetterScript>().letter;

                float mySizeY = ls.gameObject.transform.lossyScale.y;
                float myY = ls.gameObject.transform.position.y;
                float letterY = l.transform.position.y;
                if (myY - 1.5f * mySizeY <= letterY && myY >= letterY)        //If both letters are on the same platform level
                {
                    float mySizeX = ls.gameObject.transform.lossyScale.x;
                    float myX = ls.gameObject.transform.position.x;
                    float letterX = l.transform.position.x;
                    if (myX + 1.05f * mySizeX >= letterX && myX <= letterX)        //If sequential letter
                    {
                        l.transform.position = new Vector3(myX + mySizeX, ls.transform.position.y, ls.transform.position.z);
                        ls.word.Add(l);
                        foreach (GameObject j in ls.word)
                        {
                            j.GetComponent<LetterScript>().word = new List<GameObject>(ls.word);
                        }
                        print("Letter: " + lLetter + ", Word: " + l.GetComponent<LetterScript>().GetWord());
                        return l;
                    }
                }
            }
        }
        print("DONE");
        return null;
    }

    void OnTriggerEnter(Collider collider)
    {
        if (!wordfound)
        {
            if (collider.gameObject.tag.Equals("Player"))
            {
                LetterScript ls = gameObject.GetComponent<LetterScript>();
                LetterTrackerScript ts = collider.gameObject.GetComponent<LetterTrackerScript>();
                ts.WordFind(ls, ls.GetWord(), ls.GetWordCenterPosition(ls.word, ls.gameObject));
                wordfound = true;
            }
        }
    }
}
