using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;

public class EdgeDecorator : MonoBehaviour
{
    public EdgeRenderer edgeRenderer;
    private TextMeshPro weightLabel;
    private TextMeshPro valueLabel;
    public bool sideA = true;
    public string value;
    private float currentValue = 0;
    private bool animate = false;
    void Start()
    {
        
    }
    private void Awake()
    {
        StartCoroutine(InitializeLabels());
    }
    IEnumerator InitializeLabels()
    {
        while (edgeRenderer == null)
            yield return new WaitForEndOfFrame();

        var tempGo = new GameObject("Weight Label");
        tempGo.transform.parent = edgeRenderer.transform;
        weightLabel = tempGo.AddComponent<TextMeshPro>();
        weightLabel.horizontalAlignment = HorizontalAlignmentOptions.Center;
        weightLabel.verticalAlignment = VerticalAlignmentOptions.Middle;
        weightLabel.margin = new Vector4(0, 0, 0, 0.5f);

        tempGo = new GameObject("Value Label");
        tempGo.transform.parent = edgeRenderer.transform;
        valueLabel = tempGo.AddComponent<TextMeshPro>();
        valueLabel.horizontalAlignment = HorizontalAlignmentOptions.Center;
        valueLabel.verticalAlignment = VerticalAlignmentOptions.Middle;
        valueLabel.margin = new Vector4(0, 0, 0, 0.5f);
    }
    void Update()
    {
        if(edgeRenderer != null && valueLabel != null)
        {
            RelationInfo relationInfo = edgeRenderer.relationInfo;
            NetworkVisualiser nodeManager = edgeRenderer.nodeManager;
            LineRenderer lineRenderer = edgeRenderer.lineRenderer;
            if(relationInfo != null)
            {
                if (float.TryParse(value, out float newValue))
                {
                    if(animate)
                        currentValue = Mathf.Lerp(currentValue, newValue, Time.deltaTime * 2f);
                    else 
                        currentValue = newValue;
                    valueLabel.text = currentValue.ToString("0.00");
                }
                else
                {
                    valueLabel.text = value;
                }

                float tToTarget = sideA ? 0.25f : 1f;
                Vector3 positionToMoveTo = Vector3.Lerp(lineRenderer.GetPosition(0), lineRenderer.GetPosition(1), tToTarget);
                if (animate)
                    valueLabel.transform.position = Vector3.Lerp(valueLabel.transform.position, positionToMoveTo, Time.deltaTime * 2f);
                else
                    valueLabel.transform.position = positionToMoveTo;

                weightLabel.text = relationInfo.label;
                weightLabel.transform.position = Vector3.Lerp(lineRenderer.GetPosition(0), lineRenderer.GetPosition(1), 0.75f);

                Quaternion rotate = LinePerpendicular();

                weightLabel.transform.rotation = rotate;
                valueLabel.transform.rotation = rotate;


                weightLabel.fontSize = nodeManager.edgeLabelSize * nodeManager._nodeSize;
                valueLabel.fontSize = nodeManager.edgeLabelSize * nodeManager._nodeSize;

                weightLabel.margin = new Vector4(0,0,0,nodeManager.edgeLabelSize * nodeManager._nodeSize * 0.2f);
                valueLabel.margin = new Vector4(0, 0, 0, nodeManager.edgeLabelSize * nodeManager._nodeSize * 0.2f);


                if (Vector3.Distance(valueLabel.transform.position,positionToMoveTo) < 0.01)
                {
                    animate = false;
                }
            }
        }
    }
    public void SetValues(string newvalue, bool newsideA = true)
    {
        animate = false;
        this.value = newvalue;
        this.sideA = newsideA;
    }
    public void AnimateValues(string newvalue, bool newsideA = true)
    {
        animate = true;
        this.value = newvalue;
        this.sideA = newsideA;
    }
    float LabelToFloat(string label)
    {
        if (label == "")
        {
            return 1;
        }
        else
        {
            return float.Parse(label, CultureInfo.InvariantCulture.NumberFormat);
        }
    }
    Quaternion LinePerpendicular()
    {
        LineRenderer lineRenderer = edgeRenderer.lineRenderer;
        weightLabel.transform.position = Vector3.Lerp(lineRenderer.GetPosition(0), lineRenderer.GetPosition(1), 0.75f);
        Vector3 directionVector = lineRenderer.GetPosition(1) - lineRenderer.GetPosition(0);
        bool LeftToRight = Camera.main.WorldToScreenPoint(lineRenderer.GetPosition(0)).x < Camera.main.WorldToScreenPoint(lineRenderer.GetPosition(1)).x;
        Vector3 from = Vector3.right;
        if (!LeftToRight)
        {
            directionVector = -directionVector;
        }
        Quaternion rotate = Quaternion.FromToRotation(from, directionVector.normalized);
        return rotate;
    }
}
