using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LetterScript : MonoBehaviour
{
    public List<GameObject> word;
    public string letter;

	void Start ()
    {
        word = new List<GameObject>();
	}
	
	void Update ()
    {
	}

    public void PickedUp()
    {
        foreach (GameObject l in this.word)
        {
            if (l != this.gameObject)
            {
                List<GameObject> letterWord = new List<GameObject>(l.GetComponent<LetterScript>().word);
                string lLetter = l.GetComponent<LetterScript>().letter;
                bool front = false;          //Is letter l before this letter in the array

                for(int i = 0; i < letterWord.Count; i++)
                {
                    if (letterWord[i] == this.gameObject)
                    {
                        int indexOfThis = letterWord.IndexOf(letterWord[i]);
                        if (front)      //letter l is in the front of the word
                        {
                            for (int j = letterWord.Count - 1; j >= indexOfThis; j--)
                            {
                                letterWord.RemoveAt(j);
                            }
                            if (letterWord.Count == 1) { letterWord.Clear(); }
                            l.GetComponent<LetterScript>().word = new List<GameObject>(letterWord);
                            break;
                        }
                        else        //letter l is in the back of the word
                        {
                            for (int j = indexOfThis; j >= 0; j--)
                            {
                                letterWord.RemoveAt(j);
                            }
                            if (letterWord.Count == 1) { letterWord.Clear(); }
                            l.GetComponent<LetterScript>().word = new List<GameObject>(letterWord);
                            break;
                        }

                    }
                    else if (letterWord[i] == l)
                    {
                        front = true;
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
            if (l != this.gameObject)
            {
                string lLetter = l.GetComponent<LetterScript>().letter;

                float mySizeY = this.gameObject.transform.lossyScale.y;
                float myY = this.gameObject.transform.position.y;
                float letterY = l.transform.position.y;
                if (myY - 1.5f * mySizeY <= letterY && myY >= letterY)        //If both letters are on the same platform level
                {
                    float mySizeX = this.gameObject.transform.lossyScale.x;
                    float myX = this.gameObject.transform.position.x;
                    float letterX = l.transform.position.x;
                    if (myX + 1.5f * mySizeX >= letterX && myX <= letterX)        //If my letter towards the front
                    {
                        if (word.Count == 0)
                        {
                            //snap to array
                            List<GameObject> letterWord = new List<GameObject>(l.GetComponent<LetterScript>().word);
                            if (letterWord.Count == 0)
                            {
                                myX = letterX - mySizeX;
                                this.gameObject.transform.position = new Vector3(myX, this.gameObject.transform.position.y, this.gameObject.transform.position.z);
                                word.Add(this.gameObject);
                                word.Add(l);
                                l.GetComponent<LetterScript>().word = new List<GameObject>(word);
                            }
                            else
                            {
                                int myNewIndex = letterWord.IndexOf(l);
                                letterWord.Insert(myNewIndex, this.gameObject);
                                l.GetComponent<LetterScript>().word = new List<GameObject>(letterWord);
                                word = new List<GameObject>(letterWord);
                                for (int i = myNewIndex; i < word.Count; i++)
                                {
                                    float tempX = word[i].transform.position.x + word[i].transform.lossyScale.x;
                                    word[i].transform.position = new Vector3(tempX, word[i].transform.position.y, word[i].transform.position.z);
                                }
                            }
                        }
                        else
                        {
                            //If you're appending the second word to the end
                            List<GameObject> letterWord = new List<GameObject>(l.GetComponent<LetterScript>().word);
                            if (letterWord.Count == 0)
                            {
                                letterX = this.gameObject.transform.position.x + mySizeX;
                                l.transform.position = new Vector3(letterX, l.transform.position.y, l.transform.position.z);
                                word.Add(l);
                                l.GetComponent<LetterScript>().word = new List<GameObject>(word);
                            }
                            else
                            {
                                for(int i = 0; i < letterWord.Count; i++)
                                {
                                    float tempX = word[word.Count - 1].transform.position.x + word[word.Count - 1].transform.lossyScale.x;
                                    letterWord[i].transform.position = new Vector3(tempX, letterWord[i].transform.position.y, letterWord[i].transform.position.z);
                                    word.Add(letterWord[i]);
                                }
                                l.GetComponent<LetterScript>().word = new List<GameObject>(word);
                            }
                        }
                    }
                    else if (myX >= letterX && myX - 1.5f * mySizeX <= letterX)   //If my letter is towards the back
                    {
                        if (word.Count == 0)
                        {
                            //snap to array
                            List<GameObject> letterWord = new List<GameObject>(l.GetComponent<LetterScript>().word);
                            if (letterWord.Count == 0)
                            {
                                myX = letterX + mySizeX;
                                this.gameObject.transform.position = new Vector3(myX, this.gameObject.transform.position.y, this.gameObject.transform.position.z);
                                word.Add(l);
                                word.Add(this.gameObject);
                                l.GetComponent<LetterScript>().word = new List<GameObject>(word);
                            }
                            else
                            {
                                int myNewIndex = letterWord.IndexOf(l) + 1;
                                letterWord.Insert(myNewIndex, this.gameObject);
                                l.GetComponent<LetterScript>().word = new List<GameObject>(letterWord);
                                word = new List<GameObject>(letterWord);
                                for (int i = myNewIndex; i < word.Count; i++)
                                {
                                    float tempX = word[i].transform.position.x + word[i].transform.lossyScale.x;
                                    word[i].transform.position = new Vector3(tempX, word[i].transform.position.y, word[i].transform.position.z);
                                }
                            }
                        }
                        else
                        {
                            //If you're appending the second word to the beginning
                            List<GameObject> letterWord = new List<GameObject>(l.GetComponent<LetterScript>().word);
                            if (letterWord.Count == 0)
                            {
                                letterX = this.gameObject.transform.position.x - l.transform.lossyScale.x;
                                l.transform.position = new Vector3(letterX, l.transform.position.y, l.transform.position.z);
                                word.Add(l);
                                l.GetComponent<LetterScript>().word = new List<GameObject>(word);
                            }
                            else
                            {
                                for (int i = letterWord.Count - 1; i >= 0; i--)
                                {
                                    float tempX = word[0].transform.position.x - letterWord[i].transform.lossyScale.x;
                                    letterWord[i].transform.position = new Vector3(tempX, letterWord[i].transform.position.y, letterWord[i].transform.position.z);
                                    word.Insert(0, letterWord[i]);
                                }
                                l.GetComponent<LetterScript>().word = new List<GameObject>(word);
                            }
                        }
                    }
                }
            }
        }
    }

    public string GetWord()
    {
        string stringWord = System.String.Empty;
        foreach(GameObject l in word)
        {
            stringWord += l.GetComponent<LetterScript>().letter;
        }
        print(stringWord);
        return stringWord;
    }
}
