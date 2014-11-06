using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LetterTrackerScript : MonoBehaviour
{
    public GUIStyle scoreStyle;

    private GameObject[] letters;
    private AudioSource wordSound;
    static private int score = 0;
    private List<string> startWords;

    //Should replace with a "enter new area" function
    void Start()
    {
        letters = GameObject.FindGameObjectsWithTag("Letter");
        Component[] audioSources = GetComponents<AudioSource>();
        wordSound = (AudioSource)audioSources[2];

        startWords = new List<string>();
		string[] wordsInput = { "EATER", "IRATE", "RATE", "RITE", "TARE",
			"TEAR", "TIER", "TIRE", "AIR", "ARE", "ART", "ATE", "EAR", "EAT",
			"ERA", "ERE", "IRE", "RAT", "TAR", "TEA", "TEE", "TIE", "AT", "IT" };
        startWords.AddRange(wordsInput);
        //Will be lists for each level
    }

    void Update()
    {

    }

    public void WordFind(LetterScript refScript, string word, Vector3 wordCenterPosition)
    {
		string level = Application.loadedLevelName;
        switch (word)
        {
            case "TREE":
                if (!RoomStateScript.treeCreated && level.Contains("mainRoom"))
                {
                    wordSound.Play();
                    refScript.ColorHint("");
                    RoomStateScript.treeCreated = true;
                    RoomStateScript.CreateTree();
                }
                break;
            case "WATER":
                GameObject waterfallFloor = GameObject.Find("WaterfallFloor");
                if (waterfallFloor && !RoomStateScript.waterfallCreated && level.Contains("mainRoom"))
                {
                    wordSound.Play();
                    refScript.ColorHint("");
                    RoomStateScript.waterfallCreated = true;
                    RoomStateScript.CreateWaterfall();
                }
                break;
			case "AERIE":
				if (!RoomStateScript.mountainCreated && level.Contains("mainRoom"))
				{
					wordSound.Play();
					refScript.ColorHint("");
					RoomStateScript.mountainCreated = true;
					RoomStateScript.CreateMountain();
				}
				break;
			case "UP":
				wordSound.Play();
				if (level.Contains("Cloud"))
				{
					GameObject upBalloons = (GameObject)Instantiate(Resources.Load("Prefabs/UPBalloons"));
                    upBalloons.transform.position = new Vector3(wordCenterPosition.x,
                                                                wordCenterPosition.y + upBalloons.transform.localScale.y * 3f / 4f,
                                                                wordCenterPosition.z);
                    upBalloons.transform.localEulerAngles = new Vector3(0f, 180f, 0f);
                    GameObject letter1 = null;
                    GameObject letter2 = null;
                    foreach (GameObject l in letters)
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
                if (level.Contains("Cloud"))
                {
                    GameObject[] floors = GameObject.FindGameObjectsWithTag("Floor");
                    foreach (GameObject f in floors)
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
                                float wordSizeX = Resources.Load<GameObject>("Prefabs/Letter").transform.lossyScale.x * 4f;
                                float wordFrontX = wordCenterPosition.x - wordSizeX / 2f;
                                float wordBackX = wordCenterPosition.x + wordSizeX / 2f;
                                float floor1SizeX = Mathf.Abs(floorFrontX - wordFrontX);
                                float floor2SizeX = Mathf.Abs(wordBackX - floorBackX);
                                if (floor1SizeX > 0.5)
                                {
                                    Vector3 pos = new Vector3(floorFrontX + floor1SizeX / 2f, f.transform.position.y, f.transform.position.z);
                                    GameObject floor1 = (GameObject)Instantiate(Resources.Load<GameObject>("Prefabs/Floor"), pos, Quaternion.identity);
                                    floor1.transform.localScale = new Vector3(floor1SizeX, f.transform.lossyScale.y, f.transform.lossyScale.z);
                                }
                                if (floor2SizeX > 0.5)
                                {
                                    Vector3 pos = new Vector3(wordBackX + floor2SizeX / 2f, f.transform.position.y, f.transform.position.z);
                                    GameObject floor2 = (GameObject)Instantiate(Resources.Load<GameObject>("Prefabs/Floor"), pos, Quaternion.identity);
                                    floor2.transform.localScale = new Vector3(floor2SizeX, f.transform.lossyScale.y, f.transform.lossyScale.z);
                                }
                                Destroy(f);
                                //Unlink the word
                                foreach(GameObject l in refScript.word)
                                {
                                    LetterScript ls = l.GetComponent<LetterScript>();
                                    if (ls != refScript)
                                    {
                                        ls.word.Clear();
                                    }
                                }
                                refScript.word.Clear();
                                break;
                            }
                        }
                    }
                }
                break;
            case "RISE":
                wordSound.Play();
                if (level.Contains("Cloud"))
                {
                    GameObject[] floors = GameObject.FindGameObjectsWithTag("Floor");
                    foreach (GameObject f in floors)
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
                                float wordSizeX = Resources.Load<GameObject>("Prefabs/Letter").transform.lossyScale.x * 4f;
                                float wordFrontX = wordCenterPosition.x - wordSizeX / 2f;
                                float wordBackX = wordCenterPosition.x + wordSizeX / 2f;
                                float floor1SizeX = Mathf.Abs(floorFrontX - wordFrontX);
                                float floor2SizeX = Mathf.Abs(wordBackX - floorBackX);
                                if (floor1SizeX > 0.5)
                                {
                                    Vector3 pos = new Vector3(floorFrontX + floor1SizeX / 2f, f.transform.position.y, f.transform.position.z);
                                    GameObject floor1 = (GameObject)Instantiate(Resources.Load<GameObject>("Prefabs/Floor"), pos, Quaternion.identity);
                                    floor1.transform.localScale = new Vector3(floor1SizeX, f.transform.lossyScale.y, f.transform.lossyScale.z);
                                }
                                if (floor2SizeX > 0.5)
                                {
                                    Vector3 pos = new Vector3(wordBackX + floor2SizeX / 2f, f.transform.position.y, f.transform.position.z);
                                    GameObject floor2 = (GameObject)Instantiate(Resources.Load<GameObject>("Prefabs/Floor"), pos, Quaternion.identity);
                                    floor2.transform.localScale = new Vector3(floor2SizeX, f.transform.lossyScale.y, f.transform.lossyScale.z);
                                }
                                //RISE Platform
                                Vector3 posR = new Vector3(wordCenterPosition.x, f.transform.position.y, f.transform.position.z);
                                GameObject floorR = (GameObject)Instantiate(Resources.Load<GameObject>("Prefabs/Floor"), posR, Quaternion.identity);
                                floorR.transform.localScale = new Vector3(wordSizeX, f.transform.lossyScale.y, f.transform.lossyScale.z);
                                floorR.AddComponent<RiseScript>();
                                Destroy(f);
                                break;
                            }
                        }
                    }
                }
                break;
            case "BRIDGE":
                wordSound.Play();
                if (level.Contains("Cloud"))
                {
                    refScript.ColorHint("");
                    GameObject[] floors = GameObject.FindGameObjectsWithTag("Floor");
                    foreach (GameObject f in floors)
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
                                //Create Bridge
                                Vector3 pos = new Vector3(floorBackX + 15f / 2f, f.transform.position.y, f.transform.position.z);
                                GameObject bridge = (GameObject)Instantiate(Resources.Load<GameObject>("Prefabs/Floor"), pos, Quaternion.identity);
                                bridge.transform.localScale = new Vector3(15, f.transform.lossyScale.y, f.transform.lossyScale.z);
                                break;
                            }
                        }
                    }
                }
                break;
            default:
                ExtraWordFind(refScript, word, wordCenterPosition);
                break;
        }
    }

    //For words that don't pertain to gameplay
    public void ExtraWordFind(LetterScript refScript, string word, Vector3 wordCenterPosition)
    {
		string level = Application.loadedLevelName;
        if (level.Contains("mainRoom"))
        {
            if (!RoomStateScript.treeCreated)
            {
                if (word == "TR" || word == "TRE")
                {
                    refScript.ColorHint("TREE");
                    return;
                }
            }
            if (!RoomStateScript.mountainCreated)
            {
                if (word == "AE" || word == "AER" || word == "AERI")
                {
                    refScript.ColorHint("AERIE");
                    return;
                }
            }
            GameObject waterfallFloor = GameObject.Find("WaterfallFloor");
            if (waterfallFloor && !RoomStateScript.waterfallCreated)
            {
                if (word == "WA" || word == "WAT" || word == "WATE")
                {
                    refScript.ColorHint("WATER");
                    return;
                }
            }

            foreach (string w in startWords)
            {
                if (word == w)
                {
                    score++;
                    wordSound.Play();
                    startWords.Remove(w);
                    return;
                }
            }
        }
        if (level == "CloudLevel")
        {
            if (word == "BR" || word == "BRI" || word == "BRID" || word == "BRIDG")
            {
                refScript.ColorHint("BRIDGE");
                return;
            }
        }
    }

    void OnGUI()
    {
        GUI.contentColor = Color.black;
		GUI.Label(new Rect(Screen.width * 0.75f, Screen.height * 0.1f, Screen.width * 0.25f, Screen.height * 0.1f), "Score: " + score.ToString(), scoreStyle);
		GUI.Label(new Rect(Screen.width * 0.75f, Screen.height * 0.9f, Screen.width * 0.25f, Screen.height * 0.1f), "(R)eset Level", scoreStyle);
    }
}
