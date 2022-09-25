using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Http;
using Unity.VisualScripting;
using System;
using System.IO;
using System.Net;
using Newtonsoft.Json;
public class NodeInfo
{
    public int uid { get; set; }
    public int id { get; set; }
    public int hid { get; set; }
    public string func { get; set; }
    public int max { get; set; }
    public int act { get; set; }
    public int x_pos { get; set; }
    public int y_pos { get; set; }
    public int z_pos { get; set; }
    public string model { get; set; }
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
    private static readonly HttpClient httpClient = new HttpClient();
    public static List<NodeInfo> GetNetwork()
    {
        string result = Get("http://127.0.0.1:8000/network");
        Debug.Log(result);
        return JsonConvert.DeserializeObject<List<NodeInfo>>(result);
    }
    public static List<RelationInfo> GetRelations()
    {
        string result = Get("http://127.0.0.1:8000/relations");
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
