using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
public class XROffsetGrabInteractable : XRGrabInteractable
{
    private Vector3 initialLocalPos;
    private Quaternion initialLocalRot;

    public float desiredVelocity;

    public bool isRing;

    public LineRenderer lr_left;
    public LineRenderer lr_right;
    public MeshRenderer meshRenderer;
    public Color highlightedColor;
    private Color normalColor;
    private bool isHolding = false;


    private void Start()
    {
        normalColor = meshRenderer.material.color;
        if (!attachTransform)
        {
            GameObject attachPoint = new GameObject("Offset Grab Pivot");
            attachPoint.transform.SetParent(transform, false);
            attachTransform = attachPoint.transform;
        }
        else
        {
            initialLocalPos = transform.localPosition;
            initialLocalRot = transform.localRotation;

        }
    }

    private void Update()
    {
        velocityScale = desiredVelocity;

        if (isHolding)
        {

        }
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        /*
        if(args.interactorObject is XRDirectInteractor)
        {
            attachTransform.position = args.interactorObject.transform.position;
            attachTransform.rotation = args.interactorObject.transform.rotation;
        }
        else
        {
            attachTransform.localPosition = initialLocalPos;
            attachTransform.rotation = initialLocalRot;
        }
        */

        attachTransform.position = args.interactorObject.transform.position;
        attachTransform.rotation = args.interactorObject.transform.rotation;
        isHolding = true;
        HighlightObject();
        base.OnSelectEntered(args);
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        isHolding = false;
        DeHighlightObject();
        base.OnSelectExited(args);
    }

    private void HighlightObject()
    {
        meshRenderer.material.color = highlightedColor;
        lr_left.enabled = false;
        lr_right.enabled = false;
    }

    private void DeHighlightObject()
    {
        meshRenderer.material.color = normalColor;
        lr_left.enabled = true;
        lr_right.enabled = true;
    }
}
