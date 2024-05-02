using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class MouseController : MonoBehaviour
{
    public float mouseSens = 200;
    public Transform playerBody;
    float xRotation;
    float yRotation;
    public float offsetRotation;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSens * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSens * Time.deltaTime;
        xRotation -= mouseY;
        yRotation += mouseX;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        transform.localRotation = Quaternion.Euler(xRotation, yRotation + offsetRotation, 0);
    }
    public void AddOffset(float angles)
    {
        if (angles == 0)
        {
            return;
        }
        offsetRotation += angles;
    }
}