using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using RenderPipeline = UnityEngine.Rendering.RenderPipelineManager;

public class Portals1 : MonoBehaviour
{
    [SerializeField] Camera cloneCamera;
    [SerializeField] Camera mainCamera;
    [SerializeField] Transform player;
    [SerializeField] Vector3 offset;
    [SerializeField] GameObject wall1;
    [SerializeField] GameObject wall2;
    [SerializeField] int iterations = 7;
    [SerializeField] Shader shader;
    [SerializeField] Vector3 threshold;
    [SerializeField] Vector3 addition;
    [SerializeField] Vector3 lenght;
    [SerializeField] bool flipCam;
    [SerializeField] bool dual;
    [SerializeField] RenderTexture tempTexture;
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
            wall1.GetComponent<MeshRenderer>().enabled = true;
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
        //if (!GameManager.flip)
        //{
        //    wall1.GetComponent<MeshRenderer>().enabled = true;
        //    wall2.GetComponent<MeshRenderer>().enabled = false;
        //}
        //else
        //{
        //    //wall1.GetComponent<MeshRenderer>().enabled = false;
        //    //wall2.GetComponent<MeshRenderer>().enabled = true;
        //}

        //if(player.transform.position.z > threshold.z)
        //{
        //    addition = lenght;
        //}
        //else
        //{
        //    addition = Vector3.zero;
        //}
        if (player.transform.rotation.eulerAngles.y >= -90 && player.transform.rotation.eulerAngles.y <= 90)
        {
            Debug.Log("PITO");
            addition = lenght;
        }
        else if ((player.transform.rotation.eulerAngles.y >= -180 && player.transform.rotation.eulerAngles.y < -90) || (player.transform.rotation.eulerAngles.y <= 180 && player.transform.rotation.eulerAngles.y > 90)) 
        {
            Debug.Log("POTO");
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
        for (int i = iterations - 1; i >= 0; --i)
        {
            RenderCamera(portal1, portal2, i);
        }
    }
    void RenderCamera(Plane inPortal, Plane outPortal, int iterationID)
    {
        if (!GameManager.flip || !flipCam)
        {
            cloneCamera.transform.position = player.position + offset + addition;
        }
        else if (GameManager.flip)
        {
            cloneCamera.transform.position = player.position - new Vector3(offset.x, -offset.y, offset.z) - addition;
        }
        //cloneCamera.transform.rotation = player.rotation;
        cloneCamera.transform.rotation = Quaternion.Euler(mainCamera.transform.rotation.eulerAngles.x, player.rotation.eulerAngles.y, 0);

        //Vector4 clipPlaneWorldSpace = new Vector4(wall1.GetComponent<Plane>().normal.x, wall1.GetComponent<Plane>().normal.y, wall1.GetComponent<Plane>().normal.z, wall1.GetComponent<Plane>().distance);
        //Vector4 clipPlaneCameraSpace = Matrix4x4.Transpose(Matrix4x4.Inverse(cloneCamera.worldToCameraMatrix)) * clipPlaneWorldSpace;
        //var newMatrix = mainCamera.CalculateObliqueMatrix(clipPlaneCameraSpace);
        //cloneCamera.projectionMatrix = newMatrix;
    }
}