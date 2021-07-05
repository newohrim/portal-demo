using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortableObject : MonoBehaviour, IPortable
{
    private const float MAX_LIFT_Y = 2.5f;
    private const float MIN_LIFT_Y = -2.5f;

    [SerializeField]
    [Range(0.0f, 1.0f)]
    private float portableStateAlpha = 0.5f;
    [SerializeField]
    private float distanceFromCamera = 0.5f;

    public event System.Action OnPortableUpdate;
    private Quaternion initRot;
    private Color initColor;
    private new Collider collider;
    private new Renderer renderer;
    private new Rigidbody rigidbody;
    private Transform mainCamera;

    private void Awake()
    {
        collider = GetComponent<Collider>();
        renderer = GetComponent<Renderer>();
        rigidbody = GetComponent<Rigidbody>();
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
        Color newColor = initColor;
        newColor.a = portableStateAlpha;
        renderer.material.color = newColor;
        collider.enabled = false;
        rigidbody.useGravity = false;
        OnPortableUpdate += UpdatePos;
    }

    public void EndPortable()
    {
        collider.enabled = true;
        renderer.material.color = initColor;
        rigidbody.useGravity = true;
        OnPortableUpdate -= UpdatePos;
    }

    public void PushObject(float force) 
    {
        rigidbody.AddForce(mainCamera.forward * force, ForceMode.Force);
    }

    private void UpdatePos()
    {
        Vector3 temp = new Vector3(0.0f, 0.0f, distanceFromCamera);
        Quaternion yRot = Quaternion.Euler(0.0f, mainCamera.rotation.eulerAngles.y, 0.0f);
        temp = yRot * temp;
        float cosVal = Mathf.Cos(Mathf.Deg2Rad * Vector3.Angle(mainCamera.forward, temp));
        if (cosVal == 0.0f) 
        {
            return;
        }
        float offsetMag = distanceFromCamera / cosVal;
        float y = offsetMag * offsetMag - temp.x * temp.x - temp.z * temp.z;
        if (y < 0.0f) 
        {
            y = 0.0f;
        }
        temp.y = Mathf.Clamp(Mathf.Sqrt(y), MIN_LIFT_Y, MAX_LIFT_Y);
        if (mainCamera.forward.y < 0.0f)
            temp.y = -temp.y;
        transform.position = mainCamera.position + temp;
        transform.rotation = initRot;
    }

    private void OnCollisionEnter(Collision col)
    {
        Debug.Log(col.gameObject.name);
    }
}
