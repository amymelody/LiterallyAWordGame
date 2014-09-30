using UnityEngine;
using System.Collections;

public class Lee : MonoBehaviour {

	enum MovementDirection {Left, Right, None};

	public float movementSpeed;

	private ArrayList closeObjects;
	private GameObject pickedUpObject;
	private bool canJump = false;
	private bool onClimbable = false;
	private bool facingRight = true;
	private MovementDirection direction = MovementDirection.None;

	// Use this for initialization
	void Start () {
		closeObjects = new ArrayList();
	}
	
	// Update is called once per frame
	void Update () {
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

		if (Input.GetKeyDown(KeyCode.D)) {
			direction = MovementDirection.Right;
		}
		if (Input.GetKeyDown(KeyCode.A)) {
			direction = MovementDirection.Left;
		}
		if (Input.GetKeyUp(KeyCode.D)) {
			if (Input.GetKey(KeyCode.A)) {
				direction = MovementDirection.Left;
			} else {
				direction = MovementDirection.None;
			}
		}
		if (Input.GetKeyUp(KeyCode.A)) {
			if (Input.GetKey(KeyCode.D)) {
				direction = MovementDirection.Right;
			} else {
				direction = MovementDirection.None;
			}
		}

		//Handle climbing
		if (onClimbable) {
			if (Input.GetKeyUp (KeyCode.W) || Input.GetKeyUp (KeyCode.S)) {
				setYVelocity(0);
			}
			if (Input.GetKeyDown (KeyCode.W)) {
				setYVelocity (5);
			}
			if (Input.GetKeyDown (KeyCode.S)) {
				setYVelocity (-5);
			}
		//Handle jumping
		} else {
			if (canJump && Input.GetKeyDown(KeyCode.W)) {
				canJump = false;
				setYVelocity(10f);
			}
		}

		//Pick up & drop objects
		if (Input.GetKeyDown (KeyCode.E)) {
			if (!pickedUpObject) {
				pickedUpObject = GetClosestObject();
			} else if (canJump) {
				pickedUpObject = null;
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
			if (Physics.Raycast(transform.position, -Vector3.up, transform.localScale.y / 2.0f) ||
			    Physics.Raycast(transform.position + new Vector3(transform.localScale.x / 2.0f, 0, 0),
			                -Vector3.up, transform.localScale.y / 2.0f) ||
			    Physics.Raycast(transform.position - new Vector3(transform.localScale.x / 2.0f, 0, 0),
			                -Vector3.up, transform.localScale.y / 2.0f)) {
				canJump = true;
			}
		}
	}

	void OnCollisionExit(Collision collision) {
		if (collision.gameObject.tag.Equals("Floor")) {
				canJump = false;
		}
	}

	void OnTriggerEnter(Collider collider) {
		if (collider.gameObject.tag.Equals("Climbable")) {
			onClimbable = true;
			rigidbody.useGravity = false;
			setYVelocity(0);
		}

		if (collider.gameObject.tag.Equals("CanPickUp")) {
			closeObjects.Add(collider.gameObject);
		}
	}

	void OnTriggerExit(Collider collider) {
		if (collider.gameObject.tag.Equals("Climbable")) {
			onClimbable = false;
			rigidbody.useGravity = true;
		}

		if (collider.gameObject.tag.Equals("CanPickUp")) {
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
