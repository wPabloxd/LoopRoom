using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleports : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject otherPortal;
    [SerializeField] GameObject mainCam;
    [SerializeField] GameObject cloneCam;
    [SerializeField] GameObject chair1;
    [SerializeField] GameObject chair2;
    [SerializeField] float offsetRotation;
    [SerializeField] bool room2bool;
    Vector3 auxPos;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            if (GameManager.flip)
            {
                GameManager.flip = false;
            }
            else
            {
                GameManager.flip = true;
            }
            if (!room2bool)
            {
                GameManager.teleported = true;
                GameManager.swap = true;
            }
            auxPos = mainCam.transform.position;
            if(chair1.GetComponent<ObjectDetection>().loop == true)
            {
                Vector3 auxPos = chair1.transform.position - player.transform.position;
                chair1.transform.position = new Vector3(-auxPos.z + cloneCam.transform.position.x, chair1.transform.position.y, auxPos.x + cloneCam.transform.position.z);
                chair1.transform.eulerAngles += new Vector3(0, -90, 0);
                chair1.GetComponent<Rigidbody>().velocity = new Vector3(-chair1.GetComponent<Rigidbody>().velocity.z, chair1.GetComponent<Rigidbody>().velocity.y, chair1.GetComponent<Rigidbody>().velocity.x);
            }
            if (chair2.GetComponent<ObjectDetection>().loop == true)
            {
                Vector3 auxPos = chair2.transform.position - player.transform.position;
                chair2.transform.position = new Vector3(-auxPos.z + cloneCam.transform.position.x, chair2.transform.position.y, auxPos.x + cloneCam.transform.position.z);
                chair2.transform.eulerAngles += new Vector3(0, -90, 0);
                chair2.GetComponent<Rigidbody>().velocity = new Vector3(-chair2.GetComponent<Rigidbody>().velocity.z, chair2.GetComponent<Rigidbody>().velocity.y, chair2.GetComponent<Rigidbody>().velocity.x);
            }
            player.transform.position = cloneCam.transform.position + new Vector3(0, -0.75f, 0);
            //mainCam.GetComponent<MouseController>().offsetRotation += offsetRotation;
            mainCam.GetComponent<MouseController>().AddOffset(offsetRotation);
            
            if(offsetRotation == -90)
            {
                player.GetComponent<Rigidbody>().velocity = new Vector3(-player.GetComponent<Rigidbody>().velocity.z, player.GetComponent<Rigidbody>().velocity.y, player.GetComponent<Rigidbody>().velocity.x);
            }
            else if(offsetRotation == 180)
            {
                player.GetComponent<Rigidbody>().velocity = new Vector3(-player.GetComponent<Rigidbody>().velocity.x, player.GetComponent<Rigidbody>().velocity.y, -player.GetComponent<Rigidbody>().velocity.z);
            }
            else if (offsetRotation == 90)
            {
                player.GetComponent<Rigidbody>().velocity = new Vector3(player.GetComponent<Rigidbody>().velocity.z, player.GetComponent<Rigidbody>().velocity.y, -player.GetComponent<Rigidbody>().velocity.x);
            }
            cloneCam.transform.position = auxPos;
            Physics.SyncTransforms();
        }
    }
}