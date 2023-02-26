using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class VisualizerSettings : MonoBehaviour
{
    public bool showHints;
    public float hintSize;
    public string numberFormat;
    public UnityEvent onChange;

    private void Notify(UnityEvent onChange)
    {
        onChange.Invoke();
    }
    void Start()
    {
        Notify(onChange);
    }

    void Update()
    {
        
    }
    private void OnValidate()
    {
        Notify(onChange);
    }
}
