using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.XR.Interaction.Toolkit;

public class ScaleOnGrab : MonoBehaviour
{
    public Vector3 offScale;
    public Vector3 onScale;
    public XRGrabInteractable grabbable;
    [HideInInspector]
    public bool created = false;
    private Coroutine cor;
    public void Start()
    {
        if (!created)
        {
            grabbable.selectEntered.AddListener(Selected);
            grabbable.selectExited.AddListener(UnSelected);
        }
    }
    public void Selected(SelectEnterEventArgs args)
    {
        if (cor != null)
            StopCoroutine(cor);
        cor = StartCoroutine(ScaleTo(onScale));
    }
    public void UnSelected(SelectExitEventArgs args)
    {
        if (cor != null)
            StopCoroutine(cor);
        cor = StartCoroutine(ScaleTo(offScale));
    }
    IEnumerator ScaleTo(Vector3 target)
    {
        while (transform.localScale != target)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, target, Time.deltaTime);
            yield return null;
        }
    }
    public static ScaleOnGrab Create(GameObject gameObject, Vector3 offScale, Vector3 onScale, XRGrabInteractable grabbable)
    {
        ScaleOnGrab sog = gameObject.AddComponent<ScaleOnGrab>();
        gameObject.transform.localScale = Vector3.zero;
        sog.grabbable = grabbable;
        sog.offScale = offScale;
        sog.onScale = onScale;
        sog.created = true;
        grabbable.selectEntered.AddListener(sog.Selected);
        grabbable.selectExited.AddListener(sog.UnSelected);
        return sog;
    }
}
