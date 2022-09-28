using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class NodeBehaviour : MonoBehaviour
{
    private TextMeshPro UIName;
    private NodeInfo nodeInfo;
    public NodeManager nodeManager;
    public bool isGrabbed;
    public Material material;
    public NodeInfo NodeInfo
    {
        get { return nodeInfo; }
        set { nodeInfo = value; NodeInfoChanged(); }
    }
    public Vector3 position;
    private XRReSnapGrabbable interactable;
    private void NodeInfoChanged()
    {
        position = new Vector3(NodeInfo.x_pos, NodeInfo.y_pos, NodeInfo.z_pos);
    }

    void Awake()
    {
        interactable = this.GetComponent<XRReSnapGrabbable>();
        UIName = transform.GetChild(1).GetComponent<TextMeshPro>();
        /*
        material = (Material)Instantiate(Resources.Load("Materials/Neuron"));
        transform.GetChild(0).GetComponent<MeshRenderer>().sharedMaterial = material;
        material.SetColor(0, Color.red);*/

        this.transform.localPosition = position;
    }
    private void Start()
    {
        SetColor();
    }
    void Update()
    {
        transform.localScale = new Vector3(nodeManager.nodeSize, nodeManager.nodeSize, nodeManager.nodeSize);
        if(!interactable.isSelected)
            this.transform.localPosition = Vector3.Lerp(transform.localPosition,position,Time.deltaTime*1.5f);
    }
    private void SetColor()
    {
        float hue = (float)nodeManager.Networks.IndexOf(nodeInfo.id)/ nodeManager.Networks.Count;
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
        nodeManager.Nodes.Remove(nodeInfo.uid);
    }
}
