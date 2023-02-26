using Decorators;
using NetworkAnimations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Networking.Types;

public class PerceptronAnimator : AnimatorController
{
    public NetworkVisualiser nw;
    public override void Initialize()
    {
        InitializeNetwork();
        animators.Add(new ShowAnimator(1f));
        AddAnimatable(NodeValueDecorator.Create(nw.Nodes[0], 1f));
        AddAnimatable(NodeValueDecorator.Create(nw.Nodes[1], 1f));
        AddAnimatable(NodeValueDecorator.Create(nw.Nodes[2], 1f));
        AddAnimatable(WeightDecorator.Create(nw.Edges[0]));
        AddAnimatable(WeightDecorator.Create(nw.Edges[1]));
        AddAnimatable(WeightDecorator.Create(nw.Edges[2]));
        ShowAnimator valueslides = new ShowAnimator(1f);
        animators.Add(valueslides);
        AddAnimatable(ValueDecorator.Create(nw.Edges[0]));
        AddAnimatable(ValueDecorator.Create(nw.Edges[1]));
        AddAnimatable(ValueDecorator.Create(nw.Edges[2]));
        animators.Add(new AnimateAnimator(2f,valueslides.animatables));
        animators.Add(new ShowAnimator(1f));
        AddAnimatable(NodeValueDecorator.Create(nw.Nodes[3],1f));
        animators.Add(new ShowAnimator(5f));

        ShowAnimator outputedge = new ShowAnimator(1f);
        animators.Add(outputedge);
        AddAnimatable(ValueDecorator.Create(nw.Edges[3]));
        animators.Add(new AnimateAnimator(1f, outputedge.animatables));
        animators.Add(new ShowAnimator(1f));
        AddAnimatable(NodeValueDecorator.Create(nw.Nodes[4],1f));

        animators.Add(new ShowAnimator(10f));
        StartCoroutine(Wait());
    }
    IEnumerator Wait()
    {
        yield return new WaitForSeconds(5f);
        Animate();
    }
    void AddAnimatable(IAnimatable anim)
    {
        animators[animators.Count - 1].AddAnimatable(anim);
    }
    void InitializeNetwork()
    {

        List<NodeInfo> network = new List<NodeInfo>();
        NodeInfo[] n = new NodeInfo[]{
            //IN
            new NodeInfo(0,0, 0,0.5f,0,  0, "", max:0.1f, act:0.1f),
            new NodeInfo(1,0, 0,0f,0,  0, "", 0.5f, 0.5f),
            new NodeInfo(2,0, 0,-0.5f,0,  0, "", 0.2f, 0.2f),
            
            //Hidden
            new NodeInfo(3,1, 2,0,0,  0, "", 0.26f, 0.26f),

            //Output
            new NodeInfo(4,2, 4,0f,0,  0, "", 0, 0.26f),
        };
        nw.MatchNetwork(new List<NodeInfo>(n));

        HiddenLayerDecorator.Create(nw.Nodes[3], Unchanged, EmptyString);
        NodeBoolDecorator.Create(nw.Nodes[4],PerceptronCond);
        Vector3 offset = new Vector3(0, 0.15f, 0);
        float size = 3f;
        HintVisual.Create(nw.Nodes[0].Go.transform, "Input Neuron", offset, size);
        HintVisual.Create(nw.Nodes[1].Go.transform, "Input Neuron", offset, size);
        HintVisual.Create(nw.Nodes[2].Go.transform, "Input Neuron", offset, size);

        HintVisual.Create(nw.Nodes[3].Go.transform, "Hidden Neuron", offset, size);

        HintVisual.Create(nw.Nodes[4].Go.transform, "Output Neuron", offset, size);

        //nw.Nodes[4].node.gameObject.GetNamedChild("Neuron Data").GetComponent<NeuronDataDisplayer>().calculationText =
        //    "0.1*0.3\r\n+\r\n0.13*0.5\r\n+\r\n=0.03+0.065 = 0.095";

        RelationInfo[] r = new RelationInfo[]{
            // IN -> H
            new RelationInfo(0,0,3,"0.4"),
            new RelationInfo(1,1,3,"0.2"),
            new RelationInfo(2,2,3,"0.6"),

            new RelationInfo(3,3,4,""),
        };
        nw.MatchRelations(new List<RelationInfo>(r));
    }
    bool PerceptronCond(float x)
    {
        return x > 5;
    }
    float Unchanged(float x)
    {
        return x;
    }
    string EmptyString(float x)
    {
        return "";
    }
}
