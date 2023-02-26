using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace NetworkAnimations
{
    public abstract class AnimatorController : MonoBehaviour
    {
        public List<Animator> animators = new List<Animator>();
        public bool animate;
        public bool loop;
        private void Awake()
        {
            Initialize();    
        }
        private void Update()
        {

        }
        IEnumerator CorAnimate() {
            foreach (Animator animator in animators)
            {
                float t = 0;
                while(t < animator.length)
                {
                    animator.Animate(t/animator.length);
                    yield return null;
                    t += Time.deltaTime;
                }
            }
            if (loop)
            {
                List<Animator> reversed = new List<Animator>(animators);
                reversed.Reverse();
                foreach (Animator animator in reversed)
                {
                    animator.Animate(0);
                }
                StartCoroutine(CorAnimate());
            }
        }
        protected void Animate()
        {
            StartCoroutine(CorAnimate());
        }
        public virtual void Initialize() { }

    }
    public abstract class Animator
    {
        public List<IAnimatable> animatables = new List<IAnimatable>();
        public float length;
        public Animator(float length)
        {
            this.length = length;
        }
        public Animator(float length,List<IAnimatable> animatables)
        {
            this.length = length;
            this.animatables = animatables;
        }
        public abstract void Animate(float t);
        public void AddAnimatable(IAnimatable animatable)
        {
            animatables.Add(animatable);
        }
        public void RemoveAnimatable(IAnimatable animatable)
        {
            animatables.Remove(animatable);
        }
    }
    public class ShowAnimator : Animator
    {
        public ShowAnimator(float length) : base(length)
        {
        }

        public ShowAnimator(float length, List<IAnimatable> animatables) : base(length, animatables)
        {
        }

        public override void Animate(float t)
        {
            foreach (IAnimatable anim in animatables)
            {
                anim.Show(t);
            }
        }
    }
    public class HideAnimator : Animator
    {
        public HideAnimator(float length) : base(length)
        {
        }

        public HideAnimator(float length, List<IAnimatable> animatables) : base(length, animatables)
        {
        }

        public override void Animate(float t)
        {
            foreach (IAnimatable anim in animatables)
            {
                anim.Show(1 - t);
            }
        }
    }
    public class AnimateAnimator : Animator
    {
        public AnimateAnimator(float length) : base(length)
        {
        }

        public AnimateAnimator(float length, List<IAnimatable> animatables) : base(length, animatables)
        {
        }

        public override void Animate(float t)
        {
            foreach (IAnimatable anim in animatables)
            {
                anim.Animate(t);
            }
        }
    }
    public class ReverseAnimateAnimator : Animator
    {
        public ReverseAnimateAnimator(float length) : base(length)
        {
        }

        public ReverseAnimateAnimator(float length, List<IAnimatable> animatables) : base(length, animatables)
        {
        }

        public override void Animate(float t)
        {
            foreach (IAnimatable anim in animatables)
            {
                anim.Animate(1f - t);
            }
        }
    }
}