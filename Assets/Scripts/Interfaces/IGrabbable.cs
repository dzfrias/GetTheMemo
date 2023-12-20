using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGrabbable
{
    void Pickup(Transform holdTransform);
    void Drop();
}
