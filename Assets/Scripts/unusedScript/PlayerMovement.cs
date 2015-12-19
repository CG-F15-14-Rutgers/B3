using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

	public float speed = 5.5f;
	public float turnSmoothing = 15f;   // A smoothing value for turning the player.
	public float speedDampTime = 0.1f;  // The damping for the speed parameter


	float JumpPower = 5f;
	float GravityMultiplier = 2f;
	float RunCycleLegoffset = 0.2f;
	float MoveSpeedMultiplier = 1f;
	float AnimSpeedMultiplier = 1f;
	float GroundCheckDistance = 0.1f;

	//Vector3 Movement;
	Animator anim;
	Rigidbody playerRigidbody;

	bool isGrounded;
	float OrigGroundCheckDistance;
	const float k_half = 0.5f;
	Vector3 GroundNormal;
	bool Stairing;
	bool jump; 
	bool isFalling = false;

	void Awake(){
		anim = GetComponent<Animator> ();
		playerRigidbody = GetComponent <Rigidbody> ();

		anim.SetLayerWeight(1, 1f);
		OrigGroundCheckDistance = GroundCheckDistance;
	}
	

	void FixedUpdate(){
		float h = Input.GetAxisRaw("Horizontal");
		float v = Input.GetAxisRaw("Vertical");
		Move (h, v);

	}

	void Move(float h, float v){

		//CheckGroundStatus();
		// control and velocity handling is different when grounded and airborne:


		if (h != 0f || v != 0f) {
			Rotate (h, v);
			if(Input.GetButton("Walk")){
				anim.SetFloat ("Speed", 0.2f, speedDampTime, Time.deltaTime);
				if (Input.GetKeyDown (KeyCode.C) && isFalling == false) {
					playerRigidbody.AddForce (Vector3.up * JumpPower);
					isFalling = true;
					//isGrounded = false;
					anim.SetBool ("OnGround", true);
				} else {
					anim.SetBool ("OnGround", false);
				}
			}
				anim.SetFloat ("Speed", 5.5f, speedDampTime, Time.deltaTime);
			if (Input.GetKeyDown (KeyCode.C) && isFalling == false) {
				playerRigidbody.AddForce (Vector3.up * JumpPower);
				isFalling = true;
				//isGrounded = false;
				anim.SetBool ("OnGround", true);
			} else {
				anim.SetBool ("OnGround", false);
			}
		} else {
			// Otherwise set the speed parameter to 0.
			anim.SetFloat ("Speed", 0);
			if (Input.GetKeyDown (KeyCode.C) && isFalling == false) {
				playerRigidbody.AddForce (Vector3.up * JumpPower);
				isFalling = true;
				//isGrounded = false;
				anim.SetBool ("OnGround", true);
			} else {
				anim.SetBool ("OnGround", false);
			}
		}
	}

	void HandleAirborneMovement()
	{
		// apply extra gravity from multiplier:
		Vector3 extraGravityForce = (Physics.gravity * GravityMultiplier) - Physics.gravity;
		playerRigidbody.AddForce(extraGravityForce);
		
		GroundCheckDistance = playerRigidbody.velocity.y < 0 ? OrigGroundCheckDistance : 0.01f;
	}

	void HandleGroundedMovement(bool jump)
	{
		// check whether conditions are right to allow a jump:
		if (jump && anim.GetCurrentAnimatorStateInfo(0).IsName("Grounded"))
		{
			// jump!
			playerRigidbody.velocity = new Vector3(playerRigidbody.velocity.x, JumpPower, playerRigidbody.velocity.z);
			isGrounded = false;
			anim.applyRootMotion = false;
			GroundCheckDistance = 0.1f;
		}
	}

	void Rotate(float h, float v){
		Vector3 targetDirection = new Vector3 (h, 0f, v);
		Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
		Quaternion newRotation = Quaternion.Lerp(playerRigidbody.rotation, targetRotation, turnSmoothing * Time.deltaTime);
		playerRigidbody.MoveRotation(newRotation);
	}

	void CheckGroundStatus()
	{
		RaycastHit hitInfo;
		#if UNITY_EDITOR
		// helper to visualise the ground check ray in the scene view
		Debug.DrawLine(transform.position + (Vector3.up * 0.1f), transform.position + (Vector3.up * 0.1f) + (Vector3.down * GroundCheckDistance));
		#endif
		// 0.1f is a small offset to start the ray from inside the character
		// it is also good to note that the transform position in the sample assets is at the base of the character
		if (Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, out hitInfo, GroundCheckDistance))
		{
			GroundNormal = hitInfo.normal;
			isGrounded = true;
			anim.applyRootMotion = true;
		}
		else
		{
			isGrounded = false;
			GroundNormal = Vector3.up;
			anim.applyRootMotion = false;
		}
	}

	void OnCollisionStay(){
		isFalling = false;
		//isGrounded = true;
	
	}
}
