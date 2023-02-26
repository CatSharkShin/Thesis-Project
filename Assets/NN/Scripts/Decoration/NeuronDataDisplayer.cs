using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using Unity.Tutorials.Core.Editor;
using UnityEngine;

public class NeuronDataDisplayer : MonoBehaviour
{
    private NodeBehaviour node;
    public string calculationText;

    void Awake()
    {
        node = this.transform.parent.GetComponent<NodeBehaviour>();
        StartCoroutine(AnimateCalculation());
    }
    IEnumerator ShowPanel()
    {
        Transform panel = this.transform.GetChild(node.NodeInfo.networkID == 99 ? 1 : 0);

        while(panel.transform.localScale.x > 0.999)
        {
            panel.transform.localScale = new Vector3(
                Mathf.Lerp(panel.transform.localScale.x, 1, Time.deltaTime),
                panel.transform.localScale.y,
                panel.transform.localScale.z);

            yield return new WaitForEndOfFrame();
        }
    }
    IEnumerator AnimateCalculation()
    {
        TextMeshProUGUI tmp = transform.GetChild(0).GetChild(0).GetChild(0).Find("calculation").GetComponent<TextMeshProUGUI>();
        tmp.text = "";
        while (string.IsNullOrEmpty(calculationText))
            yield return new WaitForEndOfFrame();
        while (!node.isGrabbed)
            yield return new WaitForEndOfFrame();
        foreach (char c in calculationText)
        {
            tmp.text += c;
            yield return new WaitForSeconds(0.1f);
        }
    }
    void Update()
    {
        Transform panel = this.transform.GetChild(node.NodeInfo.networkID == 99 ? 1 : 0);
        TextMeshProUGUI tmp = transform.GetChild(0).GetChild(0).GetChild(0).Find("text").GetComponent<TextMeshProUGUI>();
        IEnumerable<KeyValuePair<int,Edge>> weights = node.nodeManager.Edges.Where(edge => edge.Value.cid2 == node.NodeInfo.nodeID);
        StringBuilder weightTextSB = new StringBuilder();
        int i = 0;
        foreach (var item in weights)
        {
            weightTextSB.Append(item.Value.edge.relationInfo.label);
            if(i < weights.Count()-1)
                weightTextSB.Append(", ");
            i++;
        }
        tmp.text = weightTextSB.ToString();
        float lerpTo = node.isGrabbed ? 1 : 0;
        panel.transform.localScale = new Vector3(
                Mathf.Lerp(panel.transform.localScale.x, lerpTo, Time.deltaTime*2f),
                panel.transform.localScale.y,
                panel.transform.localScale.z);
    }
}
