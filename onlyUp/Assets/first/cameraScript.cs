using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.Windows;
using TMPro;
public class cameraScript : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5;
    [SerializeField] float sprintSpeed = 10;
    [SerializeField] float gravity = -10;
    [SerializeField] float jumpHeight = 2;
    [SerializeField] GameObject groundCheck;
    [SerializeField] LayerMask groundLayer;
    public TextMeshProUGUI timpText;

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
    [SerializeField] GameObject wings;
    float xRotation;
    float yRotation;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>(); 
        input = GetComponent<move>();
        controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        wings.gameObject.SetActive(false);
        timpText.gameObject.SetActive(false);
    }
    public void fly()
    {
       // if (UnityEngine.Input.GetKey(KeyCode.T))
        {
            //transform.position = new Vector3(0, 0, 0);
            velocity.y = 10;
            //velocity.z += 5;
            Debug.Log("T");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Aripi"))
        {
            if (1 == 0)
            {
                wings.gameObject.SetActive(true);
                timp = 0;
                InvokeRepeating("fly", 0, 0.5f);
                zboara = true;
                Destroy(other.gameObject);
                timpText.gameObject.SetActive(true);
                Debug.Log("col");
            } else
            {
                velocity.y = 25;
            }
        }
    }

    public float timpul = 5f;
    public float timp = 0f;
    public bool zboara = false;

    // Update is called once per frame
    void Update()
    {
      if(zboara)
        {
            timp += Time.deltaTime;
            timpText.text = "" + timp.ToString("F2") + "/5";
        }

      if(timp >= timpul)
        {
            zboara = false;
            wings.gameObject.SetActive(false);
            CancelInvoke("fly");
            timpText.gameObject.SetActive(false);

            timp = 0;
        }
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
        }
       // LedgeGrab();
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
            controller.Move(Vector3.zero);
        }
    }
    void JumpAndGravity()
    {
        isGrounded = Physics.CheckSphere(groundCheck.transform.position, .2f, groundLayer);
        if (isGrounded && UnityEngine.Input.GetKey(KeyCode.Space))
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
            animator.SetBool("grownded", false);
            animator.SetBool("Jump", true);
        }
        else if (isGrounded)
        {
            animator.SetBool("grownded", true);
            animator.SetBool("Jump", false);
            animator.SetBool("fall", false);
        }
        else
        {
            velocity.y += gravity * Time.deltaTime;
            animator.SetBool("grownded", false);
            animator.SetBool("Jump", false);
            animator.SetBool("fall", true);
        }
        controller.Move(velocity * Time.deltaTime);

    }

}