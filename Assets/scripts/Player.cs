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

    [Header("Portals")]
    [SerializeField]
    private GameObject portalPrefab;
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
    private GameObject[] portals;
    private int temp = 0;
    
    private void Start()
    {
        rig = GetComponent<Rigidbody>();
        portals = new GameObject[2];
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        cameraRotation = new Vector2(mainCamera.transform.rotation.eulerAngles.x, 
            mainCamera.transform.rotation.eulerAngles.y);
    }

    private void Update()
    {
        RayProceed();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void LateUpdate()
    {
        CameraProceed();
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
        cameraRotation.x += Input.GetAxis("Mouse Y") * mouseSensetivityX;
        cameraRotation.y += Input.GetAxis("Mouse X") * mouseSensetivityY;
        transform.rotation = Quaternion.Euler(0, cameraRotation.y, 0);
        mainCamera.transform.rotation = Quaternion.Euler(-cameraRotation.x, cameraRotation.y, 0);
    }

    private void RayProceed()
    {
        RaycastHit hit;
        if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit, MAX_RAY_DISTANCE))
        {
            if (hit.transform.gameObject.CompareTag("LevelMesh")) 
            {
                ProceedWallHit(hit);
            }
        }
    }

    private void ProceedWallHit(RaycastHit hit)
    {
        if (Input.GetKeyDown(KeyCode.Mouse0)) 
        {
            SpawnPortal(hit.point, hit.normal);
        }
    }

    private void SpawnPortal(Vector3 hitPoint, Vector3 normal)
    {
        Vector3 upwardVec = Vector3.ProjectOnPlane(mainCamera.transform.forward, normal);
        Quaternion portalRotation = Quaternion.LookRotation(normal, Vector3.up);
        Vector3 offsetToPlayer = (transform.position - hitPoint).normalized * 0.01f;
        portals[temp] = Instantiate(portalPrefab, hitPoint + offsetToPlayer, portalRotation);
        Portal spawnedPortal = portals[temp].GetComponent<Portal>();
        if (portals[1] != null) 
        {
            Portal.ConnectPortals(portals[0].GetComponent<Portal>(), portals[1].GetComponent<Portal>(), portalCameraPrefab, this);
        }
        temp = (temp + 1) & 1;
    }

    public Camera GetPlayersCamera() 
    {
        return mainCamera;
    }
}
