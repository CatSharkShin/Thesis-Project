using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class NodeBehaviour : MonoBehaviour
{
    private TextMeshPro UIName;

    public string Name
    {
        get { return UIName.text; }
        set { UIName.text = value; }
    }
    public Vector3 position;
    private XRReSnapGrabbable interactable;
    
    void Awake()
    {
        interactable = this.GetComponent<XRReSnapGrabbable>();
        UIName = transform.GetChild(1).GetComponent<TextMeshPro>();
        this.transform.localPosition = position;
    }

    void Update()
    {
        if(!interactable.isSelected)
            this.transform.localPosition = Vector3.Lerp(transform.localPosition,position,Time.deltaTime*1.5f);
    }
}
