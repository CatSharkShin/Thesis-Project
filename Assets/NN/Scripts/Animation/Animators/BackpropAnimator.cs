using Decorators;
using NetworkAnimations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Networking.Types;

public class BackpropAnimator : AnimatorController
{
    public NetworkVisualiser nw;
    public TextMeshPro title;
    public override void Initialize()
    {
        InitializeNetwork();
        TextChanger tc = new TextChanger(title, "Forward propagation", "Backpropagation");
        animators.Add(new ShowAnimator(1f));
        AddAnimatable(NodeValueDecorator.Create(nw.Nodes[0], nw.Nodes[0].NodeInfo.act));
        AddAnimatable(NodeValueDecorator.Create(nw.Nodes[1], nw.Nodes[1].NodeInfo.act));
        AddAnimatable(WeightDecorator.Create(nw.Edges[0]));
        AddAnimatable(WeightDecorator.Create(nw.Edges[1]));
        AddAnimatable(WeightDecorator.Create(nw.Edges[2]));
        AddAnimatable(WeightDecorator.Create(nw.Edges[3]));
        AddAnimatable(WeightDecorator.Create(nw.Edges[4]));
        AddAnimatable(WeightDecorator.Create(nw.Edges[5]));

        ShowAnimator firstValDec = new ShowAnimator(1f);
        animators.Add(firstValDec);
        AddAnimatable(ValueDecorator.Create(nw.Edges[0]));
        AddAnimatable(ValueDecorator.Create(nw.Edges[1]));
        AddAnimatable(ValueDecorator.Create(nw.Edges[2]));
        AddAnimatable(ValueDecorator.Create(nw.Edges[3]));
        animators.Add(new AnimateAnimator(2f));
        AddAnimatable(firstValDec.animatables[0]);
        AddAnimatable(firstValDec.animatables[1]);
        animators.Add(new ShowAnimator(1f));
        AddAnimatable(NodeValueDecorator.Create(nw.Nodes[2], nw.Nodes[2].NodeInfo.act));

        //DELAY
        animators.Add(new ShowAnimator(7f));

        animators.Add(new AnimateAnimator(2f));
        AddAnimatable(firstValDec.animatables[2]);
        AddAnimatable(firstValDec.animatables[3]);
        animators.Add(new ShowAnimator(1f));
        AddAnimatable(NodeValueDecorator.Create(nw.Nodes[3], nw.Nodes[3].NodeInfo.act));

        ShowAnimator valDec = new ShowAnimator(1f);
        animators.Add(valDec);
        AddAnimatable(ValueDecorator.Create(nw.Edges[4]));
        AddAnimatable(ValueDecorator.Create(nw.Edges[5]));

        animators.Add(new AnimateAnimator(2f, valDec.animatables));

        animators.Add(new ShowAnimator(1f));
        AddAnimatable(NodeValueDecorator.Create(nw.Nodes[4], nw.Nodes[4].NodeInfo.act));

        //DELAY
        animators.Add(new ShowAnimator(11f));
        animators.Add(new AnimateAnimator(2f));
        AddAnimatable(tc);

        ShowAnimator weightChangers1 = new ShowAnimator(1f);
        animators.Add(weightChangers1);
        AddAnimatable(WeightChangeDecorator.Create(nw.Edges[4], 0.16f));
        AddAnimatable(WeightChangeDecorator.Create(nw.Edges[5], 0.2f));
        animators.Add(new AnimateAnimator(2f, weightChangers1.animatables));

        ShowAnimator weightChangers2 = new ShowAnimator(1f);
        animators.Add(weightChangers2);
        AddAnimatable(WeightChangeDecorator.Create(nw.Edges[0], 0.1249f));
        AddAnimatable(WeightChangeDecorator.Create(nw.Edges[1], 0.319f));
        AddAnimatable(WeightChangeDecorator.Create(nw.Edges[2], 0.2156f));
        AddAnimatable(WeightChangeDecorator.Create(nw.Edges[3], 0.713f));
        animators.Add(new AnimateAnimator(2f, weightChangers2.animatables));
        animators.Add(new HideAnimator(2f, weightChangers1.animatables.Concat(weightChangers2.animatables).ToList()));

        animators.Add(new ReverseAnimateAnimator(2f));
        AddAnimatable(tc);

        ShowAnimator values = new ShowAnimator(1f);
        animators.Add(values);
        AddAnimatable(ValueDecorator.Create(nw.Edges[0]));
        AddAnimatable(ValueDecorator.Create(nw.Edges[1]));
        AddAnimatable(ValueDecorator.Create(nw.Edges[2]));
        AddAnimatable(ValueDecorator.Create(nw.Edges[3]));
        animators.Add(new AnimateAnimator(2f, values.animatables));

        animators.Add(new AnimateAnimator(2f));
        AddAnimatable(ValueChangeDecorator.Create(nw.Nodes[2], 0.76f));
        AddAnimatable(ValueChangeDecorator.Create(nw.Nodes[3], 1.6416f));

        ShowAnimator values2 = new ShowAnimator(1f);
        animators.Add(values2);
        AddAnimatable(ValueDecorator.Create(nw.Edges[4]));
        AddAnimatable(ValueDecorator.Create(nw.Edges[5]));
        animators.Add(new AnimateAnimator(2f, values2.animatables));

        animators.Add(new AnimateAnimator(2f));
        AddAnimatable(ValueChangeDecorator.Create(nw.Nodes[4], 0.4492f));
        animators.Add(new ShowAnimator(5f));
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
            new NodeInfo(0,0, 0,0.5f,0,  0, "", max:0.5f, act:1f),
            new NodeInfo(1,0, 0,-1.5f,0,  0, "", 0.1f, 2f),
            
            //Hidden
            new NodeInfo(2,1, 2,0,0,  0, "", 0.1f, 0.56f),
            new NodeInfo(3,1, 2,-1,0,  0, "", 0.1f, 1.41f),

            //Output
            new NodeInfo(4,2, 4,-0.5f,0,  0, "", 0, 0.2899f),
        };
        nw.MatchNetwork(new List<NodeInfo>(n));

        HiddenLayerDecorator.Create(nw.Nodes[2], Relu, ReluString);
        HiddenLayerDecorator.Create(nw.Nodes[3], Relu, ReluString);
        HiddenLayerDecorator.Create(nw.Nodes[4], Relu, OutputString);

        Vector3 offset = new Vector3(0, 0.15f, 0);
        float size = 3f;
        HintVisual.Create(nw.Nodes[0].Go.transform, "Input Neuron", offset, size);
        HintVisual.Create(nw.Nodes[1].Go.transform, "Input Neuron", offset, size);

        HintVisual.Create(nw.Nodes[2].Go.transform, "Hidden Neuron", offset, size);
        HintVisual.Create(nw.Nodes[3].Go.transform, "Hidden Neuron", offset, size);

        HintVisual.Create(nw.Nodes[4].Go.transform, "Output Neuron", offset, size);

        //nw.Nodes[4].node.gameObject.GetNamedChild("Neuron Data").GetComponent<NeuronDataDisplayer>().calculationText =
        //    "0.1*0.3\r\n+\r\n0.13*0.5\r\n+\r\n=0.03+0.065 = 0.095";

        RelationInfo[] r = new RelationInfo[]{
            // IN -> H
            new RelationInfo(0,0,2,"0.12"),
            new RelationInfo(1,1,2,"0.22"),

            new RelationInfo(2,0,3,"0.21"),
            new RelationInfo(3,1,3,"0.6"),

            // H -> O
            new RelationInfo(4,2,4,"0.14"),
            new RelationInfo(5,3,4,"0.15"),
        };
        nw.MatchRelations(new List<RelationInfo>(r));
    }
    float Relu(float x)
    {
        return x < 0 ? 0 : x;
    }
    string ReluString(float x)
    {
        return $"ReLU: max(0,{x}) = "+Relu(x);
    }
    string OutputString(float x)
    {
        float mse = Mathf.Pow((x-1f), 2)*0.5f;
        return $"ReLU: max(0,{x}) \n Expected: {1} \n MSE=0.5*({x} - 1.0)\xB2 = {mse}";
    }
}
