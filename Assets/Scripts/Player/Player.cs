using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] CharacterController controller;
    [SerializeField] float speed = 8f;
    Vector3 velocity;
    float gravity = -9.81f;
    float jumpHeight = 1.5f;
    [SerializeField] Transform groundCheck;
    [SerializeField] Transform ceilingCheck;
    [SerializeField] float groundDistance;
    [SerializeField] LayerMask groundMask;
    [SerializeField] LayerMask interactMask;
    bool isGrounded;
    bool headTop;
    bool headTopCD;
    Vector3 airMovement;
    bool jumping;

    [SerializeField] float slide;
    [SerializeField] float verga;
    Vector3 fall;
    bool slope;
    bool slopeLimit;
    bool groundFlat = true;
    float slopeSin;
    Vector3 slopeHit;
    Vector3 groundChecker;

    [SerializeField] GameObject cam;

    void Update()
    {
        Movement();
        Interact();
    }
    void Movement()
    {
        Debug.Log(fall.y);
        velocity.y += gravity * Time.deltaTime;
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");
        Vector3 move = (transform.right * x + transform.forward * z).normalized;
        if (jumping)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
            controller.Move(new Vector3(move.x * speed, velocity.y, move.z * speed) * Time.deltaTime);
            if (!isGrounded)
            {
                jumping = false;
            }
        }
        if (isGrounded && groundFlat)
        {
            if (Input.GetButtonDown("Jump") || jumping)
            {
                jumping = true;
            }
            else
            {
                velocity.y = 0;
            }
            airMovement = controller.velocity;
            fall = airMovement;
            controller.Move(new Vector3(move.x * speed, velocity.y, move.z * speed) * Time.deltaTime);
        }
        else if (isGrounded && slope && !slopeLimit) 
        {
            if (Input.GetButtonDown("Jump") || jumping)
            {
                jumping = true;
            }
            else
            {
                velocity.y = -slopeHit.y * slide;
            }
            airMovement = controller.velocity;
            fall = airMovement;
            controller.Move(new Vector3(move.x * speed, velocity.y, move.z * speed) * Time.deltaTime);
        }
        else if (isGrounded && slopeLimit)
        {
            velocity.y = 0;
            fall.x -= slopeHit.x * slopeHit.x * gravity * Time.deltaTime;
            fall.y -= gravity * Time.deltaTime;
            fall.z -= slopeHit.z * slopeHit.z * gravity * Time.deltaTime;
            controller.Move(new Vector3(fall.x, -fall.y, fall.z) * Time.deltaTime);
        }
        else if (!isGrounded)
        {
            airMovement = controller.velocity;
            jumping = false;
            controller.Move(new Vector3(airMovement.x, velocity.y, airMovement.z) * Time.deltaTime);
            controller.Move(move * (speed / 3) * Time.deltaTime);
        }
        if (headTop)
        {
            if (!headTopCD)
            {  
                fall.y = 0;
                velocity.y = 0;
                headTopCD = true;
            }
        }
        else
        {
            headTopCD = false;
        }
    }
    void Interact()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            RaycastHit hit;
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 3, interactMask))
            {
                hit.collider.gameObject.GetComponentInParent<ToggleAn>().interacted = true;
            }
        }
    }
    private void FixedUpdate()
    {
        groundChecker = new Vector3(transform.position.x, groundCheck.position.y - 0.03f, transform.position.z);
        Vector3 ceilingChecker = new Vector3(transform.position.x, ceilingCheck.position.y + 0.03f, transform.position.z);
        headTop = Physics.CheckSphere(ceilingChecker, groundDistance, groundMask);
        Slope();
    }
    void Slope()
    {
        RaycastHit hit;
        Vector3 origin = new Vector3(transform.position.x, transform.position.y - 0.53f, transform.position.z);
        if (Physics.SphereCast(origin, 0.54f, Vector3.down, out hit, 0.05f, groundMask, QueryTriggerInteraction.Ignore))      
        {
            slopeHit = hit.normal;
            if (hit.point.y > groundCheck.transform.position.y)
            {
                isGrounded = false;
                return;
            }
            else
            {
                isGrounded = true;
            }
            Debug.DrawLine(groundCheck.transform.position, hit.point, Color.blue, 0.3f);
            slopeHit = hit.normal;
            slopeSin = Mathf.Sqrt(hit.normal.x * hit.normal.x + hit.normal.z * hit.normal.z);
            if (slopeSin < 0.05f)
            {
                groundFlat = true;
                slope = false;
                slopeLimit = false;
            }
            else if (slopeSin <= 0.708f)
            {
                groundFlat = false;
                slope = true;
                slopeLimit = false;
            }
            else if (slopeSin < 1)
            {
                groundFlat = false;
                slope = true;
                slopeLimit = true;
            }
            else
            {
                groundFlat = false;
                slope = false;
                slopeLimit = false;
            }
        }
        else
        {
            isGrounded = false;
        }
    }
}