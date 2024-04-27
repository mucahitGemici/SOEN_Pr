using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartLine : MonoBehaviour
{
    public XROffsetGrabInteractable xrOffsetGrabInteractable;

    private void OnTriggerEnter(Collider other)
    {
        xrOffsetGrabInteractable.StartTimer();
    }
}
