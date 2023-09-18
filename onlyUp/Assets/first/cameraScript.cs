using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.Windows;

public class cameraScript : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5;
    [SerializeField] float sprintSpeed = 10;
    [SerializeField] float gravity = -10;
    [SerializeField] float jumpHeight = 2;
    [SerializeField] GameObject groundCheck;
    [SerializeField] LayerMask groundLayer;
    Rigidbody rb;
    bool hanging;
    bool canMove = true;
    bool isGrounded;
    private bool isRunning = false;
    Vector3 velocity;
    Vector3 yclimbing;
    float reachDistance;


    public Animator animator;
    private move input;
    private CharacterController controller;
    [SerializeField] Transform cameraFollowTarget;
    [SerializeField] GameObject mainCam;
    float xRotation;
    float yRotation;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>(); 
        input = GetComponent<move>();
        controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            // Toggle running when Shift key is held
            if (UnityEngine.Input.GetKey(KeyCode.LeftShift))
            {
                isRunning = true;
                animator.SetBool("run", true);
            }
            else
            {
                isRunning = false;
                animator.SetBool("run", false);
            }

            Movement();

            JumpAndGravity();
            if (UnityEngine.Input.GetKeyDown(KeyCode.Space))
            {
                // Perform a raycast to detect climbable surfaces
                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.forward, out hit, reachDistance))
                {
                    if (hit.collider.CompareTag("Climbable"))
                    {
                        // Start climbing
                        StartClimbing(hit.transform);
                    }
                }
            }
        }
       // LedgeGrab();
    }
    private bool isClimbing = false;
    private Transform climbingSurface;
    void StartClimbing(Transform surface)
    {
        isClimbing = true;
        climbingSurface = surface;

        animator.SetBool("climbing", true);
    }
    void CameraRotation()
    {
        xRotation += input.look.y;
        yRotation += input.look.x;
        xRotation = Mathf.Clamp(xRotation, -10, 30);

        Quaternion rotation = Quaternion.Euler(xRotation, yRotation, 0);
        cameraFollowTarget.rotation = rotation;
    }

    private void LateUpdate()
    {
        CameraRotation();
    }

    private void Movement()
    {
        float speed = isRunning ? sprintSpeed : moveSpeed;
        Vector3 inputDir = new Vector3(input.Move.x, 0, input.Move.y);
        float targetRotation = 0;

        animator.SetFloat("speed", input.Move.magnitude);

        if (input.Move != Vector2.zero)
        {
            targetRotation = Quaternion.LookRotation(inputDir).eulerAngles.y + mainCam.transform.rotation.eulerAngles.y;
            Quaternion rotation = Quaternion.Euler(0, targetRotation, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 20 * Time.deltaTime);

            Vector3 targetDirection = Quaternion.Euler(0, targetRotation, 0) * Vector3.forward;
            controller.Move(targetDirection * speed * Time.deltaTime);
        }
        else
        {
            // If no input keys are pressed, stop the character's movement
            controller.Move(Vector3.zero);
        }
    }
    void JumpAndGravity()
    {
        // Apply gravity


        // Check if the character is grounded

        isGrounded = Physics.CheckSphere(groundCheck.transform.position, .2f, groundLayer);
        
        // Check for jump input
        if (isGrounded)
        {
            if (UnityEngine.Input.GetKey(KeyCode.Space))
            {
                
                    velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
                    StartCoroutine(EnableCanMove(0.25f));
                    animator.SetBool("grownded", true);
                    animator.SetBool("Jump", true);
            }
        }
        else
        {
            velocity.y += gravity * Time.deltaTime;
            animator.SetBool("grownded", false);
            animator.SetBool("Jump", false);
        }
        controller.Move(velocity * Time.deltaTime);
        //animator.SetBool("Jump", true);
        //animator.SetBool("Jump", false);
        // Move the character with updated velocity

    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("INTRA");
        if (other.CompareTag("bbox") )//&& UnityEngine.Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetBool("climbing", true);
            Debug.Log("atingere");
            Vector3 nouaPozitie = new Vector3(0, 0, 0);
            transform.position = nouaPozitie;
          



        }
    }
    IEnumerator EnableCanMove(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        canMove = true;
    }
    private void OnTriggerExit(Collider other)
    {
        animator.SetBool("climbing", false);
    }
    void LedgeGrab()
    {
        if(rb.velocity.y < 0 && !hanging) { 
        RaycastHit downHit;
        Vector3 lineDownStart = (transform.position + Vector3.up*1.5f)+transform.forward;
        Vector3 LineDownEnd = (transform.position + Vector3.up * 0.7f) + transform.forward; ;
        Physics.Linecast(lineDownStart, LineDownEnd,out downHit,LayerMask.GetMask("ground"));  
        Debug.DrawLine(lineDownStart, LineDownEnd);
            if (downHit.collider != null)
            {
                RaycastHit fwHit;
                Vector3 lineFwStart = new Vector3(transform.position.x, downHit.point.y - 0.1f, transform.position.z);
                Vector3 LineFwEnd = new Vector3(transform.position.x, downHit.point.y - 0.1f, transform.position.z);
                Physics.Linecast(lineFwStart, LineFwEnd, out fwHit, LayerMask.GetMask("ground"));
                Debug.DrawLine(lineFwStart, LineFwEnd);
                if (fwHit.collider != null)
                {
                    gravity = 0;
                    rb.velocity = Vector3.zero;
                    hanging = true;
                    //animator.SetBool("climbing", true);
                    Vector3 hangPos = new Vector3(fwHit.point.x, downHit.point.y,fwHit.point.z);
                    Vector3 offset = transform.forward * -0.1f + transform.up * -1f;
                    hangPos += offset;
                    transform.position = hangPos;
                    transform.forward = -fwHit.normal;
                    canMove = false;
                }
            }
        }
    }

}