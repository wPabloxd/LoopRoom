using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MimicObject : MonoBehaviour
{
    public GameObject original;
    [SerializeField] Vector3 offset;
    [SerializeField] Vector3 threshold;
    [SerializeField] bool side;
    [SerializeField] GameObject doorM;
    DoorSwap dS;
    private void Awake()
    {
        dS = doorM.GetComponent<DoorSwap>();
    }
    void Update()
    {
        transform.position = original.transform.position + offset;
        transform.rotation = original.transform.rotation;
        if (transform.position.z > threshold.z && side)
        {
            if (dS.door1Open && dS.door2Open)
            {
                Swap();
            }
        }
        else if (transform.position.z < threshold.z && !side)
        {
            if (dS.door1Open && dS.door2Open)
            {
                Swap();
            }
        }
    }
    private void OnEnable()
    {
        if (dS.door1Open && dS.door2Open)
        {
            if (original.transform.position.z < original.GetComponent<ObjectDetection>().limit.z || original.transform.position.z > original.GetComponent<ObjectDetection>().limit.x)
            {
                gameObject.SetActive(false);
            }           
        }
    }
    public void Swap()
    {
        if (!original.GetComponent<ObjectDetection>().grabbed)
        {
            Vector3 aux = transform.position;
            transform.position = original.transform.position;
            original.transform.position = aux;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" || other.tag == "Object")
        {
            Swap();
        }
    }
}