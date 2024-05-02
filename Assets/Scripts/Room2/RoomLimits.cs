using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomLimits : MonoBehaviour
{
    public bool chairPlaced;
    [SerializeField] GameObject room1;
    [SerializeField] GameObject auxLight;
    [SerializeField] GameObject exit;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "MainCamera")
        {
            if(other.transform.position.x > transform.position.x)
            {
                auxLight.SetActive(false);
                room1.SetActive(true);
                if (!chairPlaced)
                {
                    exit.SetActive(false);
                }
            }
            else
            {
                auxLight.SetActive(true);
                room1.SetActive(false);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "MainCamera")
        {
            if (other.transform.position.x > transform.position.x)
            {
                if (chairPlaced)
                {
                    exit.SetActive(true);
                }
                auxLight.SetActive(true);
                room1.SetActive(false);
            }
            else
            {
                exit.SetActive(false);
                auxLight.SetActive(false);
                room1.SetActive(true);
            }
        }
    }
}