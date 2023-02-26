using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ValueDecorator : MonoBehaviour,IAnimatable
{
    public Edge edge;
    public string Name => edge.cid1 + "-" + edge.cid2 + " Value Decorator";
    public TextMeshPro valueLabel;
    float changeTo;
    void Start()
    {
        NetworkVisualiser nw = edge.edge.nodeManager;
        valueLabel = this.AddComponent<TextMeshPro>();
        valueLabel.horizontalAlignment = HorizontalAlignmentOptions.Center;
        valueLabel.verticalAlignment = VerticalAlignmentOptions.Middle;
        valueLabel.margin = new Vector4(0, 0, 0, 0.5f);
        valueLabel.text = nw.Nodes[edge.edge.relationInfo.cid1].NodeInfo.act.ToString("0.##");
        float weight = 0;
        if (float.TryParse(edge.edge.relationInfo.label, out float wt))
            weight = wt;
        changeTo = nw.Nodes[edge.edge.relationInfo.cid1].NodeInfo.act * weight;

        HintVisual.Create(transform, "Value", new Vector3(0, 0.1f, 0));
        Show(0);
    }
    float positionT = 0;
    float minPositionT = 0.25f;
    float maxPositionT = 1f;
    private void Update()
    {
        NetworkVisualiser nw = edge.edge.nodeManager;
        LineRenderer lineRenderer = edge.edge.lineRenderer;

        float actualT = Mathf.Lerp(minPositionT,maxPositionT,positionT);
        valueLabel.transform.position = Vector3.Lerp(lineRenderer.GetPosition(0), lineRenderer.GetPosition(1), actualT);
        valueLabel.fontSize = nw.edgeLabelSize * nw._nodeSize;
        valueLabel.margin = new Vector4(0, 0, 0, nw.edgeLabelSize * nw._nodeSize * 0.2f);
        valueLabel.text = nw.Nodes[edge.edge.relationInfo.cid1].NodeInfo.act.ToString("0.##");
        transform.rotation = Extensions.LinePerpendicular(lineRenderer.GetPosition(0), lineRenderer.GetPosition(1));
    }
    public void Animate(float t)
    {
        positionT = t;
        valueLabel.text = Mathf.Lerp(edge.edge.nodeManager.Nodes[edge.edge.relationInfo.cid1].NodeInfo.act, changeTo, t).ToString("0.##");
        if (t > 0.99)
            Show(0);
    }

    public void Show(float t)
    {
        valueLabel.transform.localScale = (new Vector3(1, 1, 1)) * t;
    }
    public static ValueDecorator Create(Edge edge)
    {
        GameObject go = new GameObject("ValueDecorator");
        go.transform.SetParent(edge.edge.nodeManager.transform);
        ValueDecorator vd = go.AddComponent<ValueDecorator>();
        vd.edge = edge;
        return vd;
    }
}
