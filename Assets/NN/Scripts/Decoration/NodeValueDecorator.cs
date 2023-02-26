using Palmmedia.ReportGenerator.Core;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class NodeValueDecorator : MonoBehaviour,IAnimatable
{
    public Node node;
    public float animatedValue;
    private TextMeshPro tmp;
    Vector3 scale;

    public string Name => node.NodeInfo.nodeID + " value decorator";

    private void Awake()
    {
        tmp = this.AddComponent<TextMeshPro>();
        tmp.fontSize = 4;
        tmp.verticalAlignment = VerticalAlignmentOptions.Middle;
        tmp.horizontalAlignment = HorizontalAlignmentOptions.Center;
        tmp.font = Resources.Load<TMP_FontAsset>("Fonts/Font_Override");
        tmp.UpdateFontAsset();
        StartCoroutine(Initialize());
    }
    IEnumerator Initialize()
    {
        while (node == null)
            yield return null;
        transform.localScale = scale;
        Show(0);
    }
    private void Update()
    {
        scale = node.Go.transform.localScale;
        transform.position = node.Go.transform.position;
        transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);
        //currentValue = Mathf.Lerp(currentValue, node.NodeInfo.act, Time.deltaTime);
        tmp.text = node.NodeInfo.act.ToString("0.00");
    }
    public static NodeValueDecorator Create(Node node,float animatedValue)
    {
        GameObject go = new GameObject("Node Value Decorator");
        NodeValueDecorator nvd = go.AddComponent<NodeValueDecorator>();
        go.transform.SetParent(node.node.nodeManager.transform);
        nvd.node = node;
        nvd.tmp.text = node.NodeInfo.act.ToString("0.##");
        nvd.animatedValue = animatedValue;
        return nvd;
    }

    public void Animate(float t)
    {
        tmp.text = Mathf.Lerp(node.NodeInfo.act, animatedValue,t).ToString("0.##"); 
    }

    public void Show(float t)
    {
        transform.localScale = Vector3.Lerp(Vector3.zero, scale, t);
    }
}
