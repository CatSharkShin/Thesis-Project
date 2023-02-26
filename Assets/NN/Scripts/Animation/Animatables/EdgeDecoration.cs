using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EdgeDecoration : IAnimatable
{
    private string name;
    public Edge edge;
    public string Name => name;

    private float from;
    private float to;

    public TextMeshPro weightLabel;
    TextMeshPro valueLabel;
    public EdgeDecoration(Edge edge)
    {
        this.name = $"Edge {edge.edge.relationInfo.id}";
        this.edge = edge;

        this.from = edge.edge.nodeManager.Nodes[edge.cid1].NodeInfo.act;
        this.to = edge.edge.nodeManager.Nodes[edge.cid1].NodeInfo.act*float.Parse(edge.edge.relationInfo.label);

        InitializeLabels();
        weightLabel.text = edge.edge.relationInfo.label;
        valueLabel.text = this.from.ToString("0.000");

        weightLabel.transform.localScale = new Vector3(0, 0, 0);
        valueLabel.transform.localScale = new Vector3(0, 0, 0);
    }
    public void Animate(float t)
    {
        NetworkVisualiser nodeManager = edge.edge.nodeManager;
        LineRenderer lineRenderer = edge.edge.lineRenderer;

        valueLabel.text = Mathf.Lerp(from, to, t).ToString("0.000");
        weightLabel.fontSize = nodeManager.edgeLabelSize * nodeManager._nodeSize;
        valueLabel.fontSize = nodeManager.edgeLabelSize * nodeManager._nodeSize;
        weightLabel.margin = new Vector4(0, 0, 0, nodeManager.edgeLabelSize * nodeManager._nodeSize * 0.2f);
        valueLabel.margin = new Vector4(0, 0, 0, nodeManager.edgeLabelSize * nodeManager._nodeSize * 0.2f);

        Vector3 fromPos = Vector3.Lerp(lineRenderer.GetPosition(0), lineRenderer.GetPosition(1), 0.25f);
        Vector3 toPos = Vector3.Lerp(lineRenderer.GetPosition(0), lineRenderer.GetPosition(1), 1f);
        valueLabel.transform.position = Vector3.Lerp(fromPos, toPos, t);
        weightLabel.transform.position = Vector3.Lerp(lineRenderer.GetPosition(0), lineRenderer.GetPosition(1), 0.75f);

        Quaternion rotate = LinePerpendicular();
        weightLabel.transform.rotation = rotate;
        valueLabel.transform.rotation = rotate;
        if (t > 0.99)
            valueLabel.transform.localScale = Vector3.zero;
    }
    void InitializeLabels()
    {
        NetworkVisualiser nodeManager = edge.edge.nodeManager;
        EdgeRenderer edgeRenderer = edge.edge;

       
        var tempGo = new GameObject("Weight Label");
        tempGo.transform.SetParent(nodeManager.transform);
        //tempGo.transform.parent = edgeRenderer.transform;
        weightLabel = tempGo.AddComponent<TextMeshPro>();
        weightLabel.horizontalAlignment = HorizontalAlignmentOptions.Center;
        weightLabel.verticalAlignment = VerticalAlignmentOptions.Middle;
        weightLabel.margin = new Vector4(0, 0, 0, 0.5f);

        HintVisual.Create(tempGo.transform, "Weight", new Vector3(0, 0.1f, 0));

        tempGo = new GameObject("Value Label");
        //tempGo.transform.parent = edgeRenderer.transform;
        valueLabel = tempGo.AddComponent<TextMeshPro>();
        valueLabel.horizontalAlignment = HorizontalAlignmentOptions.Center;
        valueLabel.verticalAlignment = VerticalAlignmentOptions.Middle;
        valueLabel.margin = new Vector4(0, 0, 0, 0.5f);
        tempGo.transform.SetParent(nodeManager.transform);

        HintVisual.Create(tempGo.transform, "Value",new Vector3(0,0.1f,0));
    }
    Quaternion LinePerpendicular()
    {
        LineRenderer lineRenderer = edge.edge.lineRenderer;
        weightLabel.transform.position = Vector3.Lerp(lineRenderer.GetPosition(0), lineRenderer.GetPosition(1), 0.75f);
        Vector3 directionVector = lineRenderer.GetPosition(1) - lineRenderer.GetPosition(0);
        bool LeftToRight = Camera.main.WorldToScreenPoint(lineRenderer.GetPosition(0)).x < Camera.main.WorldToScreenPoint(lineRenderer.GetPosition(1)).x;
        Vector3 from = Vector3.right;
        if (!LeftToRight)
        {
            directionVector = -directionVector;
        }
        Quaternion rotate = Quaternion.FromToRotation(from, directionVector.normalized);
        return rotate;
    }

    public void Show(float t)
    {
        weightLabel.text = edge.edge.relationInfo.label;
        weightLabel.transform.localScale = (new Vector3(1, 1, 1)) * t;
        valueLabel.transform.localScale = (new Vector3(1, 1, 1)) * t;
    }
}
