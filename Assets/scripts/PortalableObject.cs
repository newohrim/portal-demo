using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalableObject : MonoBehaviour, IPortalable
{
    private readonly static Vector3 spawnPoint = new Vector3(5.0f, 10.0f, -5.0f);

    [SerializeField]
    private Transform rootTransform;
    [SerializeField]
    private Transform targetTransform;
    [SerializeField]
    private MeshRenderer meshObject;
    [SerializeField]
    private new Collider collider;
    [SerializeField]
    private float predictVelocityLimitter = 10.0f;
    [SerializeField]
    private float maxCappableSpeed = 100.0f;
    [SerializeField]
    private LayerMask portalLayer;
    public bool CloneSpawned { get; private set; } = false;
    private GameObject clone;
    private GameObject spawnedClone;
    private Portal inPortal;
    private Portal outPortal;

    private new Rigidbody rigidbody;

    private void Awake()
    {
        if (rootTransform == null) 
        {
            rootTransform = transform;
            rigidbody = GetComponent<Rigidbody>();
        }
        else 
        {
            rigidbody = rootTransform.GetComponent<Rigidbody>();
        }
        if (collider == null) 
        {
            collider = GetComponent<Collider>();
        }
        if (targetTransform == null) 
        {
            targetTransform = transform;
        }
        if (meshObject == null) 
        {
            meshObject = GetComponent<MeshRenderer>();
        }
    }

    private void FixedUpdate()
    {
        if (transform.position.y < -10.0f) 
        {
            transform.position = spawnPoint;
        }
        ClampVelocity();
        PredictCollision();
    }

    private void LateUpdate()
    {
        UpdateCloneTransform();
        if (HasClone() && clone.activeSelf) 
        {
            Vector3 relativePos = inPortal.transform.InverseTransformPoint(targetTransform.position);
            if (relativePos.z < 0.0f) 
            {
                WarpToPortal();
                UpdateCloneTransform();
            }
        }
    }

    private void UpdateCloneTransform()
    {
        if (HasClone() && clone.activeSelf)     
        {
            clone.transform.position = inPortal.GetRelativePosition(outPortal.transform, rootTransform.position);
            clone.transform.rotation = inPortal.GetRelativeRotation(outPortal.transform, rootTransform.rotation);
            clone.transform.localScale = rootTransform.localScale;
        }
    }

    private void PredictCollision()
    {
        RaycastHit hitInfo;
        if (rigidbody.velocity.magnitude >= predictVelocityLimitter && 
            Physics.Raycast(rootTransform.position, rigidbody.velocity, out hitInfo, 20.0f, portalLayer, QueryTriggerInteraction.Collide))
        {
            Portal hitPortal = hitInfo.collider.GetComponent<Portal>();
            if (hitPortal != null) 
            {
                Debug.Log("PREDICTION HAPPENED");
                hitPortal.IgnoreCollisionWith(collider, true);
            }
        }
    }

    private void ClampVelocity()
    {
        if (rigidbody.velocity.magnitude > maxCappableSpeed) 
        {
            rigidbody.velocity = Vector3.ClampMagnitude(rigidbody.velocity, maxCappableSpeed);
        }
    }

    public GameObject Clone(Portal inPortal, Portal outPortal, Collider wallCollider)
    {
        Debug.Log("CLONED");
        //Physics.IgnoreCollision(collider, wallCollider, true);
        inPortal.IgnoreCollisionWith(collider, true);

        this.inPortal = inPortal;
        this.outPortal = outPortal;

        clone = new GameObject();
        clone.SetActive(false);
        MeshFilter meshFilter = clone.AddComponent<MeshFilter>();
        MeshRenderer renderer = clone.AddComponent<MeshRenderer>();
        meshFilter.mesh = GetComponent<MeshFilter>().mesh;
        renderer.materials = GetComponent<MeshRenderer>().materials;
        clone.transform.localScale = rootTransform.localScale;

        //rigidbody = GetComponent<Rigidbody>();
        //collider = GetComponent<Collider>();

        return clone;
    }

    public bool HasClone()
    {
        return clone != null;
    }

    public void ExitPortal(Collider wallCollider)
    {
        //Physics.IgnoreCollision(collider, wallCollider, false);
        inPortal.IgnoreCollisionWith(collider, false);

        inPortal = null;
        outPortal = null;
        //GameObject.Destroy(spawnedClone);
        GameObject.Destroy(clone);
        Debug.Log("destroyed clone");
        clone = null;
        CloneSpawned = false;
    }

    public bool IsInPortal(Portal portal)
    {
        return portal == inPortal;
    }

    public bool IsBehindPortal(Portal portal)
    {
        return portal.transform.InverseTransformPoint(transform.position).z < 0.0f;
    }

    public void WarpToPortal()
    {
        Debug.Log("WARPED");
        //rootTransform.position = clone.transform.position;
        //rootTransform.rotation = clone.transform.rotation;
        rootTransform.position = inPortal.GetRelativePosition(outPortal.transform, rootTransform.position);
        rootTransform.rotation = inPortal.GetRelativeRotation(outPortal.transform, rootTransform.rotation);
        Vector3 relativeVelocity = inPortal.transform.InverseTransformDirection(rigidbody.velocity);
        relativeVelocity = Quaternion.Euler(0.0f, 180.0f, 0.0f) * relativeVelocity;
        rigidbody.velocity = outPortal.transform.TransformDirection(relativeVelocity);
        inPortal.IgnoreCollisionWith(collider, false);
        SwapPortals(); // swaps portal references
    }

    public void SwapPortals()
    {
        Portal tmp = inPortal;
        inPortal = outPortal;
        outPortal = tmp;
    }
}
