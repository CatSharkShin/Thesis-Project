using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRReSnapGrabbable : XRGrabInteractable
{
    /*
    private Vector3 _savedLocalPosition;
    protected override void OnSelectEntering(SelectEnterEventArgs args)
    {
        base.OnSelectEntering(args);
        _savedLocalPosition = transform.localPosition;
    }
    protected override void OnSelectExiting(SelectExitEventArgs args)
    {
        base.OnSelectExiting(args);
        StartCoroutine(SmoothReAttach());
    }
    IEnumerator SmoothReAttach()
    {
        while (transform.localPosition == _savedLocalPosition)
            transform.localPosition = Vector3.Lerp(transform.localPosition, _savedLocalPosition,Time.deltaTime*0.01f);
        yield return new WaitForEndOfFrame();
    }*/
}
