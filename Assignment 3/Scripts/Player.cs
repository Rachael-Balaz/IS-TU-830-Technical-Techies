using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int speed = 3;
    private float gravity = 9.8f;
    public int jumpSpeed = 3;
    private float vSpeed = 0f;
    private CharacterController controller;
    private bool isJump = false;
    private bool doubleJump = false;
    private int right = 1;
    private Vector3 vel;
    public BoxCollider leftBox;
    public BoxCollider rightBox;
    public BoxCollider centreBox;
    private Transform transform;
    private Collider collider;
    public Vector3 startPos;

    // Start is called before the first frame update
    void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
        collider = gameObject.GetComponent<Collider>();
        transform = gameObject.GetComponent<Transform>();
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //Reset velocity
        vel = Vector3.zero;

        //Double Jump Code
        if (Input.GetKeyDown("space") && doubleJump == false && isJump == true)
        { 
            vSpeed = jumpSpeed;
            doubleJump = true;

            if (gravity != 9.8f)
                gravity = 9.8f;
        }
        
        //Jump Code
        if (Input.GetKeyDown("space") && isJump == false)
        {
            vSpeed = jumpSpeed;
            isJump = true;

            if (gravity != 9.8f)
                gravity = 9.8f;
        }

        //Landing Code
        if (controller.isGrounded)
        {
            vSpeed = 0;
            doubleJump = false;
            isJump = false;

            if (gravity != 0f)
                gravity = 0f;
        }
        else
        {
            if (vSpeed < -1f)
                isJump = true;

            if (gravity != 9.8f)
                gravity = 9.8f;
        }

        //Applying Gravity
        vSpeed -= gravity * Time.deltaTime;

        //Left and Right Code
        if (Input.GetKey("d"))
            vel.x = right * speed;
        else if (Input.GetKey("a"))
            vel.x = -right * speed;

        //Applying vertical speed to velocity
        vel.y = vSpeed;
        //Moving character
        controller.Move(vel * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == leftBox)
        {
            startPos = other.gameObject.transform.position;
            startPos.y += 2;
        }
        else if (other == rightBox)
        {
            startPos = other.gameObject.transform.position;
            startPos.y += 2;
        }
        else if (other == centreBox)
        {
            startPos = other.gameObject.transform.position;
            startPos.y += 2;
        }

        if (other.tag == "KillPlane")
        {
            controller.enabled = false;
            controller.transform.position = startPos;
            controller.enabled = true;
            vel.y = 0f;
        }
    }
}
