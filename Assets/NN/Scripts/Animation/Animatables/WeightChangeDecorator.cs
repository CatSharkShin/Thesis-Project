using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NeuralNetwork;
public class WeightChangeDecorator : MonoBehaviour,IAnimatable
{
    public Edge edge;
    private float from;
    public float changeTo;
    public string Name => "WeightChanger";

    public void Animate(float t)
    {
        edge.edgeInfo.label = Mathf.Lerp(from, changeTo,t).ToString("0.00");
    }
    private Color neutralColor = Color.white;
    private Color activatedColor = Color.red;
    public void Show(float t)
    {
        edge.lineRenderer.startColor = Color.Lerp(neutralColor, activatedColor, t);
        edge.lineRenderer.endColor = Color.Lerp(neutralColor, activatedColor, t);
    }
    public static WeightChangeDecorator Create(Edge edge, float changeTo)
    {
        GameObject go = new GameObject("WeightChangeDecorator");
        go.transform.SetParent(edge.networkVisualizer.transform);
        WeightChangeDecorator wcd = go.AddComponent<WeightChangeDecorator>();
        wcd.from = float.Parse(edge.edgeInfo.label);
        wcd.edge = edge;
        wcd.changeTo = changeTo;
        return wcd;
    }
}
