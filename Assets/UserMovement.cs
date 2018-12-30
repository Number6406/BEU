using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserMovement : MonoBehaviour {

	public float speed;             //Floating point variable to store the player's movement speed.

	private Vector2 jump_impulse;
	public float jump_impulse_min;
	public float jump_impulse_max;
	public float jump_timer;
	public float jump_timer_max;

	private Animator anim;

	public PlayerState state;

	private Rigidbody2D rb2d;       //Store a reference to the Rigidbody2D component required to use 2D Physics.

	public float rayDistance = 0.5f;
	private int layerMask;

	// Use this for initialization
	void Start()
	{
		//Get and store a reference to the Rigidbody2D component so that we can access it.
		rb2d = GetComponent<Rigidbody2D> ();
		// Bit shift the index of the layer (8) to get a bit mask
		layerMask = 1 << 10;
		state = PlayerState.IDLE;
		anim = GetComponent<Animator> ();
	}

	bool IsGrounded() 
	{
		//Debug.Log (Physics2D.Raycast(transform.position, -transform.up, rayDistance, layerMask));
		return Physics2D.Raycast(transform.position, -transform.up, rayDistance, layerMask);
	}

	void Update() 
	{
		//Store the current horizontal input in the float moveHorizontal.
		float moveHorizontal = Input.GetAxis ("Horizontal");
		//Store the current vertical input in the float moveVertical.
		float moveVertical = Input.GetAxis ("Vertical");

		switch (state) {
		case PlayerState.IDLE:
			if (Input.GetKey (KeyCode.Space) && IsGrounded ()) {
				jump_timer += Time.deltaTime;
				state = PlayerState.JUMP_CHARGE;
			}
			if (moveHorizontal != 0) {
				state = PlayerState.RUN;
			}
			break;

		case PlayerState.RUN:
			//Use the two store floats to create a new Vector2 variable movement.
			Vector2 movement = new Vector2 (moveHorizontal, moveVertical);

			//Call the AddForce function of our Rigidbody2D rb2d supplying movement multiplied by speed to move our player.
			//rb2d.AddForce (movement * speed);
			//rb2d.velocity = Vector2.ClampMagnitude(rb2d.velocity, speed);
			rb2d.velocity = new Vector2 (moveHorizontal * speed, rb2d.velocity.y);
			if (rb2d.velocity.magnitude <= 1) {
				state = PlayerState.IDLE;
			}
			break;

		case PlayerState.JUMP_CHARGE:
			if(Input.GetKey(KeyCode.Space) && IsGrounded()) {
				jump_timer += Time.deltaTime;
			}
			if (Input.GetKeyUp(KeyCode.Space) && IsGrounded()){
				jump_impulse.y = Mathf.Max(jump_impulse_max * Mathf.Min(jump_timer, jump_timer_max) / jump_timer_max, jump_impulse_min);
				Debug.Log (jump_impulse);
				rb2d.AddForce (jump_impulse, ForceMode2D.Impulse);
				jump_timer = 0;
				state = PlayerState.JUMP;
			} else if (Input.GetKeyUp(KeyCode.Space)) {
				Debug.Log ("can't jump");
			}
			break;

		case PlayerState.JUMP:
			if (IsGrounded () && rb2d.velocity.y < 1) {
				state = PlayerState.IDLE;
			}
			break;

		}
	}

	//FixedUpdate is called at a fixed interval and is independent of frame rate. Put physics code here.
	void LateUpdate()
	{
		
		Debug.DrawRay(transform.position, -transform.up * rayDistance, Color.yellow);
		
		anim.SetFloat ("speed", rb2d.velocity.magnitude);
		Debug.Log (rb2d.velocity.magnitude);

	}

}
