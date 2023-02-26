using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurlsToAnimator : MonoBehaviour
{
    /*
    Hey Martin, Im Martin
    If you see this, please just use the HandGetter and create your own script
    instead of plugging the values into the animator thru this script
    */
    public HandGetter handGetter;
    public Animator animator;
    public float speed;
    private void Update()
    {
        LerpValues();
    }
    void LerpValues()
    {
        animator.SetFloat("Thumb", Mathf.Lerp(animator.GetFloat("Thumb"),handGetter.Curls[0],Time.deltaTime*speed));
        animator.SetFloat("Index", Mathf.Lerp(animator.GetFloat("Index"), handGetter.Curls[1], Time.deltaTime * speed));
        animator.SetFloat("Middle", Mathf.Lerp(animator.GetFloat("Middle"), handGetter.Curls[2], Time.deltaTime * speed));
        animator.SetFloat("Ring", Mathf.Lerp(animator.GetFloat("Ring"), handGetter.Curls[3], Time.deltaTime * speed));
        animator.SetFloat("Pinky", Mathf.Lerp(animator.GetFloat("Pinky"), handGetter.Curls[4], Time.deltaTime * speed));
    }
    void InstantSetValues()
    {
        animator.SetFloat("Thumb", handGetter.Curls[0]);
        animator.SetFloat("Index", handGetter.Curls[1]);
        animator.SetFloat("Middle", handGetter.Curls[2]);
        animator.SetFloat("Ring", handGetter.Curls[3]);
        animator.SetFloat("Pinky", handGetter.Curls[4]);
    }
}
