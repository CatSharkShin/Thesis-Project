using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using static UnityEngine.GraphicsBuffer;

namespace Decorators
{
    public class HiddenLayerDecorator : MonoBehaviour
    {
        public Node node;
        public Func<float, float> nodeFunction;
        public Func<float, string> functionString;
        public XRGrabInteractable grabbable;
        private void Awake()
        {
            StartCoroutine(Initialize());
        }
        private void Update()
        {
            TextMeshProUGUI tmp = transform.GetChild(0).GetChild(0).Find("weights").GetComponent<TextMeshProUGUI>();
            List<string> edges = node.node.nodeManager.GetLeftEdges(node.id).Select(edge => edge.RelationInfo.label).ToList();
            tmp.text = String.Join(",",edges);
        }
        IEnumerator Initialize()
        {
            while (grabbable == null)
                yield return null;
            grabbable.selectEntered.AddListener(Selected);
            grabbable.selectExited.AddListener(UnSelected);
        }
        public void Selected(SelectEnterEventArgs args)
        {
            StartCoroutine(AnimateText(true));
        }
        public void UnSelected(SelectExitEventArgs args)
        {
            StartCoroutine(AnimateText(false));
        }
        IEnumerator AnimateText(bool on)
        {
            TextMeshProUGUI tmp = transform.GetChild(0).GetChild(0).Find("calculation").GetComponent<TextMeshProUGUI>();
            tmp.text = "";
            float result = nodeFunction.Invoke(Weigthed()); // 0.67
            string calculationText = functionString.Invoke(node.NodeInfo.act); // relu(5)
            string weightingText = WeightingString();
            weightingText += " = " + Weigthed() + "\n";
            weightingText += calculationText;// + " = " + result;
            if (on)
            {
                foreach (char c in weightingText)
                {
                    tmp.text += c;
                    yield return new WaitForSeconds(0.1f);
                }
            }
        }
        private string WeightingString()
        {
            NetworkVisualiser nm = node.node.nodeManager;
            List<Edge> edges = nm.GetLeftEdges(node.id);
            List<string> lines = new List<string>();
            foreach(Edge edge in edges)
            {
                lines.Add(edge.RelationInfo.label + " * " + nm.Nodes[edge.cid1].NodeInfo.act);
            }

            return string.Join(" + ",lines);
        }
        private float Weigthed()
        {
            NetworkVisualiser nm = node.node.nodeManager;
            List<Edge> edges = nm.GetLeftEdges(node.id);

            float result = 0;
            foreach (Edge edge in edges)
            {
                result += float.Parse(edge.RelationInfo.label) * nm.Nodes[edge.cid1].NodeInfo.act;
            }

            return result;
        }
        /// <summary>
        /// Creates a Hidden Layer Decorator<br/>
        /// <br/>
        /// parent:<br/>
        /// This is the NODE it will be attached to.<br/>
        /// <br/>
        /// func:<br/>
        /// The function that should be applied to the number that we get after weighting<br/>
        /// <br/>
        /// funcstring:<br/>
        /// This should return the calculation description for the function<br/>
        /// For example: input float is 5, and the func that we call in the node is relu(x)<br/>
        /// Then the returned string is: relu(5)<br/>
        /// 
        /// </summary>
        /// <param name="parent"></param>
        /// This is the NODE it will be attached to.
        /// <param name="func"></param>
        /// The function that should be applied to the number that we get after weighting
        /// <param name="funcstring"></param>
        /// This should return the calculation description for the function
        /// For example: input float is 5, and the func that we call in the node is relu(x)
        /// Then the returned string is: relu(5)
        /// <returns></returns>
        public static HiddenLayerDecorator Create(Node node, Func<float, float> func,Func<float,string> funcstring)
        {
            GameObject Go = Instantiate(Resources.Load<GameObject>("Prefabs/Decorators/Hidden Decorator"));
            Go.transform.SetParent(node.Go.transform);
            Go.transform.localScale = new Vector3(1, 1, 1);
            HiddenLayerDecorator hld = Go.GetComponent<HiddenLayerDecorator>();
            hld.nodeFunction = func;
            hld.functionString = funcstring;
            hld.node = node;
            XRGrabInteractable grabbable = node.Go.GetComponent<XRGrabInteractable>();
            hld.grabbable = grabbable;
            ScaleOnGrab.Create(Go.transform.GetChild(0).gameObject, Vector3.zero, Vector3.one, grabbable);
            return hld;
        }
    }
}
