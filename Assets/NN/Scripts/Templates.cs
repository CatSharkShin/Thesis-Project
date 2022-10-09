using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO:
/*
Comment all this
create a full static non monobehaviour version, because the loaded resources may not have to be initiated
i was just using the wrong path
bruh
if they need to be initiated put the monobehaviour in the scene dummy
*/
public class Templates
{
    //Properties with {get; private set;}
    //They will be loaded according to the current template
    public static GameObject Node;
    public static Mesh NodeMesh{
        get {return Node.transform.GetChild(0).GetComponent<MeshFilter>().sharedMesh;}
        private set {
            Transform node = Node.transform.GetChild(0);
            node.GetComponent<MeshFilter>().sharedMesh = value;
            }
    }
    public Templates()
    {
        AssignDefaults();
        SyncResources();
        RefreshTemplateList();
    }
    public static void Test(){
        // Instantiate the Prefabs you want to use under the Template object and change it there
    }
    public static void AssignDefaults(){
        // Assign default values, load in default assets for all properties
        Node = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Node"));

        NodeMesh = Resources.Load<Mesh>("Template/lowpolytree");
    }
    public static void SyncResources(){
        // Sync the resource files that are on the server
    }
    public static void RefreshTemplateList(){
        // Clear templates
        // Get the templates from the server
    }
}
/*
public class Templates : MonoBehaviour
{
    //Properties with {get; private set;}
    //They will be loaded according to the current template

    public static Templates Instance;
    public static GameObject Node;
    public static Mesh NodeMesh{
        get {return Node.transform.GetChild(0).GetComponent<MeshFilter>().sharedMesh;}
        private set {
            Transform node = Node.transform.GetChild(0);
            node.GetComponent<MeshFilter>().sharedMesh = value;
            }
    }
    void Awake(){
        Instance = this;
    }
    void Start(){
        Test();
        AssignDefaults();
        SyncResources();
        RefreshTemplateList();
    }
    public static void Test(){
        // Instantiate the Prefabs you want to use under the Template object and change it there

        Node = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Node"),Instance.transform);

        NodeMesh = Resources.Load<Mesh>("Template/lowpolytree.fbx");
    }
    public static void AssignDefaults(){
        // Assign default values, load in default assets for all properties
    }
    public static void SyncResources(){
        // Sync the resource files that are on the server
    }
    public static void RefreshTemplateList(){
        // Clear templates
        // Get the templates from the server
    }
}
*/