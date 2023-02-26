using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HintVisual : MonoBehaviour
{
    public VisualizerSettings settings;
    public Transform target;
    public TextMeshPro tmp;
    public Vector3 offset;
    public float size;
    private void OnChange()
    {
        tmp.transform.gameObject.SetActive(settings.showHints);
        tmp.fontSize = settings.hintSize;
    }
    private void Start()
    {
        OnChange();
    }
    private void Update()
    {
        if (target == null)
            Destroy(this);
        Vector3 newPos = target.position;
        newPos += offset;
        transform.position = newPos;
        transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);
        transform.localScale = target.localScale*size;
    }
    public static HintVisual Create(Transform target,string text,Vector3 offset,float size = 1f)
    {
        GameObject go = new GameObject($"Hint: {text}");
        HintVisual hv = go.AddComponent<HintVisual>();
        hv.target = target;
        hv.settings = target.transform.GetParentComponent<VisualizerSettings>();
        hv.settings.onChange.AddListener(hv.OnChange);
        hv.tmp = go.AddComponent<TextMeshPro>();
        hv.tmp.text = text;
        hv.tmp.fontSize = hv.settings.hintSize;
        hv.tmp.horizontalAlignment = HorizontalAlignmentOptions.Center;
        hv.tmp.verticalAlignment = VerticalAlignmentOptions.Middle;
        hv.offset = offset;
        hv.size = size;
        return hv;
    }
}
