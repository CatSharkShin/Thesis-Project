using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using NeuralNetwork;
public class NodeMenu : MonoBehaviour
{
    public Node node;
    public TextMeshProUGUI ID;
    public TextMeshProUGUI Network;
    public TextMeshProUGUI Func;
    void Start()
    {
        
    }
    void Update()
    {
        if(node.nodeInfo != null)
        {
            ID.text = node.nodeInfo.nodeID.ToString();
            Network.text = node.nodeInfo.networkID.ToString();
            Func.text = node.nodeInfo.func;
        }
    }
}
