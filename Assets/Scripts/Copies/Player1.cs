using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player1 : MonoBehaviour
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

    void Start()
    {
       
    }

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
            Debug.Log("JUMP");
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
            controller.Move(new Vector3(move.x * speed, velocity.y, move.z * speed) * Time.deltaTime);
            if (!isGrounded)
            {
                jumping = false;
            }
        }
        if (isGrounded && groundFlat) //SUELO
        {
            Debug.Log("SUELO");
            if (Input.GetButtonDown("Jump") || jumping)
            {
                jumping = true;
            }
            else
            {
                velocity.y = 0;
            }
            //jumping = false;
            airMovement = controller.velocity;
            fall = airMovement;
            //fall = new Vector3(airMovement.y, airMovement.y, airMovement.y);
            controller.Move(new Vector3(move.x * speed, velocity.y, move.z * speed) * Time.deltaTime);
        }
        else if (isGrounded && slope && !slopeLimit) //RAMPA
        {
            //velocity.y = 0;
            Debug.Log("RAMPA");
            if (Input.GetButtonDown("Jump") || jumping)
            {
                jumping = true;
                //velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
            }
            else
            {
                velocity.y = -slopeHit.y * slide;
            }
            airMovement = controller.velocity;
            fall = airMovement;
            //fall = new Vector3(airMovement.y, airMovement.y, airMovement.y);
            controller.Move(new Vector3(move.x * speed, velocity.y, move.z * speed) * Time.deltaTime);
        }
        else if (isGrounded && slopeLimit) //RAMPON
        {
            //jumping = false;
            velocity.y = 0;
            Debug.Log("RAMPON");
            //airMovement = controller.velocity;

            //fall.x = fall.x / slopeHit.x;
            //fall.z = fall.z / slopeHit.z;

            fall.x -= slopeHit.x * slopeHit.x * gravity * Time.deltaTime;
            fall.y -= gravity * Time.deltaTime;
            fall.z -= slopeHit.z * slopeHit.z * gravity * Time.deltaTime;
            controller.Move(new Vector3(fall.x, -fall.y, fall.z) * Time.deltaTime);
            //controller. += slopeHit.x * slide;
        }
        else if (!isGrounded) //AIRE
        {
            Debug.Log("AIRE");
            airMovement = controller.velocity;
            //fall = airMovement;
            //fall = new Vector3(airMovement.y, airMovement.y, airMovement.y);
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
            //velocity.y += gravity * Time.deltaTime;
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
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 2, interactMask))
            {
                hit.collider.gameObject.GetComponentInParent<ToggleAn>().interacted = true;
            }
        }
    }
    private void FixedUpdate()
    {
        groundChecker = new Vector3(transform.position.x, groundCheck.position.y - 0.03f, transform.position.z);
        Vector3 ceilingChecker = new Vector3(transform.position.x, ceilingCheck.position.y + 0.03f, transform.position.z);
        //isGrounded = Physics.CheckSphere(groundChecker, groundDistance, groundMask);
        //RaycastHit hit;
        //isGrounded = Physics.SphereCast(groundCheck.position, 0.55f, Vector3.down, out hit, 0.1f, groundMask, QueryTriggerInteraction.Ignore);
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
            //Vector3 world = transform.TransformPoint(groundCheck.transform.position);
            Debug.Log("COÑO");
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
                //Debug.Log("SUELO");
                //isGrounded = true;
                groundFlat = true;
                slope = false;
                slopeLimit = false;
            }
            else if (slopeSin <= 0.708f)
            {
                //Debug.Log("RAMPA");
                //isGrounded = true;
                groundFlat = false;
                slope = true;
                slopeLimit = false;
            }
            else if (slopeSin < 1)
            {
                //Debug.Log("RAMPON");
                //isGrounded = true;
                groundFlat = false;
                slope = true;
                slopeLimit = true;
            }
            else
            {
                //isGrounded = false;
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