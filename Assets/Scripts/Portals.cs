using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using RenderPipeline = UnityEngine.Rendering.RenderPipelineManager;

public class Portals : MonoBehaviour
{
    public Vector3 offset;
    [SerializeField] Camera cloneCamera;
    [SerializeField] Camera mainCamera;
    [SerializeField] Transform player;
    [SerializeField] GameObject wall1;
    [SerializeField] GameObject wall2;
    [SerializeField] Shader shader;
    [SerializeField] Vector3 threshold;
    [SerializeField] Vector3 addition;
    [SerializeField] Vector3 lenght;
    [SerializeField] Vector3 offsetRotation;
    [SerializeField] RenderTexture tempTexture;
    [SerializeField] bool flipCam;
    [SerializeField] bool dual;
    [SerializeField] bool offCam;
    Plane portal1;
    Plane portal2;
    private void Awake()
    {
        wall1.GetComponent<Renderer>().material.mainTexture = tempTexture;
        wall2.GetComponent<Renderer>().material.mainTexture = tempTexture;
        wall1.GetComponent<Renderer>().material.shader = shader;
        wall2.GetComponent<Renderer>().material.shader = shader;
    }
    void Start()
    {
        if (!dual)
        {
            wall1.GetComponent<MeshRenderer>().enabled = false;
            wall2.GetComponent<MeshRenderer>().enabled = true;
        }
        else if (offCam)
        {
            wall1.GetComponent<MeshRenderer>().enabled = false;
            wall2.GetComponent<MeshRenderer>().enabled = false;
        }
        else
        {
            wall1.GetComponent<MeshRenderer>().enabled = true;
            wall2.GetComponent<MeshRenderer>().enabled = true;
        }
    }
    void Update()
    {
        if (player.transform.position.z > threshold.z)
        {
            addition = lenght;
        }
        else
        {
            addition = Vector3.zero;
        }
    }
    private void LateUpdate()
    {
        UpdateCamera();
    }
    void UpdateCamera()
    {
        cloneCamera.targetTexture = tempTexture;
        if (!GameManager.flip || !flipCam)
        {
            if (offsetRotation.y == -90)
            {
                cloneCamera.transform.position = new Vector3(-player.position.z, player.position.y, player.position.x) + offset;
            }
            else
            {
                cloneCamera.transform.position = player.position + offset + addition;
            }
        }
        else if (GameManager.flip)
        {
            cloneCamera.transform.position = player.position - new Vector3(offset.x, -offset.y, offset.z) - addition;
        }
        cloneCamera.transform.eulerAngles = new Vector3(mainCamera.transform.rotation.eulerAngles.x, mainCamera.transform.rotation.eulerAngles.y, 0) + offsetRotation;
    }
}