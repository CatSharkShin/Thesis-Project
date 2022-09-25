using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class DissolveFromObject : MonoBehaviour
{
    public Material material;
    public GameObject obj;
    void Start()
    {
        
    }
    void Update()
    {
        material.SetVector("_DissolveStartPoint", obj.transform.position);
    }
}
