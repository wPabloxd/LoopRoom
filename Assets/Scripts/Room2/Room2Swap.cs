using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room2Swap : MonoBehaviour
{
    [SerializeField] GameObject exit;
    [SerializeField] GameObject portal;
    [SerializeField] GameObject roomLimiter;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Object")
        {
            roomLimiter.GetComponent<RoomLimits>().chairPlaced = true;
            portal.gameObject.SetActive(false);
            exit.gameObject.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Object")
        {
            roomLimiter.GetComponent<RoomLimits>().chairPlaced = false;
            portal.gameObject.SetActive(true);
            exit.gameObject.SetActive(false);
        }
    }
}