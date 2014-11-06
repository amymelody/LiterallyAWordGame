using UnityEngine;
using System.Collections;

public class OptionScreenStateScript : MonoBehaviour
{

    public GUIStyle pressEnterStyle;
    public GUIStyle controlsStyle;
    public string level;
    string pressTab;
    string controls;

    // Use this for initialization
    void Start()
    {
        level = LevelTriggerScript.currentLevel;
        pressTab = "Press tab to return.";
        controls = "\u2194  -  Walk       Space  -  Jump\n" +
            "\u2195  -  Climb       C  -  Pickup/Drop Object\n" +
            "            Tab  -  Help Screen";
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Application.LoadLevel(level);
        }
    }

    void OnGUI()
    {
        GUI.BeginGroup(new Rect(0, 0, Screen.width, Screen.height));
        GUI.color = Color.black;
        GUI.Label(new Rect(0, Screen.height * 0.5f, Screen.width, Screen.height * 0.2f), pressTab, pressEnterStyle);
        GUI.Label(new Rect(Screen.width * 0.17f, Screen.height * 0.7f, Screen.width * 0.8f, Screen.height * 0.3f), controls, controlsStyle);
        GUI.EndGroup();
    }
}
