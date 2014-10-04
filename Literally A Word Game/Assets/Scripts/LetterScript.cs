using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LetterScript : MonoBehaviour
{
    public List<GameObject> word;
	void Start ()
    {
        word = new List<GameObject>();
	}
	
	void Update ()
    {
	}

    public void PickedUp()
    {
        foreach (GameObject l in word)
        {
            List<GameObject> letterWord = l.GetComponent<LetterScript>().word;
            bool front = false;          //Is letter l before this letter in the array

            foreach(GameObject i in letterWord)
            {
                if (i == l)
                {
                    front = true;
                }
                else if (i == this.gameObject)
                {
                    if (front)      //letter l is in the front of the word
                    {
                        for (int j = letterWord.Count - 1; j >= letterWord.IndexOf(i); j--)
                        {
                            letterWord.RemoveAt(j);
                        }
                        break;
                    }
                    else        //letter l is in the back of the word
                    {
                        for (int j = letterWord.IndexOf(i); j >= 0; j--)
                        {
                            letterWord.RemoveAt(j);
                        }
                        break;
                    }

                }
            }
        }
        word.Clear();
    }

    public void Dropped()
    {
        GameObject[] letters = GameObject.Find("Lee").GetComponent<LetterTrackerScript>().GetLetterArray();

        foreach (GameObject l in letters)
        {
            float mySizeY = this.gameObject.transform.lossyScale.y;
            float myY = this.gameObject.transform.position.y;
            float letterY = l.transform.position.y;
            if (myY - 1.5f * mySizeY < letterY && myY > letterY)        //If both letters are on the same platform level
            {
                float mySizeX = this.gameObject.transform.lossyScale.x;
                float myX = this.gameObject.transform.position.x;
                float letterX = l.transform.position.x;
                if (myX + 1.5f * mySizeX >= letterX && myX <= letterX)        //If my letter is to the right
                {
                    if (word.Count == 0)
                    {
                        //snap to array
                        List<GameObject> letterWord = l.GetComponent<LetterScript>().word;
                        if (letterWord.Count == 0)
                        {
                            myX = letterX - mySizeX;
                            this.gameObject.transform.position = new Vector3(myX, this.gameObject.transform.position.y, this.gameObject.transform.position.z);
                            word.Add(this.gameObject);
                            word.Add(l);
                            letterWord = word;
                        }
                        else
                        {
                            int myNewIndex = letterWord.IndexOf(l);
                            letterWord.Insert(myNewIndex, this.gameObject);
                            word = letterWord;
                            for (int i = myNewIndex; i < letterWord.Count; i++)
                            {
                                float tempX = letterWord[i].transform.position.x + letterWord[i].transform.lossyScale.x;
                                letterWord[i].transform.position = new Vector3(tempX, letterWord[i].transform.position.y, letterWord[i].transform.position.z);
                            }
                        }
                    }
                    else
                    {
                        //If you're appending the second word to the end
                        List<GameObject> letterWord = l.GetComponent<LetterScript>().word;
                        if (letterWord.Count == 0)
                        {
                            letterX = this.gameObject.transform.position.x + mySizeX;
                            l.transform.position = new Vector3(letterX, l.transform.position.y, l.transform.position.z);
                            word.Add(l);
                            letterWord = word;
                        }
                        else
                        {
                            foreach(gameObject
                            word.Add(
                            int myNewIndex = letterWord.IndexOf(l);
                            letterWord.Insert(myNewIndex, this.gameObject);
                            word = letterWord;
                            for (int i = myNewIndex; i < letterWord.Count; i++)
                            {
                                float tempX = letterWord[i].transform.position.x + letterWord[i].transform.lossyScale.x;
                                letterWord[i].transform.position = new Vector3(tempX, letterWord[i].transform.position.y, letterWord[i].transform.position.z);
                            }
                        }

                    }
                }
                else if (myX >= letterX && myX - 1.5f * mySizeX <= letterX)   //If my letter is to the left
                {
                    if (word.Count == 0)
                    {
                        //snap to array
                        List<GameObject> letterWord = l.GetComponent<LetterScript>().word;
                        if (letterWord.Count == 0)
                        {
                            myX = letterX + mySizeX;
                            this.gameObject.transform.position = new Vector3(myX, this.gameObject.transform.position.y, this.gameObject.transform.position.z);
                            word.Add(l.gameObject);
                            letterWord.Add(l.gameObject); 
                            word.Add(this.gameObject);
                            letterWord.Add(this.gameObject);
                        }
                        else
                        {
                            int myNewIndex = letterWord.IndexOf(l) + 1;
                            letterWord.Insert(myNewIndex, this.gameObject);
                            word = letterWord;
                            for (int i = myNewIndex; i < letterWord.Count; i++)
                            {
                                float tempX = letterWord[i].transform.position.x + letterWord[i].transform.lossyScale.x;
                                letterWord[i].transform.position = new Vector3(tempX, letterWord[i].transform.position.y, letterWord[i].transform.position.z);
                            }
                        }
                    }
                    //If you're connecting two strings
                }
            }
        }
    }
}
