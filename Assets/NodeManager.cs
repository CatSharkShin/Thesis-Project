using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Xml.Linq;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;
public class Node : IDisposable
{
    public NodeBehaviour node;
    public GameObject Go { get; }
    public Node(Transform parent,string name,Vector3 position)
    {
        Go = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Node"), parent, false);
        Go.name = name;
        Go.transform.SetParent(parent);
        node = Go.GetComponent<NodeBehaviour>();
        node.position = position;
        node.Name = name;
    }
    public Node(Transform parent, string name, Vector3 position,float size):this(parent,name,position)
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

    public EdgeRenderer edge;

    public Edge(Transform parent)
    {
        Go = new GameObject();
        Go.transform.SetParent(parent);
        edge = Go.AddComponent<EdgeRenderer>();
    }
    public Edge(Transform parent,Transform A,Transform B):this(parent)
    {
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
    public ObservableCollection<Node> Nodes { get; }
    public ObservableCollection<Edge> Edges { get; }
    public int maxNodes;
    public int maxEdges;
    public float update;
    public float nodeSize;
    public NodeManager()
    {
        Nodes = new ObservableCollection<Node>();
        Edges = new ObservableCollection<Edge>();
        Nodes.CollectionChanged += OnChanged;
    }
    void Start()
    {
        addRandomNodes();
        addEdgesBetweenRandomNodes();
        StartCoroutine(doRandomStuff());
    }
    private IEnumerator doRandomStuff()
    {
        while (true)
        {
            Debug.Log($"Nodes: {Nodes.Count}, Edges: {Edges.Count}");
            int action = Random.Range(0, 6);
            int randomNodeIndex = Random.Range(0, Nodes.Count);
            int randomNodeIndex2 = Random.Range(0, Nodes.Count);
            int randomEdgeIndex = Random.Range(0, Edges.Count);

            if(Nodes.Count < maxNodes)
            {
                Nodes.Add(randomNode());
            }else if (Nodes.Count > maxNodes)
            {
                Nodes[randomNodeIndex].Dispose();
                Nodes.Remove(Nodes[randomNodeIndex]);
                randomNodeIndex = Random.Range(0, Nodes.Count);
                randomNodeIndex2 = Random.Range(0, Nodes.Count);
            }
            switch (action)
            {
                case 0:
                    Debug.Log("Changing Node");
                    if (Nodes.Count > 0)
                        Nodes[randomNodeIndex].node.position = randomVector3();
                    break;
                case 1:
                    Debug.Log("Deleting Edges");
                    if (Edges.Count > 0)
                    {
                        Edges[randomEdgeIndex].Dispose();
                        Edges.Remove(Edges[randomEdgeIndex]);
                    }
                    break;
                case 2:
                    Debug.Log("Adding Edges");
                    if (Nodes.Count > 1 && Edges.Count < maxEdges)
                    {
                        Edge newEdge = new Edge(transform);
                        newEdge.edge.A = Nodes[randomNodeIndex].Go.transform;
                        newEdge.edge.B = Nodes[randomNodeIndex2].Go.transform;
                        Edges.Add(newEdge);
                    }
                    break;
                case 3:
                    Debug.Log("Changing Edges");
                    if (Edges.Count > 0 && Nodes.Count > 1)
                    {
                        Edges[randomEdgeIndex].edge.A = Nodes[randomNodeIndex].Go.transform;
                        Edges[randomEdgeIndex].edge.B = Nodes[randomNodeIndex2].Go.transform;
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
        return new Node(this.transform, name, randomVector3(),nodeSize);
    }
    private Vector3 randomVector3()
    {
        return new Vector3(Random.Range(-5f, 5f), Random.Range(0f, 5f), Random.Range(-5f, 5f));
    }
    void addRandomNodes()
    {
        for (int i = 0; i < maxNodes; i++)
        {
            Nodes.Add(randomNode());
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
            Edges.Add(new Edge(transform,Nodes[i].Go.transform, Nodes[otherIndex].Go.transform));
        }
    }
    private void OnChanged(object sender, NotifyCollectionChangedEventArgs args)
    {

    }
    void Update()
    {

    }
}
