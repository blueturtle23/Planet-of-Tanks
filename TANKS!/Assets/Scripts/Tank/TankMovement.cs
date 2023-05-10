using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TankMovement : MonoBehaviour
{
    public float m_Speed = 12f; // How fast the tank moves backward and forward
    public float m_TurnSpeed = 180f; //How fast the tank turns in degrees per second

    private Rigidbody m_Rigidbody;
    private float m_MovementInputValue; // The current value of the movement input
    private float m_TurnInputValue;  // The current value of the turn input
    private GameManager m_GameManager;
    private void Awake()
    {
        m_Rigidbody= GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        //when the tank is turned on, make sure it is not kinematic
        m_Rigidbody.isKinematic = false; 
        
        //also reset the input values
        m_MovementInputValue = 0f;
        m_TurnInputValue = 0f;
    }

    private void OnDisable()
    {
        // when the tank is turned off, set it to kinematic so it stops moving
        m_Rigidbody.isKinematic = true;
    }

    // Update is called once per frame
    void Update()
    {
        m_MovementInputValue = Input.GetAxis("Vertical");
        m_TurnInputValue = Input.GetAxis("Horizontal");
    }

    private void FixedUpdate()
    {
           Move();
           Turn();   
    }

    private void Move()
    {
        //create a vector in the direction the tank is facing with a magnitude 
        //based on the input, speed and time between frames
        Vector3 movement = transform.forward * m_MovementInputValue * m_Speed * Time.deltaTime;

        // Apply this movement to the rigidbody's position
        m_Rigidbody.MovePosition(m_Rigidbody.position + movement);
    }

    private void Turn()
    {
        //determine the number of degrees to be turned based on the input,
        //speed and time between frames
        float turn = m_TurnInputValue * m_TurnSpeed* Time.deltaTime;

        // make this into a rotation on the y axis
        Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);

        //apply this rotation to the rigidbody's rotation
        m_Rigidbody.MoveRotation(m_Rigidbody.rotation * turnRotation);
    }
}






