using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class climbing : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject stepRayUpper ;
    [SerializeField] GameObject stepRayLower ;
    [SerializeField] float stepHight = 0.3f;
    [SerializeField] float stepSmooth = 0.1f;
    private move imput;
    private CharacterController controller;
    void Start()
    {
        //stepRayUpper.transform.position = new Vector3(stepRayUpper.transform.position.x, stepHight, stepRayUpper.transform.position.z);
        imput = GetComponent<move>();
        controller = GetComponent<CharacterController>();
    }

    void stepClimb() 
    {
        Debug.Log("stepClimb");
        RaycastHit hitLower;
        if(Physics.Raycast(stepRayLower.transform.position, transform.TransformDirection(Vector3.forward),out hitLower,0.1f))
        {
            Debug.Log("if 1");
            RaycastHit hitUpper;
            if (!Physics.Raycast(stepRayUpper.transform.position, transform.TransformDirection(Vector3.forward), out hitUpper, 0.2f))
            {
                //rigidBody.position -= new Vector3(0f, -stepSmooth, 0f);
                controller.Move(Vector3.up * stepSmooth);
                Debug.Log("if2");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {   
        //stepClimb();
    }
}
