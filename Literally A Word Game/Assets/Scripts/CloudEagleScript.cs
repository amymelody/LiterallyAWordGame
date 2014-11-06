using UnityEngine;
using System.Collections;

public class CloudEagleScript : MonoBehaviour
{

	void Start() {
		if (RoomStateScript.cloudsCompleted) {
			Destroy(GameObject.Find("Cage"));
			Destroy (gameObject);
		}
	}

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag.Equals("Player"))
        {
			RoomStateScript.cloudsCompleted = true;
			gameObject.rigidbody.velocity = new Vector3(7.0f, 7.0f, 0f);
            Destroy(GameObject.Find("Cage"));
        }
    }
}
