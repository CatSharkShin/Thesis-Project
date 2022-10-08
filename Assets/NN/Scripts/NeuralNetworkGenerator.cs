using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class NeuralNetworkGenerator : MonoBehaviour
{
    public NetworkVisualiser networkVisualizer;
    public Button addNetwork;
    public Button exportSQL;
    public Transform networksParent;

    public List<NetworkBlueprint> networkBlueprints;

    public List<RelationInfo> relationInfos = new List<RelationInfo>();
    public List<NodeInfo> nodeInfos = new List<NodeInfo>();

    public int seed;
    void Start()
    {
        seed = (int)System.DateTime.Now.Ticks;
        addNetwork.onClick.AddListener(() => AddNetwork());
        exportSQL.onClick.AddListener(() => ExportSql());
    }
    void OnValueChanged()
    {
        // Call Generate
        Generate();
    }
    void AddNetwork()
    {
        // Duplicate first child in networksParent -
        Transform newNetwork = GameObject.Instantiate(networksParent.GetChild(0),networksParent);
        newNetwork.gameObject.SetActive(true);
        NetworkBlueprint newNetworkBlueprint = newNetwork.GetComponent<NetworkBlueprint>();
        newNetworkBlueprint.neuralNetworkGenerator = this;
        // Set ID to childcount -1 (cuz template takes up one) -
        newNetworkBlueprint.NetworkID.text = (networksParent.childCount-1).ToString();
        // Add reference of NetworkBueprint to the list -
        networkBlueprints.Add(newNetworkBlueprint);
        // Assign OnValueChanged events -
        newNetworkBlueprint.AddListener(() => OnValueChanged());
    }
    void ExportSql()
    {
        // Call UpdateInfo
        // create sql/json and export
    }
    void UpdateInfo()
    {
        Random.InitState(seed);
        // Clear NodeInfos
        nodeInfos.Clear();
        relationInfos.Clear();
        // Iterate thru network blueprints
        int nodeid = 0;
        int relationid = 0;
        foreach (NetworkBlueprint network in networkBlueprints)
        {
            Vector3 min = new Vector3((float)int.Parse(network.minX.text), (float)int.Parse(network.minY.text), (float)int.Parse(network.minZ.text));
            Vector3 max = new Vector3((float)int.Parse(network.maxX.text), (float)int.Parse(network.maxY.text), (float)int.Parse(network.maxZ.text));
            Vector3 offset = new Vector3((float)int.Parse(network.offsetX.text), (float)int.Parse(network.offsetY.text), (float)int.Parse(network.offsetZ.text));

            int networkid = int.Parse(network.NetworkID.text);
            List<NodeInfo> currentNodes = new List<NodeInfo>();
            for (int i = 0; i < network.NodeCount.value; i++)
            {
                NodeInfo newNI = new NodeInfo();
                newNI.networkID = networkid;
                newNI.nodeID = nodeid;
                if (network.Spherical.isOn) {
                    newNI.position = (Random.insideUnitSphere * (float)int.Parse(network.radius.text))+offset;
                }
                else
                {
                    newNI.position = new Vector3(
                        Random.Range(min.x, max.x) + offset.x,
                        Random.Range(min.y, max.y) + offset.y,
                        Random.Range(min.z, max.z) + offset.z);
                }
                nodeInfos.Add(newNI);
                currentNodes.Add(newNI);
                nodeid++;
            }
            /*
                 
            foreach (NodeInfo nodeInfo in currentNodes)
            {
                for (int i = 0; i < network.RelationCount.value; i++)
                {
                    int cid2;
                    do
                    {
                        cid2 = currentNodes[Random.Range(0, currentNodes.Count)].nodeID;
                    } while (cid2 == nodeInfo.nodeID);

                    RelationInfo newRelationInfo = new RelationInfo();
                    newRelationInfo.id = relationid;
                    newRelationInfo.cid1 = nodeInfo.nodeID;
                    newRelationInfo.cid2 = cid2;
                    relationInfos.Add(newRelationInfo);
                    relationid++;
                }
            }
                */

        }
        foreach (NetworkBlueprint network in networkBlueprints)
        {
            if (network.NodeCount.value < 2)
                continue;
            List<NodeInfo> currentNodes = nodeInfos.Where(n => n.networkID == int.Parse(network.NetworkID.text)).ToList();
            foreach (NodeInfo nodeInfo in currentNodes)
            {
                for (int i = 0; i < network.RelationCount.value; i++)
                {
                    int cid2;
                    do
                    {
                        cid2 = currentNodes[Random.Range(0, currentNodes.Count)].nodeID;
                    } while (cid2 == nodeInfo.nodeID);

                    RelationInfo newRelationInfo = new RelationInfo();
                    newRelationInfo.id = relationid;
                    newRelationInfo.cid1 = nodeInfo.nodeID;
                    newRelationInfo.cid2 = cid2;
                    relationInfos.Add(newRelationInfo);
                    relationid++;
                }
            }
        }
    }
    public bool NetworksValid()
    {
        foreach(NetworkBlueprint network in networkBlueprints)
        {
            if (!network.Validate())
                return false;
        }
        return true;
    }
    public void Generate()
    {
        if (NetworksValid())
        {
            UpdateInfo();
            networkVisualizer.MatchNetwork(nodeInfos);
            networkVisualizer.MatchRelations(relationInfos);
        }
    }
}
