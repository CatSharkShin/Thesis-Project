using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EdgeRenderer : MonoBehaviour
{
    public Transform A;
    public Transform B;
    public float width = 0.005f;
    private LineRenderer lineRenderer;
    void Start()
    {
        lineRenderer = this.AddComponent<LineRenderer>();
        lineRenderer.startWidth = width;
        lineRenderer.endWidth = width;
        lineRenderer.material = Resources.Load<Material>("Materials/Edge");
    }
    void Update()
    {
        if (A == null || B == null || A == B)
        {
            GameObject.Destroy(this);
        }
        else
        {
            lineRenderer.SetPosition(0, A.transform.position);
            lineRenderer.SetPosition(1, B.transform.position);
        }      
    }
}
