using UnityEngine;
using System.Collections;

public class ActionScript : MonoBehaviour {
	
	public bool DoAction(GameObject obj) {

		LetterScript letterScript = (LetterScript)obj.GetComponent ("LetterScript");
		if (letterScript) {
			if (letterScript.letter.Equals("O")) {
				if (gameObject.name.Equals("OPossumTree")) {
					GameObject oPossum = (GameObject)Instantiate(Resources.Load("Prefabs/OPossum"));
					oPossum.transform.position = transform.position;
					audio.Play();
					return true;
				}
				if (gameObject.name.Equals("Log")) {
					Color textureColor = gameObject.renderer.material.color;
					textureColor.a = 0.5f;
					gameObject.renderer.material.color = textureColor;
					Destroy (GetComponent<BoxCollider>());
					Destroy (GetComponent<ActionScript>());
					return true;
				}
			}
		}

		return false;
	}
}
