using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class EdgeRenderer : MonoBehaviour
{
    public Transform A;
    public Transform B;
    public LineRenderer lineRenderer;
    public RelationInfo relationInfo;
    public NetworkVisualiser nodeManager;
    Color baseColor = Color.white;
    Color glowColor = Color.magenta;
    private bool glow = false;
    public bool Glow
    {
        get { return glow; }
        set
        {
            glow = value;
            if (glow)
            {
                GlowEnum = GlowAnim();
                //StartCoroutine(GlowEnum);
            }
            else
            {
                //StopCoroutine(GlowEnum);
                lineRenderer.startColor = baseColor;
                lineRenderer.endColor = baseColor;
            }
        }
    }
    private IEnumerator GlowEnum;
    public EdgeRenderer(RelationInfo relationInfo)
    {
        this.relationInfo = relationInfo;
    }
    private void Awake()
    {
        lineRenderer = this.AddComponent<LineRenderer>();
        lineRenderer.material = Resources.Load<Material>("Materials/Edge");
    }
    int flow = 10;
    IEnumerator GlowAnim()
    {
        while (true)
        {
            for (int i = 0; i < flow; i++)
            {
                lineRenderer.startColor = Color.Lerp(lineRenderer.startColor, glowColor, (float)i /flow);
                yield return null;
            }
            for (int i = 0; i < flow; i++)
            {
                lineRenderer.endColor = Color.Lerp(lineRenderer.endColor, glowColor, (float)i / flow);
                yield return null;
            }
            for (int i = 0; i < flow; i++)
            {
                lineRenderer.startColor = Color.Lerp(lineRenderer.startColor, baseColor, (float)i / flow);
                yield return null;
            }
            for (int i = 0; i < flow; i++)
            {
                lineRenderer.startColor = Color.Lerp(lineRenderer.startColor, baseColor, (float)i / flow);
                yield return null;
            }
            /*
            while (!lineRenderer.startColor.Equals2(glowColor))
            {
                lineRenderer.startColor = Color.Lerp(lineRenderer.startColor, glowColor, Time.deltaTime*2);
                Debug.Log("1");
                yield return null;
            }
            while (!lineRenderer.endColor.Equals2(glowColor))
            {
                lineRenderer.endColor = Color.Lerp(lineRenderer.endColor, glowColor, Time.deltaTime * 2);
                Debug.Log("2");
                yield return null;
            }
            while (!lineRenderer.startColor.Equals2(baseColor))
            {
                lineRenderer.startColor = Color.Lerp(lineRenderer.startColor, baseColor, Time.deltaTime * 2);
                Debug.Log("3");
                yield return null;
            }
            while (!lineRenderer.endColor.Equals2(baseColor))
            {
                lineRenderer.endColor = Color.Lerp(lineRenderer.endColor, baseColor, Time.deltaTime * 2);
                Debug.Log("4");
                yield return null;
            }
            yield return new WaitForEndOfFrame();
            */
        }
    }
    void Update()
    {
        if (A == null || B == null || A == B)
        {
            GameObject.Destroy(transform.gameObject);
        }
        else
        {
            lineRenderer.SetPosition(0, A.transform.position);
            lineRenderer.SetPosition(1, B.transform.position);
            lineRenderer.startWidth = lineRenderer.endWidth = ((A.lossyScale.x + B.lossyScale.x) / 2) * nodeManager.edgeWidth;
            lineRenderer.startWidth *= 1f;//A.GetComponent<NodeBehaviour>().NodeInfo.act / A.GetComponent<NodeBehaviour>().NodeInfo.max;
            lineRenderer.endWidth *= 1f;//B.GetComponent<NodeBehaviour>().NodeInfo.act / A.GetComponent<NodeBehaviour>().NodeInfo.max;
        }
        if (glow)
        {
            GlowEnum.MoveNext();
        }
    }
}
