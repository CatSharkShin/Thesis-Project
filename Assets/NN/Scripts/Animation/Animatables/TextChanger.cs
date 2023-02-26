using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextChanger : IAnimatable
{
    public string Name => "TextChanger";
    public TextMeshPro tmp;
    public string from;
    public string to;
    public TextChanger(TextMeshPro tmp, string from, string to)
    {
        this.tmp = tmp;
        this.from = from;
        this.to = to;
        tmp.text = from;
    }

    public void Animate(float t)
    {
        tmp.text = from.Lerp(to, t);
    }
    public void Show(float t)
    {
        throw new System.NotImplementedException();
    }
    public TextChanger Create(TextMeshPro tmp,string from, string to)
    {
        TextChanger tc = new TextChanger(tmp,from,to);
        return tc;
    }
}
