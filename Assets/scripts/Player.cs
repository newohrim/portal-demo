using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    // temp for config
    public static int rec_iter = 7;

    private const string HORIZONTAL_AXIS_NAME = "Horizontal";
    private const string VERTICAL_AXIS_NAME = "Vertical";
    private const float MAX_CAMERA_ROTATION_Y = 85.0f;
    private const float MIN_CAMERA_ROTATION_Y = -85.0f;
    private const float MAX_RAY_DISTANCE = 100.0f;
    private const float MIN_PORTAL_DISTANCE = 1.0f;

    [Header("Movement")]
    [SerializeField]
    private InputSystem userInput;
    [SerializeField]
    private float playerSpeed = 5.0f;
    [SerializeField]
    private float pushForce = 5.0f;
    [SerializeField]
    private float jumpForce = 100.0f;

    [Header("Portals")]
    [SerializeField]
    private Portal portalPrefab;
    [SerializeField]
    private GameObject portalCameraPrefab;
    [SerializeField]
    private PlacementObject placementPrefab;

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
    private bool portableObjectLifted = false;
    private PlacementObject placementObject;
    private Vector3 inputVector;
    private bool readyToLiftPortable = false;
    private RaycastHit currentHit;
    private bool hitHappened = false;

    private bool CanJump
    {
        get => Mathf.Abs(rig.velocity.y) < 0.1f;
    }

    private void Start ()
    {
        ReadConfig();
    }

    private void Awake()
    {
        // USER INPUT
        userInput = new InputSystem();
        userInput.Player.PushObject.performed += context => PushObject();
        userInput.Player.TakePortable.canceled += context => TakePortable();
        userInput.Player.SpawnFirstPortal.started += context => SpawnFirstPortal(context);
        userInput.Player.SpawnSecondPortal.started += context => SpawnSecondPortal(context);
        userInput.Player.Jump.performed += context => Jump();
        // COMPONENTS INIT
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
        CameraProceed();
    }

    private void FixedUpdate()
    {
        UpdateInputVector();
        Move();
    }

    private void OnEnable()
    {
        userInput.Enable();
    }

    private void OnDisable() 
    {
        userInput.Disable();
    }

    private void UpdateInputVector()
    {
        float horizontal = userInput.Player.MoveX.ReadValue<float>();
        float vertical = userInput.Player.MoveZ.ReadValue<float>();
        inputVector = new Vector3(
            horizontal, 
            0, 
            vertical);
    }

    private void Move()
    {
        Quaternion forwardDirection = mainCamera.transform.rotation;
        forwardDirection = Quaternion.Euler(0.0f, forwardDirection.eulerAngles.y, 0.0f);
        inputVector = forwardDirection * inputVector;
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
        cameraRotation.x += userInput.Player.CameraVertical.ReadValue<float>() * mouseSensetivityX * Time.deltaTime;
        cameraRotation.y += userInput.Player.CameraHorizontal.ReadValue<float>() * mouseSensetivityY * Time.deltaTime;
        transform.rotation = Quaternion.Euler(0, cameraRotation.y, 0);
        mainCamera.transform.rotation = Quaternion.Euler(
            Mathf.Clamp(-cameraRotation.x, MIN_CAMERA_ROTATION_Y, MAX_CAMERA_ROTATION_Y), 
            cameraRotation.y, 
            0);
    }

    private void RayProceed()
    {
        if (RaycastFromCamera())
        {
            hitHappened = true;
            if (!isPortableState && currentHit.transform.gameObject.CompareTag("LevelMesh") && currentHit.distance >= MIN_PORTAL_DISTANCE) 
            {
                ProceedWallHit(currentHit);
            }
            else 
            {
                DestroyPlacementObject();
            }
        }
        else 
        {
            DestroyPlacementObject();
            hitHappened = false;
        }
    }

    private void DestroyPlacementObject()
    {
        if (placementObject != null) 
        {
            Destroy(placementObject.gameObject);
            placementObject = null;
        }
    }

    private bool RaycastFromCamera()
    {
        return Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out currentHit, MAX_RAY_DISTANCE);
    }

    private void ProceedWallHit(RaycastHit hit)
    {
        Vector3 pos;
        Quaternion rot;
        GetSpawnPosition(hit.point, hit.normal, 0.05f, out pos, out rot);
        if (placementObject == null) 
        {
            placementObject = Instantiate<PlacementObject>(placementPrefab, pos, rot);
        }
        else 
        {
            placementObject.transform.position = pos;
            placementObject.transform.rotation = rot;
        }
    }

    private void SpawnPortalProceed(RaycastHit hit, int portalId)
    {
        if (!isPortableState && placementObject != null && placementObject.PlacementPossible) 
        {
            if (!portals[portalId].IsPlaced) 
            {
                SpawnPortal(hit.point, hit.normal, hit.collider, out portals[portalId]);
                CheckConnectionCondition();
            }
            else
            {
                MovePortal(hit.point, hit.normal, hit.collider, portals[portalId]);
            }
        }
    }

    private void SpawnPortal(Vector3 hitPoint, Vector3 normal, Collider wallCollider, out Portal spawnedPortal)
    {
        Vector3 upwardVec = Vector3.ProjectOnPlane(mainCamera.transform.forward, normal);
        Quaternion portalRotation = Quaternion.LookRotation(normal, Vector3.up);
        Vector3 offsetToPlayer = (transform.position - hitPoint).normalized * 0.01f;
        spawnedPortal = Instantiate<Portal>(portalPrefab, hitPoint + offsetToPlayer, portalRotation) as Portal;
        spawnedPortal.gameObject.name = Random.Range(0, 100).ToString();
        spawnedPortal.SetWallCollider(wallCollider);
    }

    private void MovePortal(Vector3 hitPoint, Vector3 normal, Collider wallCollider, Portal movingPortal)
    {
        Quaternion portalRotation = Quaternion.LookRotation(normal, Vector3.up);
        Vector3 offsetToPlayer = (transform.position - hitPoint).normalized * 0.01f;
        movingPortal.transform.position = hitPoint + offsetToPlayer;
        movingPortal.transform.rotation = portalRotation;
        movingPortal.SetWallCollider(wallCollider);
    }

    private void GetSpawnPosition(Vector3 hitPoint, Vector3 normal, float offset, out Vector3 pos, out Quaternion rot)
    {
        rot = Quaternion.LookRotation(normal, Vector3.up);
        //Vector3 offsetToPlayer = (transform.position - hitPoint).normalized * offset;
        Vector3 offsetToPlayer = normal * offset;
        pos = hitPoint + offsetToPlayer;
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

    private void PushObject()
    {
        if (isPortableState) 
        {
            portableObject.EndPortable();
            portableObject.PushObject(pushForce);
            portableObject = null;
            isPortableState = false;
        }
    }

    private void TakePortable()
    {
        if (isPortableState) 
        {
            portableObject.EndPortable();
            isPortableState = false;
            portableObject = null;
        }
        else 
        {
            if (!hitHappened) 
            {
                // proceed raycast
                if (!RaycastFromCamera()) 
                {
                    return;
                }
            }
            IPortable portable = currentHit.collider.GetComponent<IPortable>();
            if (portable != null) 
            {
                if (!isPortableState) 
                {
                    isPortableState = true;
                    portableObject = portable;
                    portable.GoPortable(mainCamera.transform);
                }
            }
        }
    }

    private void SpawnFirstPortal(InputAction.CallbackContext context)
    {
        if (hitHappened && currentHit.collider.CompareTag("LevelMesh")) 
        {
            SpawnPortalProceed(currentHit, 0);
        }
    }

    private void SpawnSecondPortal(InputAction.CallbackContext context)
    {
        if (hitHappened && currentHit.collider.CompareTag("LevelMesh")) 
        {
            SpawnPortalProceed(currentHit, 1);
        }
    }

    private void Jump()
    {
        if (CanJump) 
        {
            rig.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void ReadConfig()
    {
        try 
        {
            using(StreamReader sr = new StreamReader("config.cfg"))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    string[] parts = line.Split();
                    switch (parts[0]) 
                    {
                        case "mouse_sensetivity":
                            int value = int.Parse(parts[1]);
                            if (value <= 0) 
                            {
                                Debug.LogError("Mouse sensetivity value must be more then zero.");
                            }
                            else 
                            {
                                mouseSensetivityX = value;
                                mouseSensetivityY = value;
                            }
                            break;
                        case "recursive_iterations":
                            int val = int.Parse(parts[1]);
                            if (val <= 0) 
                            {
                                Debug.LogError("Recursive iterations count must be more then zero.");
                            }
                            else 
                            {
                                rec_iter = val;
                            }
                            break;
                        default:
                            Debug.LogWarning("No such param in list.");
                            break;
                    }
                }
            }
        }
        catch (IOException e)
        {
            Debug.LogError("CONFIG ERROR: " + e.Message);
        }
        catch (System.Exception e) 
        {
            Debug.LogError(e.Message);
        }
    }
}
