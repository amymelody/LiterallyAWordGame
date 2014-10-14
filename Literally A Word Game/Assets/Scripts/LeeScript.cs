﻿using UnityEngine;
using System.Collections;

public class LeeScript : MonoBehaviour {

	enum MovementDirection {Left, Right, None};

	public float movementSpeed;
	public float jumpSpeed;
	public ArrayList closeObjects;

	private AudioSource jumpSound;
	private AudioSource dropSound;

	private GameObject pickedUpObject;
	private GameObject closestObject;
	private GameObject currentLadder;
	private bool canJump;
	private bool touchingLadder;
	private bool onLadder;
	private bool onTopOfLadder;
	private bool facingRight;
	private bool onBalloons;
	private MovementDirection direction = MovementDirection.None;
	private int numObjectsTouching;

	private KeyCode leftKey;
	private KeyCode rightKey;
	private KeyCode jumpKey;
	private KeyCode climbUpKey;
	private KeyCode climbDownKey;
	private KeyCode pickUpKey;

	// Use this for initialization
	void Start () {
		Component[] audioSources = GetComponents<AudioSource>();
		jumpSound = (AudioSource)audioSources[0];
		dropSound = (AudioSource)audioSources[1];

		leftKey = KeyCode.LeftArrow;
		rightKey = KeyCode.RightArrow;
		jumpKey = KeyCode.Space;
		climbUpKey = KeyCode.UpArrow;
		climbDownKey = KeyCode.DownArrow;
		pickUpKey = KeyCode.C;

		closeObjects = new ArrayList();
		closestObject = null;

		numObjectsTouching = 0;
		canJump = false;
		onLadder = false;
		touchingLadder = false;
		onTopOfLadder = false;
		facingRight = true;
		onBalloons = false;
	}
	
	// Update is called once per frame
	void Update () {

		if (!onLadder) {
			HandleMovement ();
			HandleJumping ();
			HandlePickUp ();
		}

		HighlightClosestObject ();

		HandleClimbing ();
		MovePickedUpObject ();
	}

	void HandleMovement ()
	{
		//Move based on direction
		switch (direction) {
		case MovementDirection.None:
			setXVelocity (0);
			break;
		case MovementDirection.Left:
			facingRight = false;
			renderer.material.mainTexture = (Texture2D)Resources.Load("Textures/leeleft");
			setXVelocity (-movementSpeed);
			break;
		case MovementDirection.Right:
			facingRight = true;
			renderer.material.mainTexture = (Texture2D)Resources.Load("Textures/leeright");
			setXVelocity (movementSpeed);
			break;
		}
		//Handle movement input
		if (Input.GetKeyDown (rightKey)) {
			direction = MovementDirection.Right;
		}
		if (Input.GetKeyDown (leftKey)) {
			direction = MovementDirection.Left;
		}
		if (Input.GetKeyUp (rightKey)) {
			if (Input.GetKey (leftKey)) {
				direction = MovementDirection.Left;
			}
			else {
				direction = MovementDirection.None;
			}
		}
		if (Input.GetKeyUp (leftKey)) {
			if (Input.GetKey (rightKey)) {
				direction = MovementDirection.Right;
			}
			else {
				direction = MovementDirection.None;
			}
		}
	}

	void HandleJumping ()
	{
		if (!onBalloons) {
			//If you're not in the air already, then jump
			if (canJump && Input.GetKeyDown (jumpKey)) {
				canJump = false;
				jumpSound.Play ();
				setYVelocity (jumpSpeed);
			}
			//If you release the jump key, start falling
			if (!canJump && Input.GetKeyUp (jumpKey) && rigidbody.velocity.y > 0) {
				setYVelocity (0);
			}
		}
	}

	void HandlePickUp ()
	{
		//Pick up & drop objects
		if (Input.GetKeyDown (pickUpKey)) {
			if (!pickedUpObject) {
				//Pick up object
				pickedUpObject = closestObject;
				if (pickedUpObject) {

					if (closestObject.name.Contains("Letter")) {
						closestObject.transform.localScale = new Vector3(
							closestObject.transform.localScale.x / 1.2f,
							closestObject.transform.localScale.y / 1.2f,
							closestObject.transform.localScale.z / 1.2f);
						closestObject = null;
					}

					if (pickedUpObject.rigidbody != null) {
						pickedUpObject.rigidbody.useGravity = false;
						pickedUpObject.GetComponent<LetterScript> ().PickedUp ();
					}
					if (pickedUpObject.name.Contains ("UPBalloons")) {
						setYVelocity (movementSpeed);
						rigidbody.useGravity = false;
						onBalloons = true;
					}
				}
			}
			else if (canJump || onBalloons) {
				//Drop Object
				if (onBalloons) {
					onBalloons = false;
					setYVelocity (0);
					rigidbody.useGravity = true;
					pickedUpObject.rigidbody.velocity = new Vector3 (0, movementSpeed, 0);
					pickedUpObject = null;
				}
				else {
					LetterScript letterScript = (LetterScript)pickedUpObject.GetComponent ("LetterScript");
					if (letterScript) {
						if (letterScript.canBeDropped) {
							dropSound.Play ();
							pickedUpObject.rigidbody.useGravity = true;
							pickedUpObject.rigidbody.velocity = -Vector3.up;
							pickedUpObject.GetComponent<LetterScript> ().Dropped ();
							pickedUpObject = null;
						}
					}
				}
			}
		}
	}

	void HighlightClosestObject ()
	{
		if (!pickedUpObject) {
			GameObject tempObj = closestObject;
			closestObject = GetClosestObject ();
			if (tempObj != closestObject) {
				if (tempObj && tempObj.name.Contains ("Letter")) {
					tempObj.transform.localScale = new Vector3 (tempObj.transform.localScale.x / 1.2f, tempObj.transform.localScale.y / 1.2f, tempObj.transform.localScale.z / 1.2f);
				}
				if (closestObject && closestObject.name.Contains ("Letter")) {
					closestObject.transform.localScale = new Vector3 (closestObject.transform.localScale.x * 1.2f, closestObject.transform.localScale.y * 1.2f, closestObject.transform.localScale.z * 1.2f);
				}
			}
		}
	}

	void HandleClimbing ()
	{
		//Handle climbing
		if (touchingLadder && !onLadder) {
			if (onTopOfLadder && Input.GetKeyDown (climbDownKey) || !onTopOfLadder && Input.GetKeyDown (climbUpKey)) {
				GetOnLadder ();
			}
		}
		if (onLadder) {
			if (Input.GetKeyUp (climbUpKey) || Input.GetKeyUp (climbDownKey)) {
				setYVelocity (0);
			}
			if (Input.GetKeyDown (climbUpKey)) {
				setYVelocity (movementSpeed);
			}
			if (Input.GetKeyDown (climbDownKey)) {
				setYVelocity (-movementSpeed);
			}
		}
	}

	void GetOnLadder ()
	{
		direction = MovementDirection.None;
		onLadder = true;
		rigidbody.useGravity = false;
		transform.position = new Vector3 (currentLadder.transform.position.x, transform.position.y, transform.position.z);
		setXVelocity (0);
		setYVelocity(0);
	}

	void MovePickedUpObject ()
	{
		//Move picked up object
		if (pickedUpObject) {
			float xOffset;
			float yOffset;
			if (onBalloons) {
				xOffset = transform.localScale.x / 2.0f;
				yOffset = pickedUpObject.transform.localScale.y / 2.0f;
			}
			else {
				xOffset = transform.localScale.x / 2.0f + pickedUpObject.transform.localScale.x / 2.0f;
				yOffset = 0;
			}
			if (facingRight) {
				pickedUpObject.transform.position = new Vector3 (transform.position.x + xOffset, transform.position.y + yOffset, pickedUpObject.transform.position.z);
			}
			else {
				pickedUpObject.transform.position = new Vector3 (transform.position.x - xOffset, transform.position.y + yOffset, pickedUpObject.transform.position.z);
			}
		}
	}

	public void RemoveObject(GameObject obj) {
		closeObjects.Remove(obj);
		if (closestObject == obj) {
			closestObject = null;
		}
	}

	GameObject GetClosestObject() {
		float closestDistance = 99999f;
		GameObject closestObject = null;
		foreach (GameObject obj in closeObjects) {
			float distance = obj.transform.position.x - transform.position.x;
			if (facingRight && distance >= 0 && distance < closestDistance) {
				closestDistance = distance;
				closestObject = obj;
			}
			if (!facingRight && distance <= 0 && -distance < closestDistance) {
				closestDistance = -distance;
				closestObject = obj;
			}
		}
		return closestObject;
	}

	void OnCollisionEnter(Collision collision) {
		if (collision.gameObject.tag.Equals("Floor")) {
			numObjectsTouching++;
			RaycastHit rayHit;
			if (Physics.Raycast(transform.position, 
			                    -Vector3.up, out rayHit, transform.localScale.y / 2.0f) ||
			    Physics.Raycast(transform.position + new Vector3(transform.localScale.x / 2.0f, 0, 0),
			                -Vector3.up, out rayHit, transform.localScale.y / 2.0f) ||
			    Physics.Raycast(transform.position - new Vector3(transform.localScale.x / 2.0f, 0, 0),
			                -Vector3.up, out rayHit, transform.localScale.y / 2.0f)) {
				if (!rayHit.collider.gameObject.tag.Equals("Climbable")) {
					canJump = true;
					onLadder = false;
					rigidbody.useGravity = true;
				}
			}
		}
	}

	void OnCollisionExit(Collision collision) {
		if (collision.gameObject.tag.Equals("Floor")) {
			numObjectsTouching--;
			if (numObjectsTouching == 0) {
				canJump = false;
			}
		}
	}

	void OnTriggerEnter(Collider collider) {
		if (collider.gameObject.tag.Equals("Water")) {
			Physics.gravity *= 0.5f;
		}

		if (collider.gameObject.tag.Equals("Climbable")) {
			touchingLadder = true;
			currentLadder = collider.gameObject;
			RaycastHit rayHit;
			if (Physics.Raycast(transform.position - new Vector3(0, transform.localScale.y / 4.0f, 0), 
			                    -Vector3.up, out rayHit, transform.localScale.y / 4.0f) ||
			    Physics.Raycast(transform.position + new Vector3(transform.localScale.x / 2.0f, -transform.localScale.y / 4.0f, 0),
			                -Vector3.up, out rayHit, transform.localScale.y / 4.0f) ||
			    Physics.Raycast(transform.position - new Vector3(transform.localScale.x / 2.0f, transform.localScale.y / 4.0f, 0),
			                -Vector3.up, out rayHit, transform.localScale.y / 4.0f)) {
				if (rayHit.collider.gameObject.tag.Equals("Climbable")) {
					setYVelocity(0);
					rigidbody.useGravity = false;
					canJump = true;
					onTopOfLadder = true;
				} 
			}
		}

		if (collider.gameObject.tag.Equals("Letter")) {
			closeObjects.Add(collider.gameObject);
		}
	}

	void OnTriggerExit(Collider collider) {
		if (collider.gameObject.tag.Equals("Water")) {
			Physics.gravity *= 2f;
		}

		if (collider.gameObject.tag.Equals("Climbable")) {
			RaycastHit rayHit;
			if (Physics.Raycast(transform.position - new Vector3(0, transform.localScale.y / 4.0f, 0), 
			                -Vector3.up, out rayHit, transform.localScale.y / 4.0f) ||
			    Physics.Raycast(transform.position + new Vector3(transform.localScale.x / 2.0f, -transform.localScale.y / 4.0f, 0),
			                -Vector3.up, out rayHit, transform.localScale.y / 4.0f) ||
			    Physics.Raycast(transform.position - new Vector3(transform.localScale.x / 2.0f, transform.localScale.y / 4.0f, 0),
			                -Vector3.up, out rayHit, transform.localScale.y / 4.0f)) {
				if (!rayHit.collider.gameObject.tag.Equals("Climbable")) {
					currentLadder = null;
					onTopOfLadder = false;
					touchingLadder = false;
					onLadder = false;
					if (!pickedUpObject || !pickedUpObject.name.Contains("UPBalloons")) {
						rigidbody.useGravity = true;
					}
				}
			} else {
				currentLadder = null;
				onTopOfLadder = false;
				touchingLadder = false;
				onLadder = false;
				if (!pickedUpObject || !pickedUpObject.name.Contains("UPBalloons")) {
					rigidbody.useGravity = true;
				}
			}
		}

		if (collider.gameObject.tag.Equals("Letter")) {
			closeObjects.Remove(collider.gameObject);
		}
	}

	void setXVelocity(float x) {
		transform.rigidbody.velocity = new Vector3(x,transform.rigidbody.velocity.y,0);
	}

	void setYVelocity(float y) {
		transform.rigidbody.velocity = new Vector3(transform.rigidbody.velocity.x,y,0);
	}
}
