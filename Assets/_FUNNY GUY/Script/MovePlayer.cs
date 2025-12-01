using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovePlayer : MonoBehaviour
{
    public FixedJoystick leftJoyStick;
    public Button jumpBtn;

    public FixedTouchField touchField;
    Animator animator;
    public bool isGrounded;

    protected Rigidbody rigidbody;

    protected float cameraAngleY = 180;
    protected float cameraAngleSpeed = 0.1f;
    protected float cameraPosY;
    protected float cameraPosSpeed = 0.1f;

    private float pushForce;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        leftJoyStick = GameObject.Find("Fixed Joystick").GetComponent<FixedJoystick>();
        jumpBtn = GameObject.Find("btnJump").GetComponent<Button>();
        
        touchField = GameObject.Find("PanelTouch").GetComponent<FixedTouchField>();
        animator = GetComponent<Animator>();

        jumpBtn.onClick.AddListener(() => JumpBtn());
    
    }

    void OnCollisionEnter(Collision collision)
    {
       if(collision.gameObject.tag == "FLOOR")
        {
            isGrounded = true;
        }

    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "FLOOR")
        {
            isGrounded = false;
        }

    }

    void JumpBtn()
    {
        if (isGrounded)
        {
           
            rigidbody.AddForce(Vector3.up * 30, ForceMode.Impulse);
            animator.SetTrigger("Jump");
            isGrounded = false;
        }
       
    
    }

  




    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            JumpBtn();

        var input = new Vector3(leftJoyStick.input.x, 0, leftJoyStick.input.y);
        var vel = Quaternion.AngleAxis(cameraAngleY + 180, Vector3.up) * input * 20f;

        rigidbody.linearVelocity = new Vector3(vel.x, rigidbody.linearVelocity.y, vel.z);
        //transform.rotation = Quaternion.AngleAxis(cameraAngleY + 180 + Vector3.SignedAngle(Vector3.forward, input.normalized + Vector3.forward * 0.001f, Vector3.up), Vector3.up);
        if (leftJoyStick.input.x != 0 || leftJoyStick.input.y != 0) transform.rotation = Quaternion.LookRotation(rigidbody.linearVelocity);


        if (rigidbody.linearVelocity.magnitude > 1f)
        {

            animator.SetBool("Run", true);
        }
        else
        {
            animator.SetBool("Run", false);
        }


        cameraAngleY += touchField.TouchDist.x * cameraAngleSpeed;
        Camera.main.transform.position = transform.position + Quaternion.AngleAxis(cameraAngleY, Vector3.up) * new Vector3(0, 5, 5);
        Camera.main.transform.rotation = Quaternion.LookRotation(transform.position + Vector3.up * 2f - Camera.main.transform.position, Vector3.up);
    }
}
