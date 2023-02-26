using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatableGroup
{
    public float length;
    public Coroutine coroutine;
    public List<IAnimatable> animatables = new List<IAnimatable>();
    public AnimatableGroup(float length)
    {
        this.length = length;
    }
    public void AddAnimatable(IAnimatable anim) {
        animatables.Add(anim);
    }
    public void RemoveAnimatable(IAnimatable animatable)
    {
        animatables.Remove(animatable);
    }
    public void Animate(float t)
    {
        foreach (IAnimatable animatable in animatables)
        {
            animatable.Animate(t);
        }
    }
    public void Show(float t)
    {
        foreach (IAnimatable animatable in animatables)
        {
            animatable.Show(t);
        }
    }
}
