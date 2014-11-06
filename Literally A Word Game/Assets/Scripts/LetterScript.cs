using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LetterScript : MonoBehaviour
{
    public List<GameObject> word;
    public string letter;
    public Texture defaultTex;
    public Texture currentTex;
    public Texture selectTex;
    public bool canBeDropped;

	public GameObject objectTouching;

    void Start()
    {
        word = new List<GameObject>();
        defaultTex = renderer.material.GetTexture("_MainTex");
        currentTex = defaultTex;
        canBeDropped = true;
    }

    void Update()
    {
    }

    void OnDestroy()
    {
        GameObject lee = GameObject.Find("Lee");
        if (lee)
        {
            lee.GetComponent<LeeScript>().RemoveObject(gameObject);
        }
    }

    public void PickedUp()
    {
        List<LetterScript> scriptsToCheck = new List<LetterScript>(); ;
        foreach (GameObject l in this.word)
        {
            if (l != this.gameObject)
            {
                List<GameObject> letterWord = new List<GameObject>(l.GetComponent<LetterScript>().word);
                string lLetter = l.GetComponent<LetterScript>().letter;
                bool front = false;          //Is letter l before this letter in the array

                for (int i = 0; i < letterWord.Count; i++)
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
                            //Add new words to wordsLeft list
                            bool newWord = true;
                            foreach (LetterScript s in scriptsToCheck)
                            {
                                if (l.GetComponent<LetterScript>().GetWord() == s.GetWord())
                                {
                                    newWord = false;
                                    break;
                                }
                            }
                            if (newWord) { scriptsToCheck.Add(l.GetComponent<LetterScript>()); }
                            print("Letter: " + lLetter + ", Word: " + l.GetComponent<LetterScript>().GetWord());
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
                            //Add new words to wordsLeft list
                            bool newWord = true;
                            foreach (LetterScript s in scriptsToCheck)
                            {
                                if (l.GetComponent<LetterScript>().GetWord() == s.GetWord())
                                {
                                    newWord = false;
                                    break;
                                }
                            }
                            if (newWord) { scriptsToCheck.Add(l.GetComponent<LetterScript>()); }
                            print("Letter: " + lLetter + ", Word: " + l.GetComponent<LetterScript>().GetWord());
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
        foreach (LetterScript s in scriptsToCheck)
        {
            GameObject.Find("Lee").GetComponent<LetterTrackerScript>().WordFind(s, s.GetWord(), GetWordCenterPosition(s.word, s.gameObject));
        }
    }

    public void Dropped()
    {
        GameObject[] letters = GameObject.FindGameObjectsWithTag("Letter");

        foreach (GameObject l in letters)
        {
            if (l != this.gameObject && !(word.Exists(o => o == l)))
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
                        //snap to array
                        List<GameObject> letterWord = new List<GameObject>(l.GetComponent<LetterScript>().word);
                        if (letterWord.Count == 0)
                        {
                            myX = letterX - mySizeX;
                            this.gameObject.transform.position = new Vector3(myX, this.gameObject.transform.position.y, this.gameObject.transform.position.z);
                            word.Add(this.gameObject);
                            word.Add(l);
                            l.GetComponent<LetterScript>().word = new List<GameObject>(word);
                            WordMerge(word[0]);
                            //return;
                        }
                        else
                        {
                            int myNewIndex = letterWord.IndexOf(l);
                            letterWord.Insert(myNewIndex, this.gameObject);
                            foreach (GameObject j in letterWord)
                            {
                                j.GetComponent<LetterScript>().word = new List<GameObject>(letterWord);
                                string jLetter = j.GetComponent<LetterScript>().letter;
                            }
                            float tempX;
                            for (int i = myNewIndex; i < word.Count; i++)
                            {
                                if (i == 0)
                                {
                                    tempX = word[i + 1].transform.position.x - word[i + 1].transform.lossyScale.x;
                                    word[i].transform.position = new Vector3(tempX, word[i + 1].transform.position.y, word[i + 1].transform.position.z);
                                }
                                else
                                {
                                    tempX = word[i - 1].transform.position.x + word[i - 1].transform.lossyScale.x;
                                    word[i].transform.position = new Vector3(tempX, word[i - 1].transform.position.y, word[i - 1].transform.position.z);
                                }
                            }
                            if (myNewIndex == 0)
                            {
                                WordMerge(word[0]);
                            }
                            WordMerge(word[word.Count - 1]);
                            //return;
                        }
                    }
                    else if (myX >= letterX && myX - 1.5f * mySizeX <= letterX)   //If my letter is towards the back
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
                            WordMerge(word[word.Count - 1]);
                            //return;
                        }
                        else
                        {
                            int myNewIndex = letterWord.IndexOf(l) + 1;
                            letterWord.Insert(myNewIndex, this.gameObject);
                            foreach (GameObject j in letterWord)
                            {
                                j.GetComponent<LetterScript>().word = new List<GameObject>(letterWord);
                                string jLetter = j.GetComponent<LetterScript>().letter;
                            }
                            float tempX;
                            for (int i = myNewIndex; i < word.Count; i++)
                            {
                                //always at least the second letter, so i - 1 is safe
                                tempX = word[i - 1].transform.position.x + word[i - 1].transform.lossyScale.x;
                                word[i].transform.position = new Vector3(tempX, word[i - 1].transform.position.y, word[i - 1].transform.position.z);
                            }
                            WordMerge(word[word.Count - 1]);
                            //return;
                        }
                    }
                }
                print("Letter: " + lLetter + ", Word: " + l.GetComponent<LetterScript>().GetWord());
            }
        }
        print("***Chosen letter, word: " + this.GetWord());
        GameObject.Find("Lee").GetComponent<LetterTrackerScript>().WordFind(this, this.GetWord(), GetWordCenterPosition(word, gameObject));
    }

    public void WordMerge(GameObject ltr)
    {
        GameObject[] letters = GameObject.FindGameObjectsWithTag("Letter");
        List<GameObject> ltrWord = ltr.GetComponent<LetterScript>().word;
        foreach (GameObject l in letters)
        {
            if (!(ltrWord.Exists(o => o == l)))      //If letter l isn't in the original array
            {
                string lLetter = l.GetComponent<LetterScript>().letter;

                float ltrSizeY = ltr.transform.lossyScale.y;
                float ltrY = ltr.transform.position.y;
                float letterY = l.transform.position.y;
                if (ltrY - 1.5f * ltrSizeY <= letterY && ltrY >= letterY)        //If both letters are on the same platform level
                {
                    float ltrSizeX = ltr.transform.lossyScale.x;
                    float ltrX = ltr.transform.position.x;
                    float letterX = l.transform.position.x;
                    if (ltrX + 1.5f * ltrSizeX >= letterX && ltrX <= letterX)        //If my letter towards the front
                    {
                        //If you're appending the second word to the end
                        List<GameObject> letterWord = new List<GameObject>(l.GetComponent<LetterScript>().word);
                        if (letterWord.Count == 0)
                        {
                            letterX = ltr.transform.position.x + ltrSizeX;
                            l.transform.position = new Vector3(letterX, l.transform.position.y, l.transform.position.z);
                            ltrWord.Add(l);
                            for (int i = 0; i < ltrWord.Count; i++)
                            {
                                ltrWord[i].GetComponent<LetterScript>().word = new List<GameObject>(ltrWord);
                            }
                        }
                        else
                        {
                            float tempX;
                            for (int i = 0; i < letterWord.Count; i++)
                            {
                                tempX = ltrWord[ltrWord.Count - 1].transform.position.x + ltrWord[ltrWord.Count - 1].transform.lossyScale.x;
                                letterWord[i].transform.position = new Vector3(tempX, letterWord[i].transform.position.y, letterWord[i].transform.position.z);
                                ltrWord.Add(letterWord[i]);
                            }
                            for (int i = 0; i < ltrWord.Count; i++)
                            {
                                ltrWord[i].GetComponent<LetterScript>().word = new List<GameObject>(ltrWord);
                            }
                        }
                    }
                    else if (ltrX >= letterX && ltrX - 1.5f * ltrSizeX <= letterX)   //If my letter is towards the back
                    {
                        //If you're appending the second word to the beginning
                        List<GameObject> letterWord = new List<GameObject>(l.GetComponent<LetterScript>().word);
                        if (letterWord.Count == 0)
                        {
                            letterX = ltr.transform.position.x - l.transform.lossyScale.x;
                            l.transform.position = new Vector3(letterX, l.transform.position.y, l.transform.position.z);
                            ltrWord.Insert(0, l);
                            for (int i = 0; i < ltrWord.Count; i++)
                            {
                                ltrWord[i].GetComponent<LetterScript>().word = new List<GameObject>(ltrWord);
                            }
                        }
                        else
                        {
                            float tempX;
                            for (int i = letterWord.Count - 1; i >= 0; i--)
                            {
                                tempX = ltrWord[0].transform.position.x - letterWord[i].transform.lossyScale.x;
                                letterWord[i].transform.position = new Vector3(tempX, letterWord[i].transform.position.y, letterWord[i].transform.position.z);
                                ltrWord.Insert(0, letterWord[i]);
                            }
                            for (int i = 0; i < ltrWord.Count; i++)
                            {
                                ltrWord[i].GetComponent<LetterScript>().word = new List<GameObject>(ltrWord);
                            }
                        }
                    }
                }
            }
        }
    }

    public void ColorHint(string goalWord)
    {
        switch (goalWord)
        {
            case "TREE":
                foreach (GameObject l in word)
                {
                    string ltr = l.GetComponent<LetterScript>().letter;
                    if (ltr == "T") { l.renderer.material.SetTexture("_MainTex", (Texture)Resources.Load("T_Green")); }
                    else if (ltr == "R") { l.renderer.material.SetTexture("_MainTex", (Texture)Resources.Load("R_Green")); }
                    else if (ltr == "E") { l.renderer.material.SetTexture("_MainTex", (Texture)Resources.Load("E_Green")); }
                    l.GetComponent<LetterScript>().currentTex = l.renderer.material.GetTexture("_MainTex");
                }
                break;
            case "WATER":
                foreach (GameObject l in word)
                {
                    string ltr = l.GetComponent<LetterScript>().letter;
                    if (ltr == "W") { l.renderer.material.SetTexture("_MainTex", (Texture)Resources.Load("W_Blue")); }
                    else if (ltr == "A") { l.renderer.material.SetTexture("_MainTex", (Texture)Resources.Load("A_Blue")); }
                    else if (ltr == "T") { l.renderer.material.SetTexture("_MainTex", (Texture)Resources.Load("T_Blue")); }
                    else if (ltr == "E") { l.renderer.material.SetTexture("_MainTex", (Texture)Resources.Load("E_Blue")); }
                    else if (ltr == "R") { l.renderer.material.SetTexture("_MainTex", (Texture)Resources.Load("R_Blue")); }
                    l.GetComponent<LetterScript>().currentTex = l.renderer.material.GetTexture("_MainTex");
                }
                break;
            case "AERIE":
                foreach (GameObject l in word)
                {
                    string ltr = l.GetComponent<LetterScript>().letter;
                    if (ltr == "A") { l.renderer.material.SetTexture("_MainTex", (Texture)Resources.Load("A_Pink")); }
                    else if (ltr == "E") { l.renderer.material.SetTexture("_MainTex", (Texture)Resources.Load("E_Pink")); }
                    else if (ltr == "R") { l.renderer.material.SetTexture("_MainTex", (Texture)Resources.Load("R_Pink")); }
                    else if (ltr == "I") { l.renderer.material.SetTexture("_MainTex", (Texture)Resources.Load("I_Pink")); }
                    l.GetComponent<LetterScript>().currentTex = l.renderer.material.GetTexture("_MainTex");
                }
                break;
            case "BRIDGE":
                foreach (GameObject l in word)
                {
                    string ltr = l.GetComponent<LetterScript>().letter;
                    if (ltr == "B") { l.renderer.material.SetTexture("_MainTex", (Texture)Resources.Load("B_Gold")); }
                    else if (ltr == "R") { l.renderer.material.SetTexture("_MainTex", (Texture)Resources.Load("R_Gold")); }
                    else if (ltr == "I") { l.renderer.material.SetTexture("_MainTex", (Texture)Resources.Load("I_Gold")); }
                    else if (ltr == "D") { l.renderer.material.SetTexture("_MainTex", (Texture)Resources.Load("D_Gold")); }
                    else if (ltr == "G") { l.renderer.material.SetTexture("_MainTex", (Texture)Resources.Load("G_Gold")); }
                    l.GetComponent<LetterScript>().currentTex = l.renderer.material.GetTexture("_MainTex");
                }
                break;
            default:    //Reset
                foreach (GameObject l in word)
                {
                    l.renderer.material.SetTexture("_MainTex", l.GetComponent<LetterScript>().defaultTex);
                    l.GetComponent<LetterScript>().currentTex = l.renderer.material.GetTexture("_MainTex");
                }
                break;
        }
    }

    public string GetWord()
    {
        string stringWord = System.String.Empty;
        foreach (GameObject l in word)
        {
            stringWord += l.GetComponent<LetterScript>().letter;
        }
        return stringWord;
    }

    public Vector3 GetWordCenterPosition(List<GameObject> myWord, GameObject myLetter)
    {
        int lettersBeforeMe = myWord.IndexOf(myLetter);
        float x = myLetter.transform.position.x - myLetter.transform.localScale.x / 2.0f - myLetter.transform.localScale.x * (float)lettersBeforeMe;
        float y = 0f;
        foreach (GameObject l in myWord)
        {
            if (l != myLetter)
            {
                y = l.transform.position.y;
                break;
            }
        }
        x += (myLetter.transform.localScale.x * myWord.Count) / 2.0f;
        return new Vector3(x, y, myLetter.transform.position.z);
    }

    void OnCollisionEnter(Collision collision)
    {
        canBeDropped = false;
		if (letter.Equals("V") && collision.gameObject.name.Equals("Vine") && collision.relativeVelocity.y >= 10f) {
			Destroy (collision.gameObject);
		}
		if (collision.gameObject.GetComponent<ActionScript>()) {
			objectTouching = collision.gameObject;
			if (letter == "O" && (objectTouching.name.Equals("OPossumTree") || objectTouching.name.Equals("Log"))) {
				gameObject.renderer.material.SetTexture("_MainTex", (Texture)Resources.Load("O_Green"));
			}
		}
    }

    void OnCollisionExit(Collision collision)
    {
        canBeDropped = true;
		if (letter == "O" && (collision.gameObject.name.Equals("OPossumTree") || collision.gameObject.name.Equals("Log"))) {
			gameObject.renderer.material.SetTexture("_MainTex", (Texture)Resources.Load("O"));
		}
		if (objectTouching == collision.gameObject) {
			objectTouching = null;
		}
    }
}
