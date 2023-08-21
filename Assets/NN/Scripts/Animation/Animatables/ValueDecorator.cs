using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using NeuralNetwork;
using Edge = NeuralNetwork.Edge;
public class ValueDecorator : MonoBehaviour,IAnimatable
{
    public Edge edge;
    public string Name => edge.edgeInfo.cid1 + "-" + edge.edgeInfo.cid2 + " Value Decorator";
    public TextMeshPro valueLabel;
    float changeTo;
    void Start()
    {
        Visualizer nw = edge.networkVisualizer;
        valueLabel = this.AddComponent<TextMeshPro>();
        valueLabel.horizontalAlignment = HorizontalAlignmentOptions.Center;
        valueLabel.verticalAlignment = VerticalAlignmentOptions.Middle;
        valueLabel.margin = new Vector4(0, 0, 0, 0.5f);
        valueLabel.text = nw.Nodes[edge.edgeInfo.cid1].nodeInfo.act.ToString("0.##");
        float weight = 0;
        if (float.TryParse(edge.edgeInfo.label, out float wt))
            weight = wt;
        changeTo = nw.Nodes[edge.edgeInfo.cid1].nodeInfo.act * weight;

        HintVisual.Create(transform, "Value", new Vector3(0, 0.1f, 0));
        Show(0);
    }
    float positionT = 0;
    float minPositionT = 0.25f;
    float maxPositionT = 1f;
    private void Update()
    {
        Visualizer nw = edge.networkVisualizer;
        LineRenderer lineRenderer = edge.lineRenderer;

        float actualT = Mathf.Lerp(minPositionT,maxPositionT,positionT);
        valueLabel.transform.position = Vector3.Lerp(lineRenderer.GetPosition(0), lineRenderer.GetPosition(1), actualT);

        valueLabel.fontSize = nw.edgeLabelSize * nw.NodeSize;
        valueLabel.margin = new Vector4(0, 0, 0, nw.edgeLabelSize * nw.NodeSize * 0.2f);
        valueLabel.text = nw.Nodes[edge.edgeInfo.cid1].nodeInfo.act.ToString("0.##");
        transform.rotation = Extensions.LinePerpendicular(lineRenderer.GetPosition(0), lineRenderer.GetPosition(1));
    }
    public void Animate(float t)
    {
        positionT = t;
        valueLabel.text = Mathf.Lerp(edge.networkVisualizer.Nodes[edge.edgeInfo.cid1].nodeInfo.act, changeTo, t).ToString("0.##");
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
        go.transform.SetParent(edge.networkVisualizer.transform);
        ValueDecorator vd = go.AddComponent<ValueDecorator>();
        vd.edge = edge;
        return vd;
    }
}
