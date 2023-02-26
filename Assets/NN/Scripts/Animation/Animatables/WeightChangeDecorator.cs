using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeightChangeDecorator : MonoBehaviour,IAnimatable
{
    public Edge edge;
    private float from;
    public float changeTo;
    public string Name => "WeightChanger";

    public void Animate(float t)
    {
        edge.edge.relationInfo.label = Mathf.Lerp(from, changeTo,t).ToString("0.00");
    }
    private Color neutralColor = Color.white;
    private Color activatedColor = Color.red;
    public void Show(float t)
    {
        edge.edge.lineRenderer.startColor = Color.Lerp(neutralColor, activatedColor, t);
        edge.edge.lineRenderer.endColor = Color.Lerp(neutralColor, activatedColor, t);
    }
    public static WeightChangeDecorator Create(Edge edge, float changeTo)
    {
        GameObject go = new GameObject("WeightChangeDecorator");
        go.transform.SetParent(edge.edge.nodeManager.transform);
        WeightChangeDecorator wcd = go.AddComponent<WeightChangeDecorator>();
        wcd.from = float.Parse(edge.RelationInfo.label);
        wcd.edge = edge;
        wcd.changeTo = changeTo;
        return wcd;
    }
}
