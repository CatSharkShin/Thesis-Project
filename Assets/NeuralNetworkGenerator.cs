using System.Collections;
using System.Collections.Generic;
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
        // Iterate thru network blueprints
        int nodeid = 0;
        foreach (NetworkBlueprint network in networkBlueprints)
        {
            Vector3 min = new Vector3((float)int.Parse(network.minX.text), (float)int.Parse(network.minY.text), (float)int.Parse(network.minZ.text));
            Vector3 max = new Vector3((float)int.Parse(network.maxX.text), (float)int.Parse(network.maxY.text), (float)int.Parse(network.maxZ.text));

            int networkid = int.Parse(network.NetworkID.text);
            if (network.Validate()) { 
                for (int i = 0; i < network.NodeCount.value; i++)
                {
                    NodeInfo newNI = new NodeInfo();
                    newNI.networkID = networkid;
                    newNI.nodeID = nodeid;
                    if (network.Spherical.isOn) {
                        newNI.position = Random.insideUnitSphere * (float)int.Parse(network.radius.text);
                    }
                    else
                    {
                        newNI.position = new Vector3(
                            Random.Range(min.x, max.x),
                            Random.Range(min.y, max.y),
                            Random.Range(min.z, max.z));
                    }
                    nodeInfos.Add(newNI);
                    nodeid++;
                }
            }
        }
    }
    public void Generate()
    {
        // Call UpdateInfo
        UpdateInfo();
        // Assign NodeInfos and Relations to Visualiser
        networkVisualizer.MatchNetwork(nodeInfos);
    }
}
