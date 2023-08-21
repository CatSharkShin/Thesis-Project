using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using NeuralNetwork;
using Unity.VisualScripting;
using Edge = NeuralNetwork.Edge;

public class WeightDecorator : MonoBehaviour,IAnimatable
{
    public Edge edge;
    public string Name => edge.edgeInfo.cid1+"-"+edge.edgeInfo.cid2+" Weight Decorator";
    TextMeshPro weightLabel;
    bool initialized;
    void Awake()
    {
        StartCoroutine(Initialize());
    }
    IEnumerator Initialize()
    {
        while (edge == null)
            yield return null;
        weightLabel = this.AddComponent<TextMeshPro>();
        weightLabel.horizontalAlignment = HorizontalAlignmentOptions.Center;
        weightLabel.verticalAlignment = VerticalAlignmentOptions.Middle;
        weightLabel.margin = new Vector4(0, 0, 0, 0.5f);
        weightLabel.text = edge.edgeInfo.label;

        HintVisual.Create(transform, "Weight", new Vector3(0, 0.1f, 0));
        weightLabel.transform.localScale = Vector3.zero;
        initialized = true;
    }
    private void Update()
    {
        Visualizer nw = edge.networkVisualizer;
        LineRenderer lineRenderer = edge.lineRenderer;

        transform.rotation = Extensions.LinePerpendicular(lineRenderer.GetPosition(0), lineRenderer.GetPosition(1));
        if (initialized)
        {
            weightLabel.transform.position = Vector3.Lerp(lineRenderer.GetPosition(0), lineRenderer.GetPosition(1), 0.75f);
            weightLabel.text = edge.edgeInfo.label;
            weightLabel.fontSize = nw.edgeLabelSize * nw.NodeSize;
            weightLabel.margin = new Vector4(0, 0, 0, nw.edgeLabelSize * nw.NodeSize * 0.2f);
        }
    }
    public static WeightDecorator Create(Edge edge)
    {
        GameObject go = new GameObject("WeightDecorator");
        go.transform.SetParent(edge.networkVisualizer.transform);
        WeightDecorator wd = go.AddComponent<WeightDecorator>();
        wd.edge = edge;
        return wd;
    }

    public void Animate(float t)
    {
        throw new NotImplementedException();
    }

    public void Show(float t)
    {
        weightLabel.transform.localScale = (new Vector3(1, 1, 1)) * t;
    }
}
