using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NodeBehaviour : MonoBehaviour
{
    private XRReSnapGrabbable interactable;
    private TextMeshPro UIName;
    private NodeInfo nodeInfo = new NodeInfo();
    public NetworkVisualiser nodeManager;
    public bool isGrabbed;
    public Material material;
    public NodeInfo NodeInfo
    {
        get { return nodeInfo; }
        set { nodeInfo = value;
            SetColor();
        }
    }
    public Vector3 position {
        get { return nodeInfo.position; }
        set { nodeInfo.position = value; }
    }


    public bool Glow
    {
        get { return transform.GetComponentInChildren<MeshRenderer>().material.GetFloat("_LineWidth") == 10; }
        set { transform.GetComponentInChildren<MeshRenderer>().material.SetFloat("_LineWidth",value ? 10 : 1); }
    }
    

    void Awake()
    {
        interactable = this.GetComponent<XRReSnapGrabbable>();
        UIName = transform.GetChild(1).GetComponent<TextMeshPro>();
    }
    void Update()
    {
        if (!interactable.isSelected)
        {
            if(nodeManager.relativePositioning)
                transform.localPosition = Vector3.Lerp(
                    transform.localPosition,
                    nodeInfo.position.Map(
                        nodeManager.minNodePosition,
                        nodeManager.maxNodePosition,
                        nodeManager.MinDistance,
                        nodeManager.MaxDistance),
                    Time.deltaTime
                    );
            else
            {
                transform.localPosition = Vector3.Lerp(transform.localPosition,nodeInfo.position,Time.deltaTime);
            }
                
            //transform.localPosition = (new Vector3(top.x / bottom.x, top.y / bottom.y, top.z / bottom.z) - new Vector3(0.5f, 0.5f, 0.5f)) * nodeManager.maxDistance;
        }
        transform.localScale = new Vector3(nodeManager._nodeSize, nodeManager._nodeSize, nodeManager._nodeSize);
    }
    private void Start()
    {
        SetColor();
    }
    private void SetColor()
    {
        float hue = (float)nodeManager.Networks.IndexOf(nodeInfo.networkID)/ nodeManager.Networks.Count;
        transform.GetComponentInChildren<MeshRenderer>().material.SetColor("_Color", Color.HSVToRGB(hue,1,1));
        transform.GetComponentInChildren<MeshRenderer>().material.SetColor("_EmissionColor", Color.HSVToRGB(hue,1,1));
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
