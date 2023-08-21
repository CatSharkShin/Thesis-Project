using Decorators;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using NeuralNetwork;
public class NodeBoolDecorator : MonoBehaviour
{
    Node node;
    Func<float, bool> boolFunc;
    TextMeshProUGUI tmp;
    void Awake()
    {
        tmp = transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        if(tmp != null)
        {
            tmp.text = boolFunc.Invoke(node.nodeInfo.act).ToString().ToUpper();
        }
    }
    public static NodeBoolDecorator Create(Node node, Func<float, bool> func)
    {
        GameObject Go = Instantiate(Resources.Load<GameObject>("Prefabs/Decorators/Output Decorator"));
        Go.transform.SetParent(node.transform);
        Go.transform.localScale = new Vector3(1, 1, 1);
        NodeBoolDecorator nbd = Go.AddComponent<NodeBoolDecorator>();
        nbd.node = node;
        nbd.boolFunc = func;
        XRGrabInteractable grabbable = node.gameObject.GetComponent<XRGrabInteractable>();
        ScaleOnGrab.Create(Go.transform.GetChild(0).gameObject, Vector3.zero, Vector3.one, grabbable);
        return nbd;
    }
}
