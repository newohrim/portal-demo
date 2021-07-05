using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPortable
{
    event System.Action OnPortableUpdate;
    void GoPortable(Transform player);
    void EndPortable();
    void PushObject(float force);
}
