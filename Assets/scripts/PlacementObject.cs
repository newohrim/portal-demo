using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementObject : MonoBehaviour
{
    public bool PlacementPossible { get; private set; } = true;

    [SerializeField]
    private Material testPassMaterial;
    [SerializeField]
    private Material testNotPassMaterial;

    private new Renderer renderer;
    private List<Collider> triggeredObjects;

    private void Awake()
    {
        triggeredObjects = new List<Collider>();
        renderer = GetComponent<MeshRenderer>();
    }

    private void OnTriggerEnter(Collider col) 
    {
        if (!col.isTrigger) 
        {
            PlacementPossible = false;
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
                PlacementPossible = true;
                renderer.material = testPassMaterial;
            }
        }
    }
}
