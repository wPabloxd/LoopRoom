using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDetection : MonoBehaviour
{
    public bool grabbed;
    public Vector3 limit;
    [SerializeField] GameObject[] clones;
    [SerializeField] GameObject doors;
    [SerializeField] GameObject room;
    [SerializeField] GameObject room2;
    [SerializeField] GameObject lvl1;
    [SerializeField] GameObject player;
    public int place;
    public bool loop;

    private void Update()
    {
        if (transform.position.z > limit.z && transform.position.z < limit.x && lvl1.activeInHierarchy)
        {
            place = 0;
            foreach (GameObject clone in clones)
            {
                clone.SetActive(true);
            }
            if (!grabbed)
            {
                gameObject.transform.parent = room.transform;
            }
            if (grabbed && player.GetComponent<Movement>().oldParent != null)
            {
                player.GetComponent<Movement>().oldParent = room;
            }
        }
        else if (transform.position.z < limit.z && (!doors.GetComponent<DoorSwap>().door1Open || !doors.GetComponent<DoorSwap>().door2Open) && lvl1.activeInHierarchy)
        {
            place = 1;
        }
        else if ((transform.position.z > limit.x && (!doors.GetComponent<DoorSwap>().door1Open || !doors.GetComponent<DoorSwap>().door2Open)) || (room2.activeInHierarchy && !lvl1.activeInHierarchy))
        {
            place = 2;
            if(grabbed && player.GetComponent<Movement>().oldParent != null)
            {
                player.GetComponent<Movement>().oldParent = room2;
            }
            if (!grabbed)
            {
                gameObject.transform.parent = room2.transform;
            }     
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Push")
        {
            doors.GetComponent<DoorSwap>().chairStuck = true;
        }
        else if(other.tag == "Loop" && !grabbed)
        {
            loop = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Push")
        {
            doors.GetComponent<DoorSwap>().chairStuck = false;
        }
        else if (other.tag == "Loop")
        {
            loop = false;
        }
    }
}
