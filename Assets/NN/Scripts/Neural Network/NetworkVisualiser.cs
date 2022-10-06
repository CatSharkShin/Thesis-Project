using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Xml.Linq;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Networking.Types;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class Node : IDisposable
{
    public int id { get; }
    public GameObject Go { get; }
    public NodeBehaviour node;
    public NodeInfo NodeInfo
    {
        get { return node.NodeInfo; }
        set { 
            node.NodeInfo = value;
            UpdateNodeManager();
            node.nodeManager.CalculateBounds();
        }
    }
    public Node(Transform parent,NodeInfo nodeinfo)
    {
        Go = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Node"), parent, false);
        this.id = nodeinfo.nodeID;
        node = Go.GetComponent<NodeBehaviour>();
        node.nodeManager = parent.GetComponent<NetworkVisualiser>();
        node.NodeInfo = nodeinfo;
        Go.transform.SetParent(parent);
        Go.transform.localScale = Vector3.zero;
        UpdateNodeManager();
        node.nodeManager.CalculateBounds();
    }
    public void UpdateNodeManager()
    {
        //min
        if (NodeInfo.position.x < node.nodeManager.minNodePosition.x)
        {
            node.nodeManager.minNodePosition.x = NodeInfo.position.x;
        }
        if (NodeInfo.position.y < node.nodeManager.minNodePosition.y)
        {
            node.nodeManager.minNodePosition.y = NodeInfo.position.y;
        }
        if (NodeInfo.position.z < node.nodeManager.minNodePosition.z)
        {
            node.nodeManager.minNodePosition.z = NodeInfo.position.z;
        }
        //max
        if (NodeInfo.position.x > node.nodeManager.maxNodePosition.x)
        {
            node.nodeManager.maxNodePosition.x = NodeInfo.position.x;
        }
        if (NodeInfo.position.y > node.nodeManager.maxNodePosition.y)
        {
            node.nodeManager.maxNodePosition.y = NodeInfo.position.y;
        }
        if (NodeInfo.position.z > node.nodeManager.maxNodePosition.z)
        {
            node.nodeManager.maxNodePosition.z = NodeInfo.position.z;
        }

        // Network update
        if (!node.nodeManager.Networks.Contains(NodeInfo.networkID))
        {
            node.nodeManager.Networks.Add(NodeInfo.networkID);
        }
    }
    public Node(Transform parent,NodeInfo nodeinfo,float size):this(parent,nodeinfo)
    {
        Go.transform.localScale = new Vector3(size, size, size);
    }
    public void Dispose()
    {
        GameObject.Destroy(Go);
    }
}
public class Edge : IDisposable
{
    public GameObject Go { get; }

    private EdgeRenderer edge;
    public Transform A
    {
        get { return edge.A; }
        set { edge.A = value; }
    }
    public Transform B
    {
        get { return edge.A; }
        set { edge.A = value; }
    }
    public Edge(Transform parent,RelationInfo relationInfo)
    {
        Go = new GameObject("EdgeRenderer");
        Go.transform.SetParent(parent);
        edge = Go.AddComponent<EdgeRenderer>();
        edge.nodeManager = parent.GetComponent<NetworkVisualiser>();
        edge.A = edge.nodeManager.Nodes[relationInfo.cid1].Go.transform;
        edge.B = edge.nodeManager.Nodes[relationInfo.cid2].Go.transform;
    }
    public Edge(Transform parent, Transform A, Transform B)
    {
        Go = new GameObject("EdgeRenderer");
        Go.transform.SetParent(parent);
        edge = Go.AddComponent<EdgeRenderer>();
        edge.nodeManager = parent.GetComponent<NetworkVisualiser>();
        edge.A = A;
        edge.B = B;
    }
    public void Dispose()
    {
        GameObject.Destroy(Go);
    }
}
public class NetworkVisualiser : MonoBehaviour
{
    const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

    public Dictionary<int, Node> Nodes { get; }
    public Dictionary<int, Edge> Edges { get; }

    //public Dictionary<int, Color> NetworkColors { get; }
    public List<int> Networks { get; }
    public Vector3 minNodePosition = Vector3.zero;

    public Vector3 maxNodePosition = Vector3.zero;

    public int maxNodes;
    public int maxEdges;
    public float update;
    public float nodeSize;
    public float edgeWidth;
    public Vector3 size;
    public Vector3 offset;
    public Vector3 MinDistance
    {
        get
        {
            return new Vector3(-size.x / 2, -size.y / 2, -size.z / 2) + new Vector3(0, +size.y / 2, 0) + offset;
        }
    }
    public Vector3 MaxDistance
    {
        get
        {
            return new Vector3(size.x / 2, size.y / 2, size.z / 2) + new Vector3(0, +size.y / 2, 0) + offset;
        }
    }
    public NetworkVisualiser()
    {
        Nodes = new Dictionary<int,Node>();
        Edges = new Dictionary<int,Edge>();
        //NetworkColors = new Dictionary<int, Color>();
        Networks = new List<int>();
    }
    public bool connectOnStart;
    void Start()
    {
        //StartCoroutine(useRandomGeneration());
        if (connectOnStart)
        {
            MatchNetwork();
            MatchRelations();
        }
    }
    public void CalculateBounds()
    {
        minNodePosition = Vector3.zero;
        maxNodePosition = Vector3.one;
        //TODO: FIX ERROR:
        // Happens when node count goes from 0 to 1
        foreach (KeyValuePair<int,Node> kvp in Nodes)
        {
            Node node = kvp.Value;
            //min
            if (node.NodeInfo.position.x < minNodePosition.x)
            {
                minNodePosition.x = node.NodeInfo.position.x;
            }
            if (node.NodeInfo.position.y < minNodePosition.y)
            {
                minNodePosition.y = node.NodeInfo.position.y;
            }
            if (node.NodeInfo.position.z < minNodePosition.z)
            {
                minNodePosition.z = node.NodeInfo.position.z;
            }
            //max
            if (node.NodeInfo.position.x > maxNodePosition.x)
            {
                maxNodePosition.x = node.NodeInfo.position.x;
            }
            if (node.NodeInfo.position.y > maxNodePosition.y)
            {
                maxNodePosition.y = node.NodeInfo.position.y;
            }
            if (node.NodeInfo.position.z > maxNodePosition.z)
            {
                maxNodePosition.z = node.NodeInfo.position.z;
            }
        }
    }
    public void MatchNetwork(List<NodeInfo> nodeInfos = null)
    {
        if (nodeInfos == null)
            nodeInfos = NNApi.GetNetwork();
        foreach(KeyValuePair<int,Node> node in Nodes.ToList())
        {
            if (nodeInfos.FirstOrDefault(n => n.nodeID == node.Value.NodeInfo.nodeID) == null)
            {
                Nodes.Remove(node.Key);
                node.Value.Dispose();
            }
        }
        foreach (NodeInfo nodeinfo in nodeInfos)
        {
            if (Nodes.TryGetValue(nodeinfo.nodeID,out Node node))
            {
                node.NodeInfo = nodeinfo;
            }
            else
            {
                Nodes.Add(nodeinfo.nodeID, new Node(transform, nodeinfo));
            }
        }
    }
    public void MatchRelations(List<RelationInfo> relationinfos = null)
    {
        if(relationinfos == null)
            relationinfos = NNApi.GetRelations();
        foreach (RelationInfo relationinfo in relationinfos)
        {
            if(Edges.TryGetValue(relationinfo.id, out Edge edge))
            {
                edge.A = Nodes[relationinfo.cid1].Go.transform;
                edge.B = Nodes[relationinfo.cid2].Go.transform;
            }
            else
            {
                Edges.Add(relationinfo.id,new Edge(transform, relationinfo));
            }
        }
    }

    private IEnumerator useRandomGeneration()
    {
        while (true)
        {
            //Debug.Log($"Nodes: {Nodes.Count}, Edges: {Edges.Count}");
            int action = Random.Range(0, 6);
            int randomNodeIndex = Random.Range(0, Nodes.Count);
            int randomNodeIndex2 = Random.Range(0, Nodes.Count);
            int randomEdgeIndex = Random.Range(0, Edges.Count);

            if(Nodes.Count < maxNodes)
            {
                //Nodes.Add(randomNode());
            }else if (Nodes.Count > maxNodes)
            {
                Nodes[randomNodeIndex].Dispose();
                Nodes.Remove(randomNodeIndex);
                randomNodeIndex = Random.Range(0, Nodes.Count);
                randomNodeIndex2 = Random.Range(0, Nodes.Count);
            }
            switch (action)
            {
                case 0:
                    //Debug.Log("Changing Node");
                    if (Nodes.Count > 0)
                        Nodes[randomNodeIndex].node.position = randomVector3();
                    break;
                case 1:
                    //Debug.Log("Deleting Edges");
                    if (Edges.Count > 0)
                    {
                        Edges[randomEdgeIndex].Dispose();
                        Edges.Remove(randomEdgeIndex);
                    }
                    break;
                case 2:
                    //Debug.Log("Adding Edges");
                    if (Nodes.Count > 1 && Edges.Count < maxEdges)
                    {
                        Edge newEdge = new Edge(transform, Nodes[randomNodeIndex].Go.transform, Nodes[randomNodeIndex2].Go.transform);
                        //Edges.Add(,newEdge);
                    }
                    break;
                case 3:
                    //Debug.Log("Changing Edges");
                    if (Edges.Count > 0 && Nodes.Count > 1)
                    {
                        Edges[randomEdgeIndex].A = Nodes[randomNodeIndex].Go.transform;
                        Edges[randomEdgeIndex].B = Nodes[randomNodeIndex2].Go.transform;
                    }
                    break;
            }
            yield return new WaitForEndOfFrame();
        }
    } 
    private Node randomNode()
    {
        string name = "";
        for (int j = 0; j < 10; j++)
        {
            name += chars[Random.Range(0, chars.Length)];
        }
        NodeInfo nodeinfo = new NodeInfo();
        nodeinfo.func = name;
        nodeinfo.x_pos = (int)randomVector3().x;
        nodeinfo.y_pos = (int)randomVector3().y;
        nodeinfo.z_pos = (int)randomVector3().z;
        return new Node(this.transform, nodeinfo);
    }
    private Vector3 randomVector3()
    {
        return new Vector3(Random.Range(-5f, 5f), Random.Range(0f, 5f), Random.Range(-5f, 5f));
    }
    void addRandomNodes()
    {
        for (int i = 0; i < maxNodes; i++)
        {
            //Nodes.Add(randomNode());
        }
    }
    void addEdgesBetweenRandomNodes()
    {
        for (int i = 0; i < Nodes.Count; i++)
        {
            int otherIndex;
            do
            {
                otherIndex = (int)Random.Range(0, Nodes.Count);
            } while (otherIndex == i);
            //Edges.Add(new Edge(transform,Nodes[i].Go.transform, Nodes[otherIndex].Go.transform));
        }
    }
    private void OnChanged(object sender, NotifyCollectionChangedEventArgs args)
    {

    }
    void Update()
    {
        AutoSize();
        //AutoPositionMinMax();
    }
    private void AutoPositionAvg()
    {
        //This uses all data to calculate an average position
        Vector3 nodePosSum = Vector3.zero;
        int nodeCount = 0;
        float minY = float.MaxValue;
        foreach (KeyValuePair<int, Node> node in Nodes)
        {
            if (!node.Value.node.isGrabbed)
            {
                if (node.Value.NodeInfo.y_pos < minY)
                    minY = node.Value.NodeInfo.y_pos;

                nodePosSum += node.Value.NodeInfo.position;
                nodeCount++;
            }
        }
        Vector3 avgNodePos = (nodePosSum / nodeCount);
        transform.localPosition = -avgNodePos * transform.localScale.x + new Vector3(0, (avgNodePos.y-minY)*transform.localScale.x,0);
    }
    private void AutoPositionMinMax()
    {
        Vector3 mins = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
        Vector3 maxes = new Vector3(float.MinValue, float.MinValue, float.MinValue);
        foreach (KeyValuePair<int, Node> node in Nodes)
        {
            if (!node.Value.node.isGrabbed)
            {
                if (node.Value.NodeInfo.x_pos < mins.x)
                    mins.x = node.Value.NodeInfo.x_pos;
                if (node.Value.NodeInfo.y_pos < mins.y)
                    mins.y = node.Value.NodeInfo.y_pos;
                if (node.Value.NodeInfo.z_pos < mins.z)
                    mins.z = node.Value.NodeInfo.z_pos;

                if (node.Value.NodeInfo.x_pos > maxes.x)
                    maxes.x = node.Value.NodeInfo.x_pos;
                if (node.Value.NodeInfo.y_pos > maxes.y)
                    maxes.y = node.Value.NodeInfo.y_pos;
                if (node.Value.NodeInfo.z_pos > maxes.z)
                    maxes.z = node.Value.NodeInfo.z_pos;
            }
        }
        maxes *= transform.localScale.x;
        mins *= transform.localScale.x;
        Vector3 avgNodePos = new Vector3((maxes.x+mins.x), (maxes.y + mins.y), (maxes.z + mins.z))/2;
        transform.localPosition = -avgNodePos + new Vector3(0, (avgNodePos.y - mins.y), 0);
    }
    bool sizing = false;
    Vector3 setToScale = Vector3.one;
    private void AutoSize()
    {
        // data mapping to min-max is a solution if this isnt sufficient
        if (sizing)
        {
            //transform.localScale = Vector3.Lerp(transform.localScale, setToScale, Time.deltaTime);
            if (transform.localScale.x < setToScale.x*1.05f)
                sizing = false;
        }/*
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
        if(Nodes.Count > 0)
            nodeSize = ((size.Avg()) / (Nodes.Count)) * (1/transform.localScale.Avg());
    }
}
