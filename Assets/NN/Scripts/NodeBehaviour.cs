using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class NodeBehaviour : MonoBehaviour
{
    private TextMeshPro UIName;
    private NodeInfo nodeInfo = new NodeInfo();
    public NodeManager nodeManager;
    public bool isGrabbed;
    public Material material;
    public NodeInfo NodeInfo
    {
        get { return nodeInfo; }
        set { nodeInfo = value; }
    }
    public Vector3 position {
        get { return nodeInfo.position; }
        set { nodeInfo.position = value; }
    }
    private XRReSnapGrabbable interactable;
    void Awake()
    {
        interactable = this.GetComponent<XRReSnapGrabbable>();
        UIName = transform.GetChild(1).GetComponent<TextMeshPro>();
    }
    void Update()
    {
        transform.localScale = new Vector3(nodeManager.nodeSize, nodeManager.nodeSize, nodeManager.nodeSize);
        if(!interactable.isSelected)
            this.transform.localPosition = Vector3.Lerp(transform.localPosition,position,Time.deltaTime*1.5f);
    }
    private void Start()
    {
        SetColor();
    }
    private void SetColor()
    {
        float hue = (float)nodeManager.Networks.IndexOf(nodeInfo.networkID)/ nodeManager.Networks.Count;
        transform.GetComponentInChildren<MeshRenderer>().material.SetColor("_Color", Color.HSVToRGB(hue,1,1));
    }

    public void Selected()
    {
        isGrabbed = true;
    }
    public void UnSelected()
    {
        isGrabbed = false;
    }
    private void OnDestroy()
    {
        //Removes itself from the list on destroy
        //Added it so i can test the auto sizing
        nodeManager.Nodes.Remove(nodeInfo.nodeID);
    }
}
