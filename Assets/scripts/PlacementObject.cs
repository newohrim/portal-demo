using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementObject : MonoBehaviour
{
    // Offset from wall surface
    private const float cornerZOffset = 2.0f;

    // If placement is possible in the current pos
    public bool PlacementPossible 
    { 
        get => triggerCheck && DoesCoverWall();
    }

    // Material to apply if test passed
    [SerializeField]
    private Material testPassMaterial;
    // Material to apply if test didn't pass
    [SerializeField]
    private Material testNotPassMaterial;
    // LayerMask of walls on scene
    [SerializeField]
    private LayerMask placementMask;
    // Corner points of this portal
    [SerializeField]
    private Transform[] cornerPoints;

    private new Renderer renderer;
    private List<Collider> triggeredObjects;
    private bool triggerCheck = true;

    private void Awake()
    {
        triggeredObjects = new List<Collider>();
        renderer = GetComponent<MeshRenderer>();
    }

    private void OnTriggerEnter(Collider col) 
    {
        if (!col.isTrigger) 
        {
            triggerCheck = false;
            renderer.material = testNotPassMaterial;
            triggeredObjects.Add(col);
        }
    }

    private void OnTriggerExit(Collider col) 
    {
        if (!col.isTrigger) 
        {
            triggeredObjects.Remove(col);
            if (triggeredObjects.Count == 0) 
            {
                triggerCheck = true;
                renderer.material = testPassMaterial;
            }
        }
    }

    // Very silly implentation of wall coverage by current portal
    private bool DoesCoverWall()
    {
        Vector3[] initPos;
        for (int i = 0; i < cornerPoints.Length - 1; ++i) 
        {
            initPos = new Vector3[2] { cornerPoints[i].position, cornerPoints[i + 1].position }; 

            cornerPoints[i].transform.position += 
                cornerPoints[i].transform.TransformVector(new Vector3(0.0f, 0.0f, cornerZOffset));
            cornerPoints[i + 1].transform.position -= 
                cornerPoints[i + 1].transform.TransformVector(new Vector3(0.0f, 0.0f, cornerZOffset)); 
            Debug.DrawLine(cornerPoints[i].position, cornerPoints[i + 1].position, Color.green);  
            if (!Physics.Linecast(cornerPoints[i].position, cornerPoints[i + 1].position, 
                placementMask, QueryTriggerInteraction.Ignore)) 
            {
                Debug.Log("false " + i + "1");
                return false;
            }
            cornerPoints[i].position = initPos[0];
            cornerPoints[i + 1].position = initPos[1];
            cornerPoints[i].transform.position -= 
                cornerPoints[i].transform.TransformVector(new Vector3(0.0f, 0.0f, cornerZOffset));
            cornerPoints[i + 1].transform.position += 
                cornerPoints[i + 1].transform.TransformVector(new Vector3(0.0f, 0.0f, cornerZOffset)); 
            Debug.DrawLine(cornerPoints[i + 1].position, cornerPoints[i].position, Color.green);  
            if (!Physics.Linecast(cornerPoints[i + 1].position, cornerPoints[i].position, 
                placementMask, QueryTriggerInteraction.Ignore)) 
            {
                Debug.Log("false " + i + "2");
                return false;
            }
            cornerPoints[i].position = initPos[0];
            cornerPoints[i + 1].position = initPos[1];
        }

        initPos = new Vector3[2] { cornerPoints[cornerPoints.Length - 1].position, cornerPoints[0].position }; 

        cornerPoints[cornerPoints.Length - 1].transform.position += 
            cornerPoints[cornerPoints.Length - 1].transform.TransformVector(new Vector3(0.0f, 0.0f, cornerZOffset));
        cornerPoints[0].transform.position -= 
            cornerPoints[0].transform.TransformVector(new Vector3(0.0f, 0.0f, cornerZOffset));  
        Debug.DrawLine(cornerPoints[cornerPoints.Length - 1].position, cornerPoints[0].position, Color.green);   
        if (!Physics.Linecast(cornerPoints[cornerPoints.Length - 1].position, cornerPoints[0].position, 
            placementMask, QueryTriggerInteraction.Ignore)) 
        {
            Debug.Log("false final 1");
            return false;
        }
        cornerPoints[cornerPoints.Length - 1].position = initPos[0];
        cornerPoints[0].position = initPos[1];
        cornerPoints[cornerPoints.Length - 1].transform.position -= 
            cornerPoints[cornerPoints.Length - 1].transform.TransformVector(new Vector3(0.0f, 0.0f, cornerZOffset));
        cornerPoints[0].transform.position += 
            cornerPoints[0].transform.TransformVector(new Vector3(0.0f, 0.0f, cornerZOffset));   
        Debug.DrawLine(cornerPoints[0].position, cornerPoints[cornerPoints.Length - 1].position, Color.green);   
        if (!Physics.Linecast(cornerPoints[0].position, cornerPoints[cornerPoints.Length - 1].position, 
            placementMask, QueryTriggerInteraction.Ignore)) 
        {
            Debug.Log("false final 2");
            return false;
        }
        cornerPoints[cornerPoints.Length - 1].position = initPos[0];
        cornerPoints[0].position = initPos[1];
        Debug.Log("true");

        return true;
    }
}
