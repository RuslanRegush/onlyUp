using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;


public class move : MonoBehaviour
{
    public Vector2 Move;
    public Vector2 look;
    public bool sprint;
    public bool jump ;
    void OnMove(InputValue value)
    { Move = value.Get<Vector2>(); }
    public int speed;
    //
    
    //
    
    // Start is called before the first frame update
    void Start()
    {
          
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void    OnLook(InputValue value)
    {
        look = value.Get<Vector2>();
    }
    void OnSprint(InputValue value)
    { sprint = value.isPressed; }
    void OnJump(InputValue value) 
    {
        jump = value.isPressed;
    }
    
}
