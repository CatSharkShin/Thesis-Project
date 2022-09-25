using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour
{
    public string targetName;
    private Transform target;
    private void Awake()
    {
        target = GameObject.Find(targetName).transform;
    }
    void Update()
    {
        transform.LookAt(2*transform.position-target.position);
    }
}
