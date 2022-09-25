using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EdgeRenderer : MonoBehaviour
{
    public Transform A;
    public Transform B;
    private LineRenderer lineRenderer;
    public NodeManager nodeManager;
    private void Awake()
    {
        lineRenderer = this.AddComponent<LineRenderer>();
        lineRenderer.material = Resources.Load<Material>("Materials/Edge");
    }
    void Update()
    {
        if (A == null || B == null || A == B)
        {
            GameObject.Destroy(transform.gameObject);
        }
        else
        {
            lineRenderer.SetPosition(0, A.transform.position);
            lineRenderer.SetPosition(1, B.transform.position);
        }
        lineRenderer.startWidth = lineRenderer.endWidth = ((A.lossyScale.x+B.lossyScale.x)/2)*nodeManager.edgeWidth;
    }
}
