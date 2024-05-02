using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleports1 : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject otherPortal;
    [SerializeField] GameObject mainCam;
    [SerializeField] GameObject cloneCam;
    Vector3 auxPos;
    bool late;
    void Start()
    {
        
    }
    void Update()
    {
        
    }
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

            auxPos = mainCam.transform.position;
            player.transform.position = cloneCam.transform.position + new Vector3(0, -0.75f, 0);
            late = true;
          
            //Quaternion t = Quaternion.Inverse(transform.rotation) * player.transform.rotation;
            //player.transform.eulerAngles = Vector3.up * (otherPortal.transform.eulerAngles.y - (transform.eulerAngles.y - player.transform.eulerAngles.y) + 180);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        
    }
    private void LateUpdate()
    {
        if (late)
        {
            late = false;
            cloneCam.transform.position = auxPos;
            Vector3 CamLEA = mainCam.transform.localEulerAngles;
            mainCam.transform.localEulerAngles = Vector3.right * (otherPortal.transform.eulerAngles.x + mainCam.transform.localEulerAngles.x);
        }
    }
}
