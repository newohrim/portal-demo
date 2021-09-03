using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using RenderPipeline = UnityEngine.Rendering.RenderPipelineManager;

public class PortalCamera : MonoBehaviour
{
    private static readonly Quaternion halfTurn = Quaternion.Euler(0.0f, 180.0f, 0.0f);

    //[SerializeField]
    private int recursiveIterations = 7;

    private Camera portalCamera;
    private Camera mainCamera;
    private Portal[] connectedPortals;
    private RenderTexture[] renderTextures;

    private void Awake()
    {
        recursiveIterations = Player.rec_iter;
        connectedPortals = new Portal[2];
        renderTextures = new RenderTexture[2] 
        {
            new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.ARGB32),
            new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.ARGB32)
        };
        //portalCamera = GetComponent<Camera>();    
        mainCamera = GetComponent<Camera>();
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
        if (cam != mainCamera)
            return;
        if (connectedPortals[0] != null && connectedPortals[1] != null) 
        {
            if (connectedPortals[0].IsVisible)
            {
                portalCamera.targetTexture = renderTextures[0];
                for (int i = recursiveIterations - 1; i >= 0; --i) 
                {
                    RenderCamera(connectedPortals[0], connectedPortals[1], i, SRC);
                }
            }
            if (connectedPortals[1].IsVisible) 
            {
                portalCamera.targetTexture = renderTextures[1];
                for (int i = recursiveIterations - 1; i >= 0; --i) 
                {
                    RenderCamera(connectedPortals[1], connectedPortals[0], i, SRC);
                }
            }
        }
    }

    private void RenderCamera(Portal inPortal, Portal outPortal, int iteration, ScriptableRenderContext SRC)
    {
        Transform cameraTransform = portalCamera.transform;
        cameraTransform.position = transform.position;
        cameraTransform.rotation = transform.rotation;

        for (int i = 0; i <= iteration; ++i) 
        {
            // POS
            cameraTransform.position = inPortal.GetRelativePosition(outPortal.transform, cameraTransform.position);
            // ROT
            cameraTransform.rotation = inPortal.GetRelativeRotation(outPortal.transform, cameraTransform.rotation);
        }

        // OBLIQUE PLANE (reverse plane or far plane swpap bug)
        Plane obliquePlane = new Plane(outPortal.transform.forward, outPortal.transform.position);
        Vector4 clipPlaneWorldSpace = new Vector4(obliquePlane.normal.x, obliquePlane.normal.y, obliquePlane.normal.z, obliquePlane.distance);
        Vector4 clipPlaneCameraSpace = Matrix4x4.Transpose(Matrix4x4.Inverse(portalCamera.worldToCameraMatrix)) * clipPlaneWorldSpace;
        Matrix4x4 mat = portalCamera.CalculateObliqueMatrix(clipPlaneCameraSpace);
        portalCamera.projectionMatrix = mat;

        // RENDER TO TEXTURE CALL
        UniversalRenderPipeline.RenderSingleCamera(SRC, portalCamera);
        portalCamera.ResetProjectionMatrix();
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
