using UnityEngine;
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
	private bool canDrop;
	private bool touchingLadder;
	private bool onLadder;
	private bool onTopOfLadder;
	private bool facingRight;
	private bool onBalloons;
	private MovementDirection direction = MovementDirection.None;
	private int numObjectsTouching;
	private BoxCollider tCollider;
	private BoxCollider tTipCollider;

	private KeyCode leftKey;
	private KeyCode rightKey;
	private KeyCode jumpKey;
	private KeyCode climbUpKey;
	private KeyCode climbDownKey;
	private KeyCode pickUpKey;
	private KeyCode resetKey;

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
		resetKey = KeyCode.R;

		closeObjects = new ArrayList();
		closestObject = null;
		tCollider = null;
		tTipCollider = null;

		numObjectsTouching = 0;
		canJump = false;
		canDrop = true;
		onLadder = false;
		touchingLadder = false;
		onTopOfLadder = false;
		facingRight = true;
		onBalloons = false;
	}
	
	// Update is called once per frame
	void Update () {

		UpdateMetrics();

		if (Input.GetKeyDown (resetKey)) {
			Application.LoadLevel(Application.loadedLevelName);
		}
	
		if (!onLadder) {
			HandleMovement ();
			HandleJumping ();
			HandlePickUp ();
		}

		HighlightClosestObject ();

		HandleClimbing ();
		MovePickedUpObject ();
	}

	void UpdateMetrics() {
		string level = Application.loadedLevelName;
		GameObject metricsManager = GameObject.Find("MetricsManager");
		if (metricsManager) {
			if (level.Contains("mainRoom")) {
				if (!RoomStateScript.treeCreated && !RoomStateScript.mountainCreated) {
					metricsManager.GetComponent<MetricManagerScript>().timeInMainRoom += Time.deltaTime;
				}
			} else if (level.Contains("CloudsTransition")) {
				if (!RoomStateScript.accessedClouds) {
					metricsManager.GetComponent<MetricManagerScript>().timeInCloudsTransition += Time.deltaTime;
				}
			} else if (level.Contains("CloudLevel")) {
				if (!RoomStateScript.cloudsCompleted) {
					metricsManager.GetComponent<MetricManagerScript>().timeToCompleteCloudsLevel += Time.deltaTime;
				}
			} else if (level.Contains("ForestTransition")) {
				if (!RoomStateScript.accessedForest) {
					metricsManager.GetComponent<MetricManagerScript>().timeInForestTransition += Time.deltaTime;
				}
			} else if (level.Contains("ForestLevel")) {
				if (!RoomStateScript.forestCompleted) {
					metricsManager.GetComponent<MetricManagerScript>().timeToCompleteForestLevel += Time.deltaTime;
				}
			}
		}
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

			if (tCollider && tCollider.center.z != -0.5f) {
				tCollider.center -= new Vector3(0f, 0f, 1f);
			}
			if (tTipCollider && tTipCollider.center.z != -1.45f) {
				tTipCollider.center -= new Vector3(0f, 0f, 2.9f);
			}
			break;
		case MovementDirection.Right:
			facingRight = true;
			renderer.material.mainTexture = (Texture2D)Resources.Load("Textures/leeright");
			setXVelocity (movementSpeed);

			if (tCollider && tCollider.center.z != 0.5f) {
				tCollider.center += new Vector3(0f, 0f, 1f);
			}
			if (tTipCollider && tTipCollider.center.z != 1.45f) {
				tTipCollider.center += new Vector3(0f, 0f, 2.9f);
			}
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
                    if (!(closestObject.name.Contains("UP"))) {
                    closestObject.renderer.material.SetTexture("_MainTex", closestObject.GetComponent<LetterScript>().currentTex);
					}/*closestObject.transform.localScale = new Vector3(
						closestObject.transform.localScale.x / 1.2f,
						closestObject.transform.localScale.y / 1.2f,
						closestObject.transform.localScale.z / 1.2f);*/
					closestObject = null;

					if (pickedUpObject.rigidbody != null) {
						pickedUpObject.rigidbody.useGravity = false;
						pickedUpObject.GetComponent<LetterScript> ().PickedUp ();
						//If you pick up a "hook" T in forest level
						if (pickedUpObject.name.Equals("T")) {
							tCollider = gameObject.AddComponent<BoxCollider>();
							if (facingRight) {
								tCollider.center = new Vector3(-1.5f, 0.2f, 0.5f);
							} else {
								tCollider.center = new Vector3(-1.5f, 0.2f, -0.5f);
							}
							tCollider.size = new Vector3(1f, 0.1f, 1.8f);

							tTipCollider = gameObject.AddComponent<BoxCollider>();
							if (facingRight) {
								tTipCollider.center = new Vector3(-1.5f, 0.2f, 1.45f);
							} else {
								tTipCollider.center = new Vector3(-1.5f, 0.2f, -1.45f);
							}
							tTipCollider.size = new Vector3(1f, 0.1f, 0.05f);
						}
					}

					if (pickedUpObject.name.Contains ("UPBalloons")) {
						setYVelocity (movementSpeed);
						rigidbody.useGravity = false;
						onBalloons = true;
					}

				}
			}

			else if ((canJump || onBalloons) && canDrop) {
				//Do any special action when letting go of object
				LetterScript letterScript = (LetterScript)pickedUpObject.GetComponent ("LetterScript");
				if (letterScript) {
					GameObject objectTouching = letterScript.objectTouching;
					if (objectTouching) {
						ActionScript actionScript = (ActionScript)objectTouching.GetComponent("ActionScript");
						if (actionScript) {
							if (actionScript.DoAction(pickedUpObject)) {
								Destroy(pickedUpObject);
								pickedUpObject = null;
							}
						}
					}
				}

				if (pickedUpObject) {
					//Drop Object
					if (onBalloons) {
						onBalloons = false;
						setYVelocity (0);
						rigidbody.useGravity = true;
						//pickedUpObject.rigidbody.velocity = new Vector3 (0, movementSpeed, 0);
						pickedUpObject = null;
					}
					else {
						if (letterScript) {
							if (letterScript.canBeDropped) {
								dropSound.Play ();
								pickedUpObject.rigidbody.useGravity = true;
								pickedUpObject.rigidbody.velocity = -Vector3.up;
								pickedUpObject.GetComponent<LetterScript> ().Dropped ();
								if (pickedUpObject.name.Equals("T")) {
									Destroy (tCollider);
									tCollider = null;
									Destroy(tTipCollider);
									tTipCollider = null;
								} 

								pickedUpObject = null;
							}
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
				if (tempObj) {
                    if (!(tempObj.name.Contains("UP"))) {
                    tempObj.renderer.material.SetTexture("_MainTex", tempObj.GetComponent<LetterScript>().currentTex);
					}//tempObj.transform.localScale = new Vector3 (tempObj.transform.localScale.x / 1.2f, tempObj.transform.localScale.y / 1.2f, tempObj.transform.localScale.z / 1.2f);
				}
                if (closestObject) {
                    if (!(closestObject.name.Contains("UP"))) {
                    closestObject.renderer.material.SetTexture("_MainTex", closestObject.GetComponent<LetterScript>().selectTex);
					}//closestObject.transform.localScale = new Vector3 (closestObject.transform.localScale.x * 1.2f, closestObject.transform.localScale.y * 1.2f, closestObject.transform.localScale.z * 1.2f);
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
			float distance = Mathf.Abs(obj.transform.position.x - transform.position.x);
			if (distance < closestDistance) {
				closestDistance = distance;
				closestObject = obj;
			}
		}
		return closestObject;
	}

	RaycastHit hitObjectBelow(Vector3 startingVector, float horizontalOffset, float distance) {
		RaycastHit rayHit;
		Physics.Raycast(startingVector, -Vector3.up, out rayHit, distance);
		if (!rayHit.collider) {
			Physics.Raycast(startingVector + new Vector3(horizontalOffset, 0, 0), -Vector3.up, out rayHit, distance);
		}
		if (!rayHit.collider) {
			Physics.Raycast(startingVector - new Vector3(horizontalOffset, 0, 0), -Vector3.up, out rayHit, distance);
		}
		return rayHit;
	}
		
	void OnCollisionEnter(Collision collision) {
		if (collision.gameObject.tag.Equals("Floor")) {
			numObjectsTouching++;
			RaycastHit rayHit = hitObjectBelow(transform.position,
			                                   transform.localScale.x / 2.0f,
			                                   transform.localScale.y / 2.0f);
			if (rayHit.collider) {
				if (!rayHit.collider.gameObject.tag.Equals("Climbable")) {
					canJump = true;
					onLadder = false;
					rigidbody.useGravity = true;
				}
			}
		}

		if (!canJump && tCollider) {
			if (collision.contacts[0].thisCollider == tCollider ||
			    collision.contacts[collision.contacts.Length-1].thisCollider == tCollider) {
				canJump = true;
			}
		}

		if (collision.gameObject.name.Equals("CantDrop")) {
			canDrop = false;
		}
	}

	void OnCollisionExit(Collision collision) {
		if (collision.gameObject.tag.Equals("Floor")) {
			numObjectsTouching--;
			if (numObjectsTouching == 0) {
				canJump = false;
			}
		}

		if (collision.gameObject.name.Equals("CantDrop")) {
			canDrop = true;
		}
	}

	void OnTriggerEnter(Collider collider) {
		if (collider.gameObject.tag.Equals("Water")) {
			Physics.gravity *= 0.5f;
		}

		if (collider.gameObject.name.Equals("VTrigger")) {
			Destroy (collider.gameObject);
			Destroy (GameObject.Find ("VFloor"));
		}

		if (collider.gameObject.tag.Equals("Climbable")) {
			touchingLadder = true;
			currentLadder = collider.gameObject;
			RaycastHit rayHit = hitObjectBelow(transform.position - new Vector3(0, transform.localScale.y / 4.0f, 0),
			                                   transform.localScale.x / 2.0f,
			                                   transform.localScale.y / 4.0f);

			if (rayHit.collider && rayHit.collider.gameObject.tag.Equals("Climbable")) {
				setYVelocity(0);
				rigidbody.useGravity = false;
				canJump = true;
				onTopOfLadder = true;
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

			RaycastHit rayHit = hitObjectBelow(transform.position - new Vector3(0, transform.localScale.y / 4.0f, 0),
			                                   transform.localScale.x / 2.0f,
			                                   transform.localScale.y / 4.0f);

			if (rayHit.collider) {
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

	public void setYVelocity(float y) {
		transform.rigidbody.velocity = new Vector3(transform.rigidbody.velocity.x,y,0);
	}
}
