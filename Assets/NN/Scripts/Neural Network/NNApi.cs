using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Http;
using Unity.VisualScripting;
using System;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using Thry;

public class NodeInfo
{
    public int nodeID { get; set; }
    public int networkID { get; set; }
    public int funcID { get; set; }
    public string func { get; set; }
    public float max { get; set; }
    public int act { get; set; }
    public float x_pos { get; set; }
    public float y_pos { get; set; }
    public float z_pos { get; set; }
    public string model { get; set; }
    public Vector3 position {
        get {
            return new Vector3(x_pos, y_pos, z_pos);
        } 
        set {
            x_pos = value.x;
            y_pos = value.y;
            z_pos = value.z;

        } 
    }
}
public class RelationInfo
{
    public int id { get; set; }
    public int cid1 { get; set; }
    public int cid2 { get; set; }
    public string label { get; set; }
    public int x_pos1 { get; set; }
    public int y_pos1 { get; set; }
    public int z_pos1 { get; set; }
    public int x_pos2 { get; set; }
    public int y_pos2 { get; set; }
    public int z_pos2 { get; set; }
}
public class NNApi
{
    private static readonly string url = "https://snetwork.uni-eszterhazy.hu/";
    //private static readonly string url = "http://localhost:8000/";
    private static readonly HttpClient httpClient = new HttpClient();
    public static List<NodeInfo> GetNetwork()
    {
        string endpoint = "network";
        string result = Get(url+endpoint);
        Debug.Log(result);
        return JsonConvert.DeserializeObject<List<NodeInfo>>(result);
    }
    public static List<RelationInfo> GetRelations()
    {
        string endpoint = "relations";
        string result = Get(url + endpoint);
        Debug.Log(result);
        return JsonConvert.DeserializeObject<List<RelationInfo>>(result);
    }
    public static string Get(string uri)
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);

        using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
        using (Stream stream = response.GetResponseStream())
        using (StreamReader reader = new StreamReader(stream))
        {
            return reader.ReadToEnd();
        }
    }
}
