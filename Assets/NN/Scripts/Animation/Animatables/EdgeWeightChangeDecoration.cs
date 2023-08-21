using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgeWeightChangeDecoration : IAnimatable
{
    private string name;
    public string Name => name;
    private EdgeDecoration edgeDecor;
    private float changeTo;
    private float changeFrom;
    private Color startColor;
    private Color endColor;
    public EdgeWeightChangeDecoration(EdgeDecoration edgeDecor,float changeTo)
    {
        this.name = $"WeightChanger: Edge {edgeDecor.Name}";
        this.edgeDecor = edgeDecor;
        this.changeTo = changeTo;
        changeFrom = float.Parse(edgeDecor.edge.edgeInfo.label);

        startColor = edgeDecor.edge.lineRenderer.startColor;
        endColor = edgeDecor.edge.lineRenderer.endColor;
    }
    public void Animate(float t)
    {
        edgeDecor.weightLabel.text = Mathf.Lerp(changeFrom, changeTo, t).ToString();
        edgeDecor.edge.edgeInfo.label = edgeDecor.weightLabel.text;
        float colorT = 1-(Mathf.Abs(0.5f-t)*2);
        edgeDecor.edge.lineRenderer.startColor = Color.Lerp(startColor, Color.red, colorT);
        edgeDecor.edge.lineRenderer.endColor = Color.Lerp(endColor, Color.red, colorT);
        edgeDecor.weightLabel.color = Color.Lerp(Color.white, Color.red, colorT);
    }

    public void Show(float t)
    {
        //edgeDecor.edge.lineRenderer.startColor = Color.Lerp(startColor, Color.red, t);
        //edgeDecor.edge.lineRenderer.startColor = Color.Lerp(endColor, Color.red, t);
    }
}
