using UnityEngine;
using System.Collections;

public class BScript : MonoBehaviour
{
    bool texChanged;

    void Start()
    {
        texChanged = false;
    }

	void Update ()
    {
	    if(!texChanged)
        {
            LetterScript ls = gameObject.GetComponent<LetterScript>();
            ls.currentTex = (Texture)Resources.Load("B_Gold");
            gameObject.renderer.material.SetTexture("_MainTex", ls.currentTex);
            texChanged = true;
        }
	}
}
