using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPortalable 
{
    bool HasClone();
    GameObject Clone(Portal inPortal, Portal outPortal, Collider wallCollider);
    void ExitPortal(Collider wallCollider);
    void WarpToPortal();
}
