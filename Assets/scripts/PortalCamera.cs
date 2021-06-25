using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using RenderPipeline = UnityEngine.Rendering.RenderPipelineManager;

public class PortalCamera : MonoBehaviour
{
    private static readonly Quaternion halfTurn = Quaternion.Euler(0.0f, 180.0f, 0.0f);

    private Camera portalCamera;
    private Camera mainCamera;
    private Portal[] connectedPortals;
    private RenderTexture[] renderTextures;

    private void Awake()
    {
        connectedPortals = new Portal[2];
        renderTextures = new RenderTexture[2] 
        {
            new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.ARGB32),
            new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.ARGB32)
        };
        //portalCamera = GetComponent<Camera>();
        mainCamera = GetComponent<Camera>();
    }

    private void LateUpdate()
    {
        //UpdateRelativePosition();
        //GenerateObliqueMatrix();
    }

    private void OnEnable()
    {
        Debug.Log("enabled");
        RenderPipeline.beginCameraRendering += UpdateCamera;
    }

    private void OnDisable()
    {
        Debug.Log("disabled");
        RenderPipeline.beginCameraRendering -= UpdateCamera;
    }

    private void UpdateCamera(ScriptableRenderContext SRC, Camera cam)
    {
        //Debug.Log(cam.gameObject.name);
        Debug.Log(cam.gameObject.name);
        if (connectedPortals[0] != null && connectedPortals[1] != null) 
        {
            //Debug.Log("updating");
            portalCamera.targetTexture = renderTextures[0];
            RenderCamera(connectedPortals[0], connectedPortals[1], SRC);
            portalCamera.targetTexture = renderTextures[1];
            RenderCamera(connectedPortals[1], connectedPortals[0], SRC);
        }
    }

    private void RenderCamera(Portal inPortal, Portal outPortal, ScriptableRenderContext SRC)
    {
        // POS
        Transform inTransform = inPortal.transform;
        Transform outTransform = outPortal.transform;

        Transform cameraTransform = portalCamera.transform;
        cameraTransform.position = transform.position;
        cameraTransform.rotation = transform.rotation;

        Vector3 relativePos = inTransform.InverseTransformPoint(cameraTransform.position);
        relativePos = halfTurn * relativePos;
        cameraTransform.position = outTransform.TransformPoint(relativePos);

        // Rotate the camera to look through the other portal.
        Quaternion relativeRot = Quaternion.Inverse(inTransform.rotation) * cameraTransform.rotation;
        relativeRot = halfTurn * relativeRot;
        cameraTransform.rotation = outTransform.rotation * relativeRot;

        // ROT
        //Vector3 viewDir = portalCamera.transform.parent.position - portalCamera.transform.position;
        //portalCamera.transform.rotation = Quaternion.LookRotation(viewDir);

        // OBLIQUE PLANE
        Plane obliquePlane = new Plane(outPortal.transform.forward, outPortal.transform.position);
        Vector4 clipPlaneWorldSpace = new Vector4(obliquePlane.normal.x, obliquePlane.normal.y, obliquePlane.normal.z, obliquePlane.distance);
        Vector4 clipPlaneCameraSpace = Matrix4x4.Transpose(Matrix4x4.Inverse(portalCamera.worldToCameraMatrix)) * clipPlaneWorldSpace;
        Matrix4x4 mat = portalCamera.CalculateObliqueMatrix(clipPlaneCameraSpace);
        portalCamera.projectionMatrix = mat;

        //portalCamera.transform.SetParent(null);

        // RENDER TO TEXTURE CALL
        UniversalRenderPipeline.RenderSingleCamera(SRC, portalCamera);
    }

    private void UpdateRelativePosition()
    {
        // POS
        /*
        transform.localPosition = Vector3.zero;
        transform.rotation = Quaternion.identity;
        //Vector3 fromOtherPortalToPlayerCamera = playersCamera.position - otherPortal.position;
        Vector3 fromOtherPortalToPlayerCamera = otherPortal.InverseTransformPoint(playersCamera.position);
        Debug.Log(transform.parent.gameObject.name);
        Debug.Log(fromOtherPortalToPlayerCamera);
        //fromOtherPortalToPlayerCamera = transform.InverseTransformVector(fromOtherPortalToPlayerCamera);
        //Debug.Log(fromOtherPortalToPlayerCamera);
        Vector3 relativeOffset = transform.parent.TransformPoint(fromOtherPortalToPlayerCamera);
        relativeOffset = transform.parent.InverseTransformPoint(relativeOffset);
        Debug.Log(relativeOffset);
        Debug.Log(transform.position);
        transform.localPosition -= relativeOffset;
        //Debug.Break();

        // ROT
        Vector3 viewDir = transform.parent.position - transform.position;
        //Debug.Log(transform.parent.gameObject.name + " " + viewDir);
        transform.rotation = Quaternion.LookRotation(viewDir);
        //transform.rotation.SetLookRotation(-transform.localPosition, Vector3.up);
        //Debug.Log(transform.localPosition);
        //transform.LookAt(transform.parent);*/
    }

    private void GenerateObliqueMatrix()
    {
        /*
        Plane obliquePlane = new Plane(otherPortal.forward, otherPortal.position);
        Vector4 clipPlaneWorldSpace = new Vector4(obliquePlane.normal.x, obliquePlane.normal.y, obliquePlane.normal.z, obliquePlane.distance);
        Vector4 clipPlaneCameraSpace = Matrix4x4.Transpose(Matrix4x4.Inverse(portalCamera.worldToCameraMatrix)) * clipPlaneWorldSpace;
        Matrix4x4 mat = mainCamera.CalculateObliqueMatrix(clipPlaneCameraSpace);
        portalCamera.projectionMatrix = mat;
        */
    }

    public void SetPortals(Portal p1, Portal p2)
    {
        connectedPortals[0] = p1;
        connectedPortals[1] = p2;
        connectedPortals[0].PortalMesh.material.mainTexture = renderTextures[0];
        connectedPortals[1].PortalMesh.material.mainTexture = renderTextures[1];
    }

    public void SetPortalCamera(Camera portalCamera)
    {
        this.portalCamera = portalCamera;
    }
}
