using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace NeuralNetwork{
    public class Node : MonoBehaviour
    {
        public Visualizer networkVisualiser;
        public NodeInfo nodeInfo;

        public bool IsGrabbed { get { return interactable.isSelected; } }

        private XRReSnapGrabbable interactable;
        private void Awake()
        {
            interactable = this.GetComponent<XRReSnapGrabbable>();
        }
        void Start()
        {
            SetColor();
        }
        private void SetColor()
        {
            float hue = (float)networkVisualiser.Networks.IndexOf(nodeInfo.networkID) / networkVisualiser.Networks.Count;
            transform.GetComponentInChildren<MeshRenderer>().material.SetColor("_Color", Color.HSVToRGB(hue, 1, 1));
            transform.GetComponentInChildren<MeshRenderer>().material.SetColor("_EmissionColor", Color.HSVToRGB(hue, 1, 1));
        }
        void Update()
        {
            if (nodeInfo == null)
            {
                Debug.Log("NODEINFO NULL, DESTROYED");
                return;
            }
            // TODO: Implement relative position calculation and create these fields
            if (!interactable.isSelected)
            {
                if (networkVisualiser.relativePositioning)
                {
                    //Debug.Log($"localPosition {transform.localPosition}");
                    //Debug.Log($"minNodePosition {networkVisualiser.minNodePosition}");
                    //Debug.Log($"maxNodePosition {networkVisualiser.maxNodePosition}");
                    //Debug.Log($"MinDistance {networkVisualiser.MinDistance}");
                    //Debug.Log($"MaxDistance {networkVisualiser.MaxDistance}");
                    Vector3 newPos = Vector3.Lerp(
                        transform.localPosition,
                        nodeInfo.position.Map(
                            networkVisualiser.minNodePosition,
                            networkVisualiser.maxNodePosition,
                            networkVisualiser.MinDistance,
                            networkVisualiser.MaxDistance),
                        Time.deltaTime
                        );
                    transform.localPosition = newPos;
                }
                else
                {
                    transform.localPosition = Vector3.Lerp(transform.localPosition, nodeInfo.position + networkVisualiser.offset, Time.deltaTime);
                }

                //transform.localPosition = (new Vector3(top.x / bottom.x, top.y / bottom.y, top.z / bottom.z) - new Vector3(0.5f, 0.5f, 0.5f)) * networkVisualiser.maxDistance;
            }
            transform.localScale = new Vector3(
                networkVisualiser.NodeSize,
                networkVisualiser.NodeSize,
                networkVisualiser.NodeSize);
        }
        public static Node Create(Transform parent, NodeInfo nodeinfo)
        {
            GameObject Go = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Node"), parent, false);
            Go.transform.SetParent(parent);
            Go.transform.localScale = Vector3.zero;
            Node node = Go.AddComponent<Node>();
            node.nodeInfo = nodeinfo;
            node.networkVisualiser = parent.GetComponentInParent<Visualizer>();
            if (!node.networkVisualiser.Networks.Contains(node.nodeInfo.networkID))
            {
                node.networkVisualiser.Networks.Add(node.nodeInfo.networkID);
                foreach (var n in node.networkVisualiser.Nodes)
                {
                    n.Value.SetColor();
                }
            }
            return node;
        }
        private void OnDestroy() {
            networkVisualiser.Nodes.Remove(nodeInfo.nodeID);
            if(!networkVisualiser.Nodes.Where(node => node.Value.nodeInfo.networkID == nodeInfo.networkID).Any())
            {
                networkVisualiser.Networks.Remove(nodeInfo.networkID);
            }
            foreach (var n in networkVisualiser.Nodes)
            {
                n.Value.SetColor();
            }
        }
    }
}