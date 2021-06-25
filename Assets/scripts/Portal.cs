using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public bool isPlaced { get; private set; } = false;

    [field: SerializeField]
    public Renderer PortalMesh { get; set; }

    private PortalCamera linkedCamera;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void LinkCamera(PortalCamera pCam)
    {
        linkedCamera = pCam;
    }

    public static void ConnectPortals(Portal p1, Portal p2, GameObject portalCamera, Player player)
    {
        GameObject pCam = Instantiate(portalCamera, Vector3.zero, Quaternion.identity);
        //PortalCamera pCamComponent = pCam.GetComponent<PortalCamera>();
        PortalCamera pCamComponent = player.GetPlayersCamera().gameObject.AddComponent<PortalCamera>();
        p1.LinkCamera(pCamComponent);
        p2.LinkCamera(pCamComponent);
        pCamComponent.SetPortals(p1, p2);
        pCamComponent.SetPortalCamera(pCam.GetComponent<Camera>());
    }
}
