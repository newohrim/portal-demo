using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private const string HORIZONTAL_AXIS_NAME = "Horizontal";
    private const string VERTICAL_AXIS_NAME = "Vertical";
    private const float MAX_CAMERA_ROTATION_Y = 85.0f;
    private const float MIN_CAMERA_ROTATION_Y = -85.0f;
    private const float MAX_RAY_DISTANCE = 100.0f;

    [Header("Movement")]
    [SerializeField]
    private float playerSpeed = 5.0f;
    [SerializeField]
    private float pushForce = 5.0f;

    [Header("Portals")]
    [SerializeField]
    private Portal portalPrefab;
    [SerializeField]
    private GameObject portalCameraPrefab;

    [Header("Camera")]
    [SerializeField]
    private Camera mainCamera;
    [SerializeField]
    private float mouseSensetivityX = 1.0f;
    [SerializeField]
    private float mouseSensetivityY = 1.0f;
    
    private Rigidbody rig;
    private Vector2 cameraRotation;
    private Portal[] portals;
    private bool isPortableState;
    private IPortable portableObject;
    
    private void Start()
    {
        rig = GetComponent<Rigidbody>();
        portals = new Portal[2] 
        {
            new Portal(),
            new Portal()
        };
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        cameraRotation = new Vector2(mainCamera.transform.rotation.eulerAngles.x, 
            mainCamera.transform.rotation.eulerAngles.y);
    }

    private void Update()
    {
        RayProceed();
        if (Input.GetKeyDown(KeyCode.Mouse0) && isPortableState) 
        {
            isPortableState = false;
            portableObject.EndPortable();
            portableObject.PushObject(pushForce);
            portableObject = null;
        }
        CameraProceed();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void LateUpdate()
    {
        //CameraProceed();
    }

    private void Move()
    {
        Matrix4x4 mat4 = Matrix4x4.identity;
        Quaternion forwardDirection = mainCamera.transform.rotation;
        forwardDirection = Quaternion.Euler(0.0f, forwardDirection.eulerAngles.y, 0.0f);
        mat4.SetTRS(new Vector3(1.0f, 1.0f, 1.0f), forwardDirection, new Vector3(1.0f, 1.0f, 1.0f));
        Vector3 inputVector = new Vector3(
            Input.GetAxis(HORIZONTAL_AXIS_NAME), 
            0, 
            Input.GetAxis(VERTICAL_AXIS_NAME));
        inputVector = mat4.MultiplyVector(inputVector);
        rig.MovePosition(transform.position + inputVector * playerSpeed * Time.fixedDeltaTime);
    }

    private void CameraProceed()
    {
        cameraRotation.x = -mainCamera.transform.rotation.eulerAngles.x;
        cameraRotation.y = mainCamera.transform.rotation.eulerAngles.y;
        if (cameraRotation.x < 180.0f) 
        {
            cameraRotation.x += 360.0f;
        }
        if (cameraRotation.x > 180.0f) 
        {
            cameraRotation.x -= 360.0f;
        }
        cameraRotation.x += Input.GetAxis("Mouse Y") * mouseSensetivityX;
        cameraRotation.y += Input.GetAxis("Mouse X") * mouseSensetivityY;
        transform.rotation = Quaternion.Euler(0, cameraRotation.y, 0);
        mainCamera.transform.rotation = Quaternion.Euler(
            Mathf.Clamp(-cameraRotation.x, MIN_CAMERA_ROTATION_Y, MAX_CAMERA_ROTATION_Y), 
            cameraRotation.y, 
            0);
    }

    private void RayProceed()
    {
        RaycastHit hit;
        if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit, MAX_RAY_DISTANCE))
        {
            if (!isPortableState && hit.transform.gameObject.CompareTag("LevelMesh")) 
            {
                ProceedWallHit(hit);
            }
            else 
            {
                IPortable portable = hit.collider.GetComponent<IPortable>();
                if (portable != null) 
                {
                    if (Input.GetKeyUp(KeyCode.E))
                    {
                        if (!isPortableState) 
                        {
                            isPortableState = true;
                            portableObject = portable;
                            portable.GoPortable(mainCamera.transform);
                        }
                        else 
                        {
                            isPortableState = false;
                            portableObject = null;
                            portable.EndPortable();
                        }
                    }
                }
            }
        }
    }

    private void ProceedWallHit(RaycastHit hit)
    {
        if (Input.GetKeyUp(KeyCode.Mouse0)) 
        {
            if (!portals[0].IsPlaced) 
            {
                SpawnPortal(hit.point, hit.normal, hit.collider, out portals[0]);
                CheckConnectionCondition();
            }
            else
            {
                MovePortal(hit.point, hit.normal, hit.collider, portals[0]);
            }
        }
        else if (Input.GetKeyUp(KeyCode.Mouse1)) 
        {
            if (!portals[1].IsPlaced) 
            {
                SpawnPortal(hit.point, hit.normal, hit.collider, out portals[1]);
                CheckConnectionCondition();
            }
            else
            {
                MovePortal(hit.point, hit.normal, hit.collider, portals[1]);
            }
        }
    }

    private void SpawnPortal(Vector3 hitPoint, Vector3 normal, Collider wallCollider, out Portal spawnedPortal)
    {
        Vector3 upwardVec = Vector3.ProjectOnPlane(mainCamera.transform.forward, normal);
        Quaternion portalRotation = Quaternion.LookRotation(normal, Vector3.up);
        Vector3 offsetToPlayer = (transform.position - hitPoint).normalized * 0.01f;
        spawnedPortal = Instantiate<Portal>(portalPrefab, hitPoint + offsetToPlayer, portalRotation) as Portal;
        spawnedPortal.SetWallCollider(wallCollider);
    }

    private void MovePortal(Vector3 hitPoint, Vector3 normal, Collider wallCollider, Portal movingPortal)
    {
        Quaternion portalRotation = Quaternion.LookRotation(normal, Vector3.up);
        Vector3 offsetToPlayer = (transform.position - hitPoint).normalized * 0.01f;
        movingPortal.transform.position = hitPoint + offsetToPlayer;
        movingPortal.transform.rotation = portalRotation;
    }

    private void CheckConnectionCondition()
    {
        if (portals[0].IsPlaced && portals[1].IsPlaced) 
        {
            Portal.ConnectPortals(portals[0], portals[1], portalCameraPrefab, this);
        }
    }

    public Camera GetPlayersCamera() 
    {
        return mainCamera;
    }
}
