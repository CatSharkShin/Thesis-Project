using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAnimatable
{
    public void Animate(float t);
    string Name { get; }
    public void Show(float t);
}
