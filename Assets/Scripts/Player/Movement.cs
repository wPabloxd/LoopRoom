using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] LayerMask ground;
    [SerializeField] LayerMask interactMask;
    [SerializeField] LayerMask grabMask;
    [SerializeField] Transform cam;
    CapsuleCollider capCollider;
    Rigidbody playerRB;

    Vector3 inputMovement;
    Vector3 moveDir;
    Vector3 slopeMoveDir;

    float speed = 3;
    float airSpeed = 0.3f;
    float slopeSin;
    float slopeCos;
    float targetAngle;
    
    bool slopeLimit;
    bool slope;
    bool moving;
    bool jump;
    bool grounded;

    Vector3 slopeHit;

    [SerializeField] Transform grabArea;
    GameObject grabObject;
    public GameObject oldParent;
    GameObject highlighted;
    bool grabbed;
    Quaternion auxRot;

    ContactPoint LastContactPoint;

    void Start()
    {
        capCollider = GetComponent<CapsuleCollider>();
        playerRB = GetComponent<Rigidbody>();
    }
    void Update()
    {
        Interact();
        if (grabbed)
        {
            MoveGrabbed();
        }
        inputMovement = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        if (inputMovement.magnitude >= 0.1f)
        {
            moving = true;
            targetAngle = Mathf.Atan2(inputMovement.x, inputMovement.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            moveDir = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;    
            slopeMoveDir = Vector3.ProjectOnPlane(moveDir.normalized, slopeHit);
        }
        else
        {
            moving = false;
            moveDir = Vector3.zero;
            slopeMoveDir = Vector3.zero;
        }
        if (Input.GetKeyDown(KeyCode.Space) && grounded && !slopeLimit)
        {
            Jump();
        }
    }
    void Highligth()
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 2, ground | interactMask) && !grabbed)
        {
            if (hit.collider.gameObject.tag == "Object" || hit.collider.gameObject.tag == "Door" || hit.collider.gameObject.tag == "Mirror")
            {
                if(highlighted != null)
                {
                    highlighted.GetComponent<Outline>().enabled = false;
                }
                hit.collider.gameObject.GetComponent<Outline>().enabled = true;
                highlighted = hit.collider.gameObject;          
            }
            else if(highlighted != null)
            {
                highlighted.GetComponent<Outline>().enabled = false;
            }
        }
        else
        {
            if (highlighted != null && !grabbed)
            {
                highlighted.GetComponent<Outline>().enabled = false;
                highlighted = null;
            }
        }
    }
    private void IsGrounded()
    {
        if(Physics.CheckSphere(transform.position - new Vector3(0, 0.85f, 0), 0.4f, ground))
        {
            grounded = true;
        }
        else
        {
            grounded = false;
        }
    }
    private void FixedUpdate()
    {
        MovePlayer();
        Highligth();
    }
    void MovePlayer()
    {
        if (grounded && !slope && !slopeLimit)
        {
            playerRB.drag = 13;
            playerRB.AddRelativeForce(moveDir.normalized * speed * 30, ForceMode.Acceleration);
        }
        else if (grounded && slope && !slopeLimit)
        {
            if (moving)
            {
                playerRB.drag = 13;
                playerRB.AddRelativeForce(Vector3.up * 9.81f, ForceMode.Acceleration);
                playerRB.AddRelativeForce(Vector3.Normalize(slopeHit) * -98.1f, ForceMode.Acceleration);
                playerRB.AddRelativeForce(slopeMoveDir.normalized * speed * 30 * slopeCos, ForceMode.Acceleration);
            }
            else
            {
                playerRB.drag = 13;
                playerRB.AddRelativeForce(Vector3.up * 9.81f, ForceMode.Acceleration);
                playerRB.AddRelativeForce(Vector3.Normalize(slopeHit) * -9.81f, ForceMode.Acceleration);
            }

        }
        else if (grounded && slope && slopeLimit)
        {
            playerRB.drag = 0;
            playerRB.AddRelativeForce(moveDir.normalized * airSpeed * 10, ForceMode.Acceleration);
            playerRB.AddRelativeForce(Vector3.down * 10, ForceMode.Acceleration);
        }
        else if (!grounded)
        {
            playerRB.drag = 1;
            playerRB.AddRelativeForce(moveDir.normalized * airSpeed * 17, ForceMode.Acceleration);
        }
        if (jump)
        {
            if(grounded || slope)
            {
                playerRB.AddRelativeForce(Vector3.up * 7, ForceMode.VelocityChange);
            }
            jump = false;
        }
    }
    void Jump()
    {
        jump = true;
    }
    void Interact()
    {
        if (Input.GetKeyDown(KeyCode.E) && !grabbed)
        {
            RaycastHit hit;
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 2, interactMask))
            {
                if (hit.collider.gameObject.tag == "Door")
                {
                    hit.collider.gameObject.GetComponentInParent<ToggleAn>().interacted = true;
                }
                else if (hit.collider.gameObject.tag == "Object" && !grabbed)
                {
                    Grab(hit.collider.gameObject);
                }
                else if (hit.collider.gameObject.tag == "Mirror" && !grabbed)
                {
                    hit.collider.gameObject.GetComponent<MimicObject>().Swap();
                    Grab(hit.collider.gameObject.GetComponent<MimicObject>().original);
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            Ungrab();
        }
    }
    void Grab(GameObject grab)
    {
        Collider[] coll = grab.gameObject.GetComponents<Collider>();
        foreach (Collider col in coll)
        {
            Physics.IgnoreCollision(col, gameObject.GetComponent<Collider>());
        }
        grabObject = grab.gameObject;
        grabObject.GetComponent<ObjectDetection>().grabbed = true;
        grabObject.GetComponent<Rigidbody>().useGravity = false;
        grabObject.GetComponent<Rigidbody>().drag = 10;
        grabObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
        oldParent = grabObject.transform.parent.gameObject;
        grabObject.GetComponent<Rigidbody>().transform.parent = grabArea;
        auxRot = grabObject.transform.localRotation;
        grabbed = true;
        if (highlighted != null)
        {
            highlighted.GetComponent<Outline>().enabled = false;
            highlighted = grab;
            highlighted.GetComponent<Outline>().enabled = true;
        }
    }
    void Ungrab()
    {
        Collider[] coll = grabObject.GetComponents<Collider>();
        foreach (Collider col in coll)
        {
            Physics.IgnoreCollision(col, gameObject.GetComponent<Collider>(), false);
        }
        grabObject.GetComponent<ObjectDetection>().grabbed = false;
        grabObject.GetComponent<Rigidbody>().useGravity = true;
        grabObject.GetComponent<Rigidbody>().drag = 1;
        grabObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        grabObject.transform.SetParent(oldParent.transform, true);
        oldParent = null;
        grabObject = null;
        grabbed = false;
    }
    void MoveGrabbed()
    {
        if (Vector3.Distance(grabObject.transform.position, grabArea.position) > 0.1f && grabbed)
        {
            Vector3 moveDirection = grabArea.position - grabObject.transform.position;
            grabObject.GetComponent<Rigidbody>().AddForce(moveDirection * 150);
            grabObject.transform.localRotation = auxRot * grabArea.localRotation;
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.layer == 3 || collision.gameObject.layer == 9)
        {
            foreach (ContactPoint p in collision.contacts)
            {
                if(p.point.y < transform.position.y - 0.56f)
                {
                    LastContactPoint = p;
                    Vector3 bottom = capCollider.bounds.center - (Vector3.up * capCollider.bounds.extents.y);
                    Vector3 curve = bottom + (Vector3.up * capCollider.radius);
                    slopeSin = Mathf.Sqrt(p.normal.x * p.normal.x + p.normal.z * p.normal.z);
                    slopeCos = Mathf.Sqrt(p.normal.y * p.normal.y);
                    Vector3 dir = curve - p.point;
                    slopeHit = p.normal;
                    Debug.DrawLine(curve, p.point, Color.blue, 0.55f);
                    if (p.point.y < curve.y - 0.15f)
                    {
                        grounded = true;
                    }
                    if (slopeSin < 0.05f && p.point.y < transform.position.y - 0.55f)
                    {
                        slope = false;
                        slopeLimit = false;
                    }
                    else if (slopeSin <= 0.708f && p.point.y < transform.position.y - 0.55f)
                    {
                        slope = true;
                        slopeLimit = false;
                    }
                    else if (slopeSin < 1 && p.point.y < transform.position.y - 0.55f)
                    {
                        slope = true;
                        slopeLimit = true;
                    }
                    else if (slopeSin >= 1 && p.point.y < transform.position.y - 0.55f)
                    {
                        slope = false;
                        slopeLimit = false;
                    }
                }               
            }
        }        
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == 3)
        {
            if(LastContactPoint.point.y < transform.position.y - 0.56f)
            {
                grounded = false;
            }
        }
    }
}