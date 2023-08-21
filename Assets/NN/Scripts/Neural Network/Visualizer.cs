using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using NeuralNetwork;
namespace NeuralNetwork
{
    public class Visualizer : MonoBehaviour
    {
        public Dictionary<int, Node> Nodes { get; }
        public Dictionary<int, Edge> Edges { get; }
        public List<int> Networks { get; }

        // Calculated Info
        [System.NonSerialized]
        public Vector3 minNodePosition = Vector3.positiveInfinity;
        [System.NonSerialized]
        public Vector3 maxNodePosition = Vector3.negativeInfinity;

        private float nodeSize;
        public float NodeSize
        {
            get { return nodeSize; }
        }

        public Vector3 MinDistance
        {
            get
            {
                return new Vector3(-size.x / 2, 0, -size.z / 2) + offset;
            }
        }
        public Vector3 MaxDistance
        {
            get
            {
                return new Vector3(size.x / 2, size.y, size.z / 2) + offset;
            }
        }

        // relative positioning options
        public Vector3 size;
        public Vector3 offset;
        public float edgeLabelSize; // Decorators, move to Settings
        public float edgeWidth;
        public bool connectOnStart;
        public bool relativePositioning;
        private Visualizer()
        {
            Nodes = new Dictionary<int, Node>();
            Edges = new Dictionary<int, Edge>();
            Networks = new List<int>();
        }
        void Start()
        {
            if (connectOnStart)
            {
                MatchNodes(Api.GetNetwork()); //RENAME
                MatchEdges(Api.GetRelations()); //RENAME
            }
        }
        private void Update()
        {
            CalculateNodeSize();
            CalculateBounds();
        }
        public void CalculateBounds()
        {
            if (Nodes.Count == 0)
                return;
            minNodePosition = Vector3.positiveInfinity;//Nodes.First().Value.nodeInfo.position + Vector3.one;
            maxNodePosition = Vector3.negativeInfinity;//Nodes.First().Value.nodeInfo.position - Vector3.one;
            if (Nodes.Count == 1)
            {
                minNodePosition = Nodes[0].nodeInfo.position - Vector3.one;//Nodes.First().Value.nodeInfo.position + Vector3.one;
                maxNodePosition = Nodes[0].nodeInfo.position + Vector3.one;//Nodes.First().Value.nodeInfo.position - Vector3.one;
                return;
            }

            minNodePosition.x = Nodes.Values.Min(node => node.nodeInfo.position.x);
            minNodePosition.y = Nodes.Values.Min(node => node.nodeInfo.position.y);
            minNodePosition.z = Nodes.Values.Min(node => node.nodeInfo.position.z);

            maxNodePosition.x = Nodes.Values.Max(node => node.nodeInfo.position.x);
            maxNodePosition.y = Nodes.Values.Max(node => node.nodeInfo.position.y);
            maxNodePosition.z = Nodes.Values.Max(node => node.nodeInfo.position.z);

            if (maxNodePosition.x == minNodePosition.x)
                maxNodePosition.x += 1;
            if (maxNodePosition.y == minNodePosition.y)
                maxNodePosition.y += 1;
            if (maxNodePosition.z == minNodePosition.z)
                maxNodePosition.z += 1;
        }

        // Matches the network with the given nodeinfos
        // This will basically reset the nodes, since unneeded nodes are removed
        public void MatchNodes(List<NodeInfo> nodeInfos = null)
        {
            // Removes unneeded nodes
            foreach (KeyValuePair<int, Node> node in Nodes.ToList())
            {
                if (nodeInfos.FirstOrDefault(n => n.nodeID == node.Key) == null)
                {
                    Nodes.Remove(node.Key);
                    Destroy(node.Value.gameObject);
                }
            }
            // Sets new Nodeinfo
            // Adds new Nodes if needed
            foreach (NodeInfo nodeinfo in nodeInfos)
            {
                if (Nodes.TryGetValue(nodeinfo.nodeID, out Node node))
                {
                    node.nodeInfo = nodeinfo;
                }
                else
                {
                    Nodes.Add(nodeinfo.nodeID, Node.Create(transform, nodeinfo));
                }
            }
            CalculateBounds();
        }

        public void MatchEdges(List<EdgeInfo> edgeInfos = null)
        {
            // Removes unneeded edges
            foreach (KeyValuePair<int, Edge> edge in Edges.ToList())
            {
                if (edgeInfos.FirstOrDefault(e => e.id == edge.Key) == null)
                {
                    Edges.Remove(edge.Key);
                    Destroy(edge.Value.gameObject);
                }
            }
            /*
            foreach (KeyValuePair<int, Edge> edge in Edges.ToList())
            {
                Destroy(edge.Value.gameObject);
            }
            Edges.Clear();*/
            // Adds or sets edges
            foreach (EdgeInfo edgeInfo in edgeInfos)
            {
                if (Edges.TryGetValue(edgeInfo.id, out Edge edge))
                {
                    edge.nodeA = Nodes[edgeInfo.cid1];
                    edge.nodeB = Nodes[edgeInfo.cid2];
                }
                else
                {
                    Edges.Add(edgeInfo.id, Edge.Create(transform, edgeInfo));
                }
            }
            CalculateBounds();
        }

        private void CalculateNodeSize()
        {
            // data mapping to min-max is a solution if this isnt sufficient
            /*if (sizing)
            {
                //transform.localScale = Vector3.Lerp(transform.localScale, setToScale, Time.deltaTime);
                if (transform.localScale.x < setToScale.x * 1.05f)
                    sizing = false;
            }*/
            /*
            float mxDistance = size.Avg();
            foreach (KeyValuePair<int, Node> node in Nodes)
            {
                if (!node.Value.node.isGrabbed)
                {
                    float distance = Vector3.Distance(node.Value.node.position, Vector3.zero);
                    if (distance > mxDistance)
                        mxDistance = distance;
                }
            }
            if (mxDistance > size.Avg() || mxDistance < size.Avg() * 0.95f)
            {
                float newScale = (size.Avg() / mxDistance);
                setToScale = new Vector3(newScale, newScale, newScale);
                sizing = true;
            }*/
            if (Nodes.Count > 0)
                nodeSize = ((size.Avg()*2) / (Nodes.Count)); //* (1 / transform.localScale.Avg());
        }
        public List<Node> GetLeftNodes(int forId)
        {
            return Edges.Where(edge => edge.Value.edgeInfo.cid2 == forId).Select(edge => Nodes[edge.Value.edgeInfo.cid1]).ToList();
        }
        public List<Node> GetRightNodes(int forId)
        {
            return Edges.Where(edge => edge.Value.edgeInfo.cid1 == forId).Select(edge => Nodes[edge.Value.edgeInfo.cid2]).ToList();
        }
        public List<Edge> GetLeftEdges(int forId)
        {
            return Edges.Where(edge => edge.Value.edgeInfo.cid2 == forId).Select(edge => edge.Value).ToList();
        }
        public List<Edge> GetRightEdges(int forId)
        {
            return Edges.Where(edge => edge.Value.edgeInfo.cid1 == forId).Select(edge => edge.Value).ToList();
        }
    }
}
