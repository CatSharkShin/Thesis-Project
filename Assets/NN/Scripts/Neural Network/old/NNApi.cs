using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Http;
using Unity.VisualScripting;
using System;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Runtime.InteropServices;

namespace NeuralNetwork
{
    public abstract class SQLReady
    {
        public abstract string Table { get; }
        public virtual string ToSQL()
        {
            Func<PropertyInfo, bool> matching =
                    property => !property.GetCustomAttributes(typeof(JsonIgnoreAttribute), false)
                                        .Any();

            List<string> COLS = new List<string>();
            List<string> VALUES = new List<string>();

            foreach (PropertyInfo prop in this.GetType().GetProperties().Where(matching))
            {
                var obj = prop.GetValue(this);
                string val = obj == null ? " " : obj.ToString();
                COLS.Add($"`{prop.Name}`");
                VALUES.Add($"'{val}'");
            }

            return $"INSERT INTO `{Table}` ({string.Join(", ", COLS)}) VALUES ({string.Join(", ", VALUES)});";
        }
    }

    public class NodeInfo : SQLReady
    {
        [JsonIgnore]
        public override string Table { get { return "networkdata"; } }
        public int nodeID { get; set; }
        public int funcID { get; set; }
        public int networkID { get; set; }
        public string func { get; set; }
        public float max { get; set; }
        public float act { get; set; }
        public float x_pos { get; set; }
        public float y_pos { get; set; }
        public float z_pos { get; set; }
        public string model { get; set; }
        [JsonIgnore]
        public Vector3 position
        {
            get
            {
                return new Vector3(x_pos, y_pos, z_pos);
            }
            set
            {
                x_pos = value.x;
                y_pos = value.y;
                z_pos = value.z;

            }
        }
        public NodeInfo()
        {

        }
        public NodeInfo(int nodeID, int networkID = 0, float x_pos = 0, float y_pos = 0, float z_pos = 0,
        int funcID = 0, string func = "", float max = 0, float act = 0, string model = "")
        {
            this.nodeID = nodeID;
            this.networkID = networkID;
            this.x_pos = x_pos;
            this.y_pos = y_pos;
            this.z_pos = z_pos;
            this.funcID = funcID;
            this.func = func;
            this.max = max;
            this.act = act;
            this.model = model;
        }
    }
    public class EdgeInfo : SQLReady
    {
        [JsonIgnore]
        public override string Table { get { return "relations"; } }
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
        public EdgeInfo() { }
        public EdgeInfo(int id, int cid1, int cid2,
        string label = "")
        {
            this.cid1 = cid1;
            this.cid2 = cid2;
            this.id = id;
            this.label = label;
        }
    }
    public class Api
    {
        private static readonly string url = "https://snetwork.uni-eszterhazy.hu/";
        //private static readonly string url = "http://localhost:8000/";
        private static readonly HttpClient httpClient = new HttpClient();
        public static List<NodeInfo> GetNetwork()
        {
            string endpoint = "network";
            string result = Get(url + endpoint);
            return JsonConvert.DeserializeObject<List<NodeInfo>>(result);
        }
        public static List<EdgeInfo> GetRelations()
        {
            string endpoint = "relations";
            string result = Get(url + endpoint);
            return JsonConvert.DeserializeObject<List<EdgeInfo>>(result);
        }
        public static string Get(string uri)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }

}