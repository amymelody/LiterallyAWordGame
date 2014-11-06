using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {

	public GameObject followObject;
	public GameObject levelBounds;

	private float cameraWidth;
	private float cameraHeight;
	private float levelBottomBound;
	private float levelTopBound;
	private float levelRightBound;
	private float levelLeftBound;

	// Use this for initialization
	void Start () {
		cameraWidth = camera.aspect * 2f * camera.orthographicSize;
		cameraHeight = 2f * camera.orthographicSize;

		levelRightBound = levelBounds.transform.position.x + levelBounds.transform.localScale.x / 2.0f;
		levelLeftBound = levelBounds.transform.position.x - levelBounds.transform.localScale.x / 2.0f;
		levelTopBound = levelBounds.transform.position.y + levelBounds.transform.localScale.y / 2.0f;
		levelBottomBound = transform.position.y - cameraHeight / 2.0f;
	}
	
	// Update is called once per frame
	void Update () {
		float rightBound = followObject.transform.position.x + cameraWidth / 2.0f;
		float leftBound = followObject.transform.position.x - cameraWidth / 2.0f;
		if (rightBound <= levelRightBound && leftBound >= levelLeftBound) {
			SetXPosition(followObject.transform.position.x);
		}

		float topBound = followObject.transform.position.y + cameraHeight / 2.0f;
		float bottomBound = followObject.transform.position.y - cameraHeight / 2.0f;
		if (topBound <= levelTopBound && bottomBound >= levelBottomBound) {
			SetYPosition(followObject.transform.position.y);
		}
	}

	void SetXPosition(float xVal) {
		transform.position = new Vector3(xVal, transform.position.y, transform.position.z);
	}

	void SetYPosition(float yVal) {
		transform.position = new Vector3(transform.position.x, yVal, transform.position.z);
	}
}
