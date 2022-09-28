using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NodeMenu : MonoBehaviour
{
    public NodeBehaviour node;
    public TextMeshProUGUI ID;
    public TextMeshProUGUI Network;
    public TextMeshProUGUI Func;
    void Start()
    {
        
    }
    void Update()
    {
        if(node.NodeInfo != null)
        {
            ID.text = node.NodeInfo.uid.ToString();
            Network.text = node.NodeInfo.id.ToString();
            Func.text = node.NodeInfo.func;
        }
    }
}
