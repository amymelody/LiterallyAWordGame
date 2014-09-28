using UnityEngine;
using System.Collections;

public class Lee : MonoBehaviour {

	enum MovementDirection {Left, Right, None};

	public float movementSpeed;
	private bool canJump = false;
	private bool onLadder = false;
	private MovementDirection direction = MovementDirection.None;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		//Handle left and right movement
		switch (direction) {
			case MovementDirection.None:
				setXVelocity(0);
				break;
			case MovementDirection.Left:
				setXVelocity(-movementSpeed);
				break;
			case MovementDirection.Right:
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

		//Handle jumping & climbing
		if (onLadder) {
			if (Input.GetKeyUp (KeyCode.W) || Input.GetKeyUp (KeyCode.S)) {
				setYVelocity(0);
			}
			if (Input.GetKeyDown (KeyCode.W)) {
				setYVelocity (5);
			}
			if (Input.GetKeyDown (KeyCode.S)) {
				setYVelocity (-5);
			}
		} else {
			if (canJump && Input.GetKeyDown(KeyCode.W)) {
				canJump = false;
				setYVelocity(10f);
			}
		}
	}

	void OnCollisionEnter(Collision collision) {
		if (collision.gameObject.tag.Equals("Floor")) {

			RaycastHit myRayHit;

			if (Physics.Raycast(transform.position, -Vector3.up, out myRayHit, 0.5f)) {
				canJump = true;
			}

		}
	}

	void OnTriggerEnter(Collider collider) {

		if (collider.gameObject.tag.Equals("Climbable")) {
			onLadder = true;
			rigidbody.useGravity = false;
			setYVelocity(0);
		}
	}

	void OnTriggerExit(Collider collider) {
		if (collider.gameObject.tag.Equals("Climbable")) {
			onLadder = false;
			rigidbody.useGravity = true;
		}
	}

	void setXVelocity(float x) {
		transform.rigidbody.velocity = new Vector3(x,transform.rigidbody.velocity.y,0);
	}

	void setYVelocity(float y) {
		transform.rigidbody.velocity = new Vector3(transform.rigidbody.velocity.x,y,0);
	}
}
