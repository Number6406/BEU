using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserMovement : MonoBehaviour {

	public float speed;             //Floating point variable to store the player's movement speed.
	public Vector2 jump_impulse;

	private Rigidbody2D rb2d;       //Store a reference to the Rigidbody2D component required to use 2D Physics.

	public float rayDistance = 0.5f;

	// Use this for initialization
	void Start()
	{
		//Get and store a reference to the Rigidbody2D component so that we can access it.
		rb2d = GetComponent<Rigidbody2D> ();
	}

	bool IsGrounded() {
		Debug.Log(Physics2D.Raycast(transform.position, -transform.up, rayDistance).transform.name);
		return false;
	}

	//FixedUpdate is called at a fixed interval and is independent of frame rate. Put physics code here.
	void FixedUpdate()
	{
		//Store the current horizontal input in the float moveHorizontal.
		float moveHorizontal = Input.GetAxis ("Horizontal");

		//Store the current vertical input in the float moveVertical.
		float moveVertical = Input.GetAxis ("Vertical");

		//Use the two store floats to create a new Vector2 variable movement.
		Vector2 movement = new Vector2 (moveHorizontal, moveVertical);

		//Call the AddForce function of our Rigidbody2D rb2d supplying movement multiplied by speed to move our player.
		//rb2d.AddForce (movement * speed);
		rb2d.velocity = new Vector2(moveHorizontal * speed, rb2d.velocity.y);

		Debug.DrawRay(transform.position, -transform.up * rayDistance, Color.yellow);
		if (Input.GetKeyUp(KeyCode.Space) && IsGrounded()){
			Debug.Log ("jump");
			rb2d.AddForce (jump_impulse, ForceMode2D.Impulse);
		} else if (Input.GetKeyUp(KeyCode.Space)) {
			Debug.Log ("can't jump");
		}

		//Debug.Log (rb2d.velocity);

	}

}
