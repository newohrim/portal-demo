using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    private static readonly Quaternion halfTurn = Quaternion.Euler(0.0f, 180.0f, 0.0f);

    public bool IsPlaced { get; private set; } = false;

    [field: SerializeField]
    public Renderer PortalMesh { get; set; }
    [SerializeField]
    private GameObject holders;

    private PortalCamera linkedCamera;
    private Portal otherPortal;
    private Collider wallCollider;
    private List<IPortalable> currentInstances;

    private void Awake()
    {
        IsPlaced = true;
        currentInstances = new List<IPortalable>();
    }

    public void LinkCamera(PortalCamera pCam)
    {
        linkedCamera = pCam;
    }

    public Vector3 GetRelativePosition(Transform outTransform, Vector3 viewerPos)
    {
        Vector3 relativePos = transform.InverseTransformPoint(viewerPos);
        relativePos = halfTurn * relativePos;
        return outTransform.TransformPoint(relativePos);
    }

    public Quaternion GetRelativeRotation(Transform outTransform, Quaternion viewerRot)
    {
        Quaternion relativeRot = Quaternion.Inverse(transform.rotation) * viewerRot;
        relativeRot = halfTurn * relativeRot;
        return outTransform.rotation * relativeRot;
    }

    public static void ConnectPortals(Portal p1, Portal p2, GameObject portalCamera, Player player)
    {
        GameObject pCam = Instantiate(portalCamera, Vector3.zero, Quaternion.identity);
        //PortalCamera pCamComponent = pCam.GetComponent<PortalCamera>();
        PortalCamera pCamComponent = player.GetPlayersCamera().gameObject.AddComponent<PortalCamera>();
        p1.LinkCamera(pCamComponent);
        p2.LinkCamera(pCamComponent);
        p1.otherPortal = p2;
        p2.otherPortal = p1;
        pCamComponent.SetPortals(p1, p2);
        pCamComponent.SetPortalCamera(pCam.GetComponent<Camera>());
    }

    public void SetWallCollider(Collider wallCollider)
    {
        this.wallCollider = wallCollider;
    }

    private void OnTriggerEnter(Collider col)
    {
        IPortalable portalable = col.GetComponent<IPortalable>();
        if (portalable != null) 
        {
            currentInstances.Add(portalable);
            if (holders != null && !holders.activeSelf)
                holders.SetActive(true);
            if (!portalable.HasClone()) 
            {
                portalable.Clone(this, otherPortal, wallCollider).SetActive(true);
            }
        }
    }   

    private void OnTriggerExit(Collider col)
    {
        IPortalable portalable = col.GetComponent<IPortalable>();
        if (portalable != null) 
        {
            currentInstances.Remove(portalable);
            if (currentInstances.Count == 0 && holders != null) 
            {
                holders.SetActive(false);
            }
            if (portalable.HasClone()) 
            {
                portalable.ExitPortal(wallCollider);
            }
        }
    }
}
