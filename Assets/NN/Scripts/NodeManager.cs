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
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class NodeObject : IDisposable
{
    public int id { get; }
    public GameObject Go { get; }
    public NodeBehaviour node;
    public NodeInfo NodeInfo
    {
        get { return node.NodeInfo; }
        set { node.NodeInfo = value; }
    }
    public NodeObject(Transform parent,NodeInfo nodeinfo)
    {
        this.id = nodeinfo.uid;
        Go = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Node"), parent, false);
        node = Go.GetComponent<NodeBehaviour>();
        node.NodeInfo = nodeinfo;
        node.nodeManager = parent.GetComponent<NodeManager>();
        Go.transform.SetParent(parent);
    }
    public NodeObject(Transform parent,NodeInfo nodeinfo,float size):this(parent,nodeinfo)
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
        edge.nodeManager = parent.GetComponent<NodeManager>();
        edge.A = edge.nodeManager.Nodes[relationInfo.cid1].Go.transform;
        edge.B = edge.nodeManager.Nodes[relationInfo.cid2].Go.transform;
    }
    public Edge(Transform parent, Transform A, Transform B)
    {
        Go = new GameObject("EdgeRenderer");
        Go.transform.SetParent(parent);
        edge = Go.AddComponent<EdgeRenderer>();
        edge.nodeManager = parent.GetComponent<NodeManager>();
        edge.A = A;
        edge.B = B;
    }
    public void Dispose()
    {
        GameObject.Destroy(Go);
    }
}
public class NodeManager : MonoBehaviour
{
    const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

    public Dictionary<int, NodeObject> Nodes { get; }
    public Dictionary<int, Edge> Edges { get; }

    //public Dictionary<int, Color> NetworkColors { get; }
    public List<int> Networks { get; }

    public int maxNodes;
    public int maxEdges;
    public float update;
    public float nodeSize;
    public float edgeWidth;
    public float maxDistance;
    public NodeManager()
    {
        Nodes = new Dictionary<int,NodeObject>();
        Edges = new Dictionary<int,Edge>();
        //NetworkColors = new Dictionary<int, Color>();
        Networks = new List<int>();
    }
    void Start()
    {
        //StartCoroutine(useRandomGeneration());
        MatchNetwork();
        MatchRelations();
    }
    private void MatchNetwork()
    {
        List<NodeInfo> nodeinfos = NNApi.GetNetwork();
        foreach (NodeInfo nodeinfo in nodeinfos)
        {
            /*if (!NetworkColors.ContainsKey(nodeinfo.id))
            {
                NetworkColors.Add(nodeinfo.id, Random.ColorHSV());
            }*/
            if (!Networks.Contains(nodeinfo.id))
            {
                Networks.Add(nodeinfo.id);
            }
            if (Nodes.TryGetValue(nodeinfo.uid,out NodeObject node))
            {
                node.NodeInfo = nodeinfo;
            }
            else
            {
                Nodes.Add(nodeinfo.uid, new NodeObject(transform, nodeinfo));
            }
        }
    }
    private void MatchRelations()
    {
        List<RelationInfo> relationinfos = NNApi.GetRelations();
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
    private NodeObject randomNode()
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
        return new NodeObject(this.transform, nodeinfo);
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
    }
    bool sizing = false;
    Vector3 setToScale;
    private void AutoSize()
    {
        if (sizing)
        {
            // I should probably set the scaling size depending on the acquired data from the server
            // since the data inside the visualization keeps changing from many things, movement, grabbing etc.
            // Or checking the nodes' desired position would be better too since that doesn't change when grabbing
            transform.localScale = Vector3.Lerp(transform.localScale, setToScale, Time.deltaTime);
            if (transform.localScale.x < setToScale.x*1.05f)
                sizing = false;
        }
        else
        {
        }
        float mDistance = 0;
        foreach (KeyValuePair<int, NodeObject> node in Nodes)
        {
            if (!node.Value.node.isGrabbed)
            {
                float distance = Vector3.Distance(node.Value.node.position, Vector3.zero);
                if (distance > mDistance)
                    mDistance = distance;
            }
        }
        if (mDistance > maxDistance || mDistance < maxDistance*0.95f)
        {
            float newScale = ((maxDistance / mDistance));
            setToScale = new Vector3(newScale, newScale, newScale);
            sizing = true;
        }
        nodeSize = ((maxDistance) / (Nodes.Count*2)) * (1/transform.localScale.x);
    }
}
