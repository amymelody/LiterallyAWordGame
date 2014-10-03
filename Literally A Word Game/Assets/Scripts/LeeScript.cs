using UnityEngine;
using System.Collections;

public class LeeScript : MonoBehaviour {

	enum MovementDirection {Left, Right, None};

	public float movementSpeed;
	public float jumpSpeed;

	private ArrayList closeObjects;
	private GameObject pickedUpObject;
	private GameObject currentLadder;
	private bool canJump;
	private bool touchingLadder;
	private bool onLadder;
	private bool onTopOfLadder;
	private bool facingRight;
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
		closeObjects = new ArrayList();
		numObjectsTouching = 0;
		canJump = false;
		onLadder = false;
		touchingLadder = false;
		onTopOfLadder = false;
		facingRight = true;
		leftKey = KeyCode.LeftArrow;
		rightKey = KeyCode.RightArrow;
		jumpKey = KeyCode.Space;
		climbUpKey = KeyCode.UpArrow;
		climbDownKey = KeyCode.DownArrow;
		pickUpKey = KeyCode.C;
	}
	
	// Update is called once per frame
	void Update () {

		if (!onLadder) {

			//Handle left and right movement
			switch (direction) {
			case MovementDirection.None:
				setXVelocity(0);
				break;
			case MovementDirection.Left:
				facingRight = false;
				setXVelocity(-movementSpeed);
				break;
			case MovementDirection.Right:
				facingRight = true;
				setXVelocity(movementSpeed);
				break;
			}

			if (Input.GetKeyDown(rightKey)) {
				direction = MovementDirection.Right;
			}
			if (Input.GetKeyDown(leftKey)) {
				direction = MovementDirection.Left;
			}
			if (Input.GetKeyUp(rightKey)) {
				if (Input.GetKey(leftKey)) {
					direction = MovementDirection.Left;
				} else {
					direction = MovementDirection.None;
				}
			}
			if (Input.GetKeyUp(leftKey)) {
				if (Input.GetKey(rightKey)) {
					direction = MovementDirection.Right;
				} else {
					direction = MovementDirection.None;
				}
			}

			//Handle jumping
			if (canJump && Input.GetKeyDown(jumpKey)) {
				canJump = false;
				setYVelocity(jumpSpeed);
			}

			//Pick up & drop objects
			if (Input.GetKeyDown (pickUpKey)) {
				if (!pickedUpObject) {
					pickedUpObject = GetClosestObject();
					if (pickedUpObject) {
						pickedUpObject.rigidbody.useGravity = false;
					}
				} else if (canJump) {
					LetterScript letterScript = (LetterScript)pickedUpObject.GetComponent("LetterScript");
					if (letterScript) {
						if (letterScript.canBeDropped) {
							pickedUpObject.rigidbody.useGravity = true;
							pickedUpObject.rigidbody.velocity = -Vector3.up;
							pickedUpObject = null;
						}
					}
				}
			}
		}

		//Handle climbing
		if (touchingLadder && !onLadder) {
			if (onTopOfLadder && Input.GetKeyDown(climbDownKey) ||
			    !onTopOfLadder && Input.GetKeyDown (climbUpKey)) {
				onLadder = true;
				rigidbody.useGravity = false;
				transform.position = new Vector3(currentLadder.transform.position.x, 
				                                 transform.position.y,
				                                 transform.position.z);
			}
		}

		if (onLadder) {
			if (Input.GetKeyUp (climbUpKey) || Input.GetKeyUp (climbDownKey)) {
				setYVelocity(0);
			}
			if (Input.GetKeyDown (climbUpKey)) {
				setYVelocity (movementSpeed);
			}
			if (Input.GetKeyDown (climbDownKey)) {
				setYVelocity (-movementSpeed);
			}
		}

		//Move picked up object
		if (pickedUpObject) {
			if (facingRight) {
				pickedUpObject.transform.position = 
					new Vector3(transform.position.x + transform.localScale.x / 2.0f +
					            pickedUpObject.transform.localScale.x / 2.0f,
					            transform.position.y,
					            pickedUpObject.transform.position.z);
			} else {
				pickedUpObject.transform.position = 
					new Vector3(transform.position.x - transform.localScale.x / 2.0f -
					            pickedUpObject.transform.localScale.x / 2.0f,
					            transform.position.y,
					            pickedUpObject.transform.position.z);
			}
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
			if (Physics.Raycast(transform.position, -Vector3.up, transform.localScale.y / 2.0f) ||
			    Physics.Raycast(transform.position + new Vector3(transform.localScale.x / 2.0f, 0, 0),
			                -Vector3.up, transform.localScale.y / 2.0f) ||
			    Physics.Raycast(transform.position - new Vector3(transform.localScale.x / 2.0f, 0, 0),
			                -Vector3.up, transform.localScale.y / 2.0f)) {
				canJump = true;
				onLadder = false;
				rigidbody.useGravity = true;
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
		if (collider.gameObject.tag.Equals("Climbable")) {
			touchingLadder = true;
			currentLadder = collider.gameObject;
			RaycastHit rayHit;
			if (Physics.Raycast(transform.position, -Vector3.up, out rayHit, transform.localScale.y / 2.0f)) {
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
		if (collider.gameObject.tag.Equals("Climbable")) {
			currentLadder = null;
			onTopOfLadder = false;
			touchingLadder = false;
			onLadder = false;
			rigidbody.useGravity = true;
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
