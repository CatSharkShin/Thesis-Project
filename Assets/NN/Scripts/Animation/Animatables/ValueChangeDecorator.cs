using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NeuralNetwork;
public class ValueChangeDecorator : MonoBehaviour, IAnimatable
{
    public Node node;
    private float from;
    public float changeTo;
    public string Name => "ValueChangeDecorator";

    public void Animate(float t)
    {
        node.nodeInfo.act = Mathf.Lerp(from, changeTo, t);
    }

    public void Show(float t)
    {
        //
        //
    }
    public static ValueChangeDecorator Create(Node node, float changeTo)
    {
        GameObject go = new GameObject("WeightChangeDecorator");
        go.transform.SetParent(node.transform);
        ValueChangeDecorator vcd = go.AddComponent<ValueChangeDecorator>();
        vcd.from = node.nodeInfo.act;
        vcd.node = node;
        vcd.changeTo = changeTo;
        return vcd;
    }
}
