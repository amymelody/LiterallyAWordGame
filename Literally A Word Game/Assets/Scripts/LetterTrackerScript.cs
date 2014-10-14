﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LetterTrackerScript : MonoBehaviour
{
	public GUIStyle scoreStyle;

    private GameObject[] letters;
    private AudioSource wordSound;
    private int score;
    private List<string> startWords;

    //Should replace with a "enter new area" function
	void Start ()
    {
        letters = GameObject.FindGameObjectsWithTag("Letter");
        Component[] audioSources = GetComponents<AudioSource>();
        wordSound = (AudioSource)audioSources[2];
        score = 0;

        startWords = new List<string>();
        string[] wordsInput = { "ARE", "ART", "AT", "ATE", "AWE", 
                              "EAR", "EAT", "EATER", "ERE", "EWE", 
                              "RAT", "RATE", "RAW", 
                              "TAR", "TEA", "TEAR", "TEE", 
                              "WAR", "WE", "WEAR", "WERE", "WET" };
        startWords.AddRange(wordsInput);
        //Will be lists for each level
	}

	void Update ()
    {
	
	}

    public GameObject[] GetLetterArray()
    {
        return letters;
    }

    public void SetLetterArray()
    {
        letters = GameObject.FindGameObjectsWithTag("Letter");
    }

    public void WordFind(string word, Vector3 wordCenterPosition)
    {
        switch (word)
        {
            case "TREE":
			    if (!RoomStateScript.treeCreated)
                {
                    wordSound.Play();
				    RoomStateScript.treeCreated = true;
				    GameObject tree = (GameObject)Instantiate(Resources.Load("Prefabs/Tree"));
				    tree.transform.position = new Vector3(-4.268143f, 6.929121f, 1.886556f);
				    tree.transform.localEulerAngles = new Vector3(0f, 180f, 0f);

				    GameObject treeLadder = (GameObject)Instantiate(Resources.Load("Prefabs/TreeLadder"));
					treeLadder.transform.position = new Vector3(-4.268143f, 6.929121f, 0.2479973f);
			    }
                break;
            case "WATER":
				GameObject waterfallFloor = GameObject.Find ("WaterfallFloor");
                if (waterfallFloor)
                {
                    wordSound.Play();
					GameObject.Destroy(waterfallFloor);
					GameObject waterfall = (GameObject)Instantiate(Resources.Load ("Prefabs/Waterfall"));
					waterfall.transform.position = new Vector3(2.100769f, -0.5f, 0.2479973f);
					waterfall.transform.localEulerAngles = new Vector3(0f, 180f, 0f);
				}
				break;
            case "UP":
                wordSound.Play();
			    if (!GameObject.Find("UPBalloons(Clone)"))
                {
				    GameObject upBalloons = (GameObject)Instantiate(Resources.Load("Prefabs/UPBalloons"));
				    upBalloons.transform.position = new Vector3(wordCenterPosition.x,
				                                                wordCenterPosition.y + upBalloons.transform.localScale.y / 2.0f,
				                                                wordCenterPosition.z);
				    upBalloons.transform.localEulerAngles = new Vector3(0f, 180f, 0f);
                    GameObject letter1 = null;
                    GameObject letter2 = null;
                    foreach(GameObject l in letters)
                    {
                        if (wordCenterPosition.y <= l.transform.position.y && wordCenterPosition.y + 1 > l.transform.position.y)
                        {
                            float letterX = l.transform.position.x;
                            if (wordCenterPosition.x - 1 < letterX && wordCenterPosition.y + 1 > letterX)
                            {
                                if (letter1 == null) { letter1 = l; }
                                else
                                {
                                    letter2 = l;
                                    break;
                                }
                            }
                        }
                    }
				    GameObject.Destroy(letter1);
				    GameObject.Destroy(letter2);
			    }
			    break;
            case "DROP":
                wordSound.Play();
                //How to check level? Should only happen in "CloudLevel"
                GameObject[] floors = GameObject.FindGameObjectsWithTag("Floor");
                foreach(GameObject f in floors)
                {
                    float floorY = f.transform.position.y;
                    if (wordCenterPosition.y > floorY && wordCenterPosition.y - 1f < floorY)        //If f is the platform level that the word is on
                    {
                        float floorX = f.transform.position.x;
                        float floorSizeX = f.transform.lossyScale.x;
                        float floorFrontX = floorX - floorSizeX / 2f;
                        float floorBackX = floorX + floorSizeX / 2f;
                        if (wordCenterPosition.x > floorFrontX && wordCenterPosition.x < floorBackX)
                        {
                            //Find the two sections of floor on either side of the word
                            float wordSizeX = Resources.Load<GameObject>("Letter").transform.lossyScale.x * 4f;
                            float wordFrontX = wordCenterPosition.x - wordSizeX / 2f;
                            float wordBackX = wordCenterPosition.x + wordSizeX / 2f;
                            float floor1SizeX = floorFrontX - wordFrontX;
                            float floor2SizeX = wordBackX - floorBackX;
                            if (floor1SizeX > 0.5)
                            {
                                GameObject floor1 = (GameObject)Instantiate(Resources.Load("Prefabs/Floor"));
                                floor1.transform.localScale = new Vector3(floor1SizeX, floor1.transform.lossyScale.y, floor1.transform.lossyScale.z);
                                floor1.transform.position = new Vector3(floor1SizeX / 2f, floor1.transform.position.y, floor1.transform.position.z);
                            }
                            if (floor2SizeX > 0.5)
                            {
                                GameObject floor2 = (GameObject)Instantiate(Resources.Load("Prefabs/Floor"));
                                floor2.transform.localScale = new Vector3(floor2SizeX, floor2.transform.lossyScale.y, floor2.transform.lossyScale.z);
                                floor2.transform.position = new Vector3(floor2SizeX / 2f, floor2.transform.position.y, floor2.transform.position.z);
                            }
                            Destroy(f);
                            break;
                        }
                    }
                }
                break;
	        default:
                ExtraWordFind(word, wordCenterPosition);
	            break;
        }
    }

    //For words that don't pertain to gameplay
    public void ExtraWordFind(string word, Vector3 wordCenterPosition)
    {
        foreach(string w in startWords)
        {
            if(word == w)
            {
                score++;
                wordSound.Play();
                startWords.Remove(w);
                break;
            }
        }
    }

    void OnGUI()
    {
        GUI.contentColor = Color.black;
        GUI.Label(new Rect(330, 40, 100, 50), "Score: " + score.ToString(), scoreStyle);
    }
}
