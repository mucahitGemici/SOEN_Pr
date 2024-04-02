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

    public XRInteractorLineVisual lr_left;
    public XRInteractorLineVisual lr_right;
    public XRInteractorLineVisual lr_left_teleport;
    public XRInteractorLineVisual lr_right_teleport;
    public MeshRenderer[] meshRenderers;
    public Color highlightedColor;
    private Color normalColor;
    private bool isHolding = false;


    private void Start()
    {
        normalColor = meshRenderers[0].material.color;
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
        //meshRenderer.material.color = highlightedColor;
        setHighlightedColor();
        lr_left.enabled = false;
        lr_right.enabled = false;
        lr_left_teleport.enabled = false;
        lr_right_teleport.enabled = false;
    }

    private void DeHighlightObject()
    {
        //meshRenderer.material.color = normalColor;
        setNormalColor();
        lr_left.enabled = true;
        lr_right.enabled = true;
        lr_left_teleport.enabled = true;
        lr_right_teleport.enabled = true;
    }

    private void setHighlightedColor()
    {
        foreach (MeshRenderer renderer in meshRenderers)
        {
            renderer.material.color = highlightedColor;
        }
    }

    private void setNormalColor()
    {
        foreach (MeshRenderer renderer in meshRenderers)
        {
            renderer.material.color = normalColor;
        }
    }

    public void setHitColor(MeshRenderer renderer)
    {
        renderer.material.color = Color.red;
        StartCoroutine(ResetColor(renderer));
    }

    private IEnumerator ResetColor(MeshRenderer renderer)
    {
        yield return new WaitForSeconds(1f);
        if (isHolding)
        {
            renderer.material.color = highlightedColor;
        }
        else
        {
            renderer.material.color = normalColor;
        }
    }


}
