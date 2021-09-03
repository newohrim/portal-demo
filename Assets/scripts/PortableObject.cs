using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortableObject : MonoBehaviour, IPortable
{
    private const float MAX_LIFT_Y = 2.5f;
    private const float MIN_LIFT_Y = -2.5f;
    private const int IGNORE_RAYCAST_LAYER = 2;

    [SerializeField]
    [Range(0.0f, 1.0f)]
    private float portableStateAlpha = 0.5f;
    [SerializeField]
    private float distanceFromCamera = 0.5f;
    [SerializeField]
    private float offsetFromWall = 0.15f;

    public event System.Action OnPortableUpdate;
    private float max_camera_distance;
    private Quaternion initRot;
    private Color initColor;
    private int initLayer;
    private new Collider collider;
    private new Renderer renderer;
    private new Rigidbody rigidbody;
    private Transform mainCamera;

    private void Awake()
    {
        collider = GetComponent<Collider>();
        renderer = GetComponent<Renderer>();
        rigidbody = GetComponent<Rigidbody>();
        // hypotenuse in vertical LIFT_Y and horizontal distanceFromCamera triangle
        max_camera_distance = Mathf.Sqrt(
            Mathf.Pow(Mathf.Max(Mathf.Abs(MAX_LIFT_Y), Mathf.Abs(MIN_LIFT_Y)), 2) 
            + distanceFromCamera * distanceFromCamera
        );
    }

    private void Update()
    {
        OnPortableUpdate?.Invoke();
    }

    public void GoPortable(Transform mainCamera)
    {
        this.mainCamera = mainCamera;
        initRot = transform.rotation;
        initColor = renderer.material.color;
        initLayer = gameObject.layer;
        Color newColor = initColor;
        newColor.a = portableStateAlpha;
        renderer.material.color = newColor;
        //collider.enabled = false;
        rigidbody.useGravity = false;
        gameObject.layer = IGNORE_RAYCAST_LAYER;
        OnPortableUpdate += UpdatePos;
    }

    public void EndPortable()
    {
        collider.enabled = true;
        renderer.material.color = initColor;
        rigidbody.useGravity = true;
        gameObject.layer = initLayer;
        OnPortableUpdate -= UpdatePos;
    }

    public void PushObject(float force) 
    {
        rigidbody.AddForce(mainCamera.forward * force, ForceMode.Force);
    }

    private void UpdatePos()
    {
        rigidbody.velocity = Vector3.zero;
        float distance = distanceFromCamera;
        Vector3 normalOffset = Vector3.zero;
        RaycastHit hitInfo;
        // better use offsetMag instead of distanceFromCamera
        if (Physics.Raycast(mainCamera.transform.position, mainCamera.forward, out hitInfo, max_camera_distance)) 
        {
            if (hitInfo.distance < distanceFromCamera) 
            {
                //distance = hitInfo.distance;
                float liftY = hitInfo.point.y - mainCamera.transform.position.y;
                float sqrDistance = hitInfo.distance * hitInfo.distance - liftY * liftY;
                if (sqrDistance < 0.0f)
                    sqrDistance = 0.0f;
                distance = Mathf.Sqrt(sqrDistance);
                normalOffset = hitInfo.normal * offsetFromWall;
            }
        }
        Vector3 temp = new Vector3(0.0f, 0.0f, distance);
        Quaternion yRot = Quaternion.Euler(0.0f, mainCamera.rotation.eulerAngles.y, 0.0f);
        temp = yRot * temp;
        float cosVal = Mathf.Cos(Mathf.Deg2Rad * Vector3.Angle(mainCamera.forward, temp));
        if (cosVal == 0.0f) 
        {
            return;
        }
        float offsetMag = distance / cosVal;
        float y = offsetMag * offsetMag - temp.x * temp.x - temp.z * temp.z;
        if (y < 0.0f) 
        {
            y = 0.0f;
        }
        temp.y = Mathf.Clamp(Mathf.Sqrt(y), MIN_LIFT_Y, MAX_LIFT_Y);
        if (mainCamera.forward.y < 0.0f)
            temp.y = -temp.y;
        //temp.y += verticalOffset;
        //transform.position = mainCamera.position + temp;
        rigidbody.MovePosition(mainCamera.position + temp + normalOffset);
        transform.rotation = initRot;
    }

    //private void OnCollisionEnter(Collision col)
    //{
    //    Debug.Log(col.gameObject.name);
    //}
}
