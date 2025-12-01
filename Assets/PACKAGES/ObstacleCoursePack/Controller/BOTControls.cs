using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Pathfinding;


[RequireComponent (typeof (Rigidbody))]
[RequireComponent (typeof (CapsuleCollider))]

public class BOTControls : MonoBehaviour {
	
	public float speed = 10.0f;
	public float airVelocity = 8f;
	public float gravity = 10.0f;
	public float maxVelocityChange = 10.0f;
	public float jumpHeight = 2.0f;
	public float maxFallSpeed = 20.0f;
	public float rotateSpeed = 25f; //Speed the player rotate
	private Vector3 moveDir;
	
	private Rigidbody rb;
	public Animator animator;
	public AIPath aIPath;

	



	private float distToGround;

	private bool canMove = true; //If player is not hitted
	private bool isStuned = false;
	private bool wasStuned = false; //If player was stunned before get stunned another time
	private float pushForce;
	private Vector3 pushDir;

	public Vector3 checkPoint;
	private bool slide = false;

	float time;

	

	

	void  Start (){
		animator = GetComponent<Animator>();
		// get the distance to ground
		distToGround = GetComponent<Collider>().bounds.extents.y;
		aIPath = GetComponent<AIPath>();
		time = Random.Range(1, 10);

		StartCoroutine(Countdown());

	}

	IEnumerator Countdown()
	{
		aIPath.canMove = false;
		yield return new WaitForSeconds(4.0f);
		aIPath.canMove = true;
	}

	private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Jump")
        {
			Jump();
		}
		
    }

    bool IsGrounded (){
		return Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.1f);
	}

	void Jump()
	{
			Vector3 velocity = rb.linearVelocity;
			animator.SetTrigger("Jump");
			rb.linearVelocity = new Vector3(velocity.x, CalculateJumpVerticalSpeed(), velocity.z);
	}

	void Awake () {
		rb = GetComponent<Rigidbody>();
		rb.freezeRotation = true;
		rb.useGravity = false;

		checkPoint = transform.position;
		
	}
	
	void FixedUpdate () {
		if (canMove)
		{
			

			if (moveDir.x != 0 || moveDir.z != 0)
			{
				Vector3 targetDir = moveDir; //Direction of the character

				targetDir.y = 0;
				if (targetDir == Vector3.zero)
					targetDir = transform.forward;
				Quaternion tr = Quaternion.LookRotation(targetDir); //Rotation of the character to where it moves
				Quaternion targetRotation = Quaternion.Slerp(transform.rotation, tr, Time.deltaTime * rotateSpeed); //Rotate the character little by little
				transform.rotation = targetRotation;
			}

			if (IsGrounded())
			{
			 // Calculate how fast we should be moving
				Vector3 targetVelocity = moveDir;
				targetVelocity *= speed;

				// Apply a force that attempts to reach our target velocity
				Vector3 velocity = rb.linearVelocity;
				if (targetVelocity.magnitude < velocity.magnitude) //If I'm slowing down the character
				{
					targetVelocity = velocity;
					rb.linearVelocity /= 1.1f;
				}
				Vector3 velocityChange = (targetVelocity - velocity);
				velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
				velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
				velocityChange.y = 0;
				if (!slide)
				{
					if (Mathf.Abs(rb.linearVelocity.magnitude) < speed * 1.0f)
						rb.AddForce(velocityChange, ForceMode.VelocityChange);
				}
				else if (Mathf.Abs(rb.linearVelocity.magnitude) < speed * 1.0f)
				{
					rb.AddForce(moveDir * 0.15f, ForceMode.VelocityChange);
					//Debug.Log(rb.velocity.magnitude);
				}

				

			}
			else
			{
				

				if (!slide)
				{
					Vector3 targetVelocity = new Vector3(moveDir.x * airVelocity, rb.linearVelocity.y, moveDir.z * airVelocity);
					Vector3 velocity = rb.linearVelocity;
					Vector3 velocityChange = (targetVelocity - velocity);
					velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
					velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
					rb.AddForce(velocityChange, ForceMode.VelocityChange);
					if (velocity.y < -maxFallSpeed)
						rb.linearVelocity = new Vector3(velocity.x, -maxFallSpeed, velocity.z);
				}
				else if (Mathf.Abs(rb.linearVelocity.magnitude) < speed * 1.0f)
				{
					rb.AddForce(moveDir * 0.15f, ForceMode.VelocityChange);
				}
			}
		}
		else
		{
			rb.linearVelocity = pushDir * pushForce;
			

		}
		// We apply gravity manually for more tuning control
		rb.AddForce(new Vector3(0, -gravity * GetComponent<Rigidbody>().mass, 0));

		

	}

	private void Update()
	{

		if (time < 0)
		{
			int rd = Random.Range(0, 5);
			if (rd == 2) {
				Jump();
				time = Random.Range(1, 20); }
		}
		else time -= Time.deltaTime;
		
		


		if (rb.linearVelocity.magnitude > 0.1f)
		{
			
			animator.SetBool("Run", true);
		}
		

		RaycastHit hit;
		if (Physics.Raycast(transform.position, -Vector3.up, out hit, distToGround + 0.1f))
		{
			if (hit.transform.tag == "Slide")
			{
				slide = true;
			}
			else
			{
				slide = false;
			}

			
		}
	}


 
    float CalculateJumpVerticalSpeed () {
		// From the jump height and gravity we deduce the upwards speed 
		// for the character to reach at the apex.
		return Mathf.Sqrt(2 * jumpHeight * gravity);
	}

	public void HitPlayer(Vector3 velocityF, float time, int fallback_value)
	{
        switch (fallback_value)
        {
			case 0 :
				animator.SetTrigger("Fallback");
				break;
			case 1:
				animator.SetTrigger("Fallback_2");
				break;
			case 2:
				animator.SetTrigger("Hit_Small");
				break;
		}

		


		rb.linearVelocity = velocityF;
		pushForce = velocityF.magnitude;
		pushDir = Vector3.Normalize(velocityF);
		StartCoroutine(Decrease(velocityF.magnitude, time));
	}

	public void LoadCheckPoint()
	{
		transform.position = checkPoint;
	}

	private IEnumerator Decrease(float value, float duration)
	{
		if (isStuned) 
			wasStuned = true;
		isStuned = true;
		canMove = false;
		aIPath.canMove = false;

		float delta = 0;
		delta = value / duration;

		for (float t = 0; t < duration; t += Time.deltaTime)
		{
			yield return null;
			if (!slide) //Reduce the force if the ground isnt slide
			{
				pushForce = pushForce - Time.deltaTime * delta;
				pushForce = pushForce < 0 ? 0 : pushForce;
				//Debug.Log(pushForce);
			}
			rb.AddForce(new Vector3(0, -gravity * GetComponent<Rigidbody>().mass, 20)); //Add gravity
		}

		if (wasStuned)
		{
			wasStuned = false;
		}
		else
		{
			isStuned = false;
			canMove = true;
			aIPath.canMove = true;
		}
	}
}
