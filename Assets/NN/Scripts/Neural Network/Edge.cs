using NeuralNetwork;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace NeuralNetwork{
    public class Edge : MonoBehaviour
    {
        public Node nodeA;
        public Node nodeB;
        public LineRenderer lineRenderer;
        public EdgeInfo edgeInfo;
        public Visualizer networkVisualizer;
        private void Awake()
        {
            lineRenderer = this.AddComponent<LineRenderer>();
            lineRenderer.material = Resources.Load<Material>("Materials/Edge");
        }
        void Update()
        {
            if(networkVisualizer.Nodes[edgeInfo.cid1] != null && networkVisualizer.Nodes[edgeInfo.cid2] != null)
            {
                nodeA = networkVisualizer.Nodes[edgeInfo.cid1];
                nodeB = networkVisualizer.Nodes[edgeInfo.cid2];
            }
            if (nodeA == null || nodeB == null || nodeA == nodeB)
            {
                GameObject.Destroy(transform.gameObject);
            }
            else
            {
                lineRenderer.SetPosition(0, nodeA.transform.position);
                lineRenderer.SetPosition(1, nodeB.transform.position);

                lineRenderer.startWidth = lineRenderer.endWidth = ((nodeA.transform.lossyScale.x + nodeB.transform.lossyScale.x) / 2) * networkVisualizer.edgeWidth;
                //lineRenderer.startWidth *= 1f;//A.GetComponent<NodeBehaviour>().NodeInfo.act / A.GetComponent<NodeBehaviour>().NodeInfo.max;
                //lineRenderer.endWidth *= 1f;//B.GetComponent<NodeBehaviour>().NodeInfo.act / A.GetComponent<NodeBehaviour>().NodeInfo.max;
            }
        }
        public static Edge Create(Transform parent, EdgeInfo edgeInfo)
        {
            GameObject go = new GameObject("EdgeRenderer");
            go.transform.SetParent(parent);
            Edge edge = go.AddComponent<Edge>();
            edge.networkVisualizer = parent.GetComponent<Visualizer>();
            edge.edgeInfo = edgeInfo;
            return edge;
        }
    }

}