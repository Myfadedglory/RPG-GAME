using UnityEngine;

namespace Script.Utilities
{
    public class EntityState<T> : IState where T : Entity
    {
        protected float StateTimer { get; set; }
        protected bool IsAnimationFinished { get; set; }
        protected string AnimBoolName {  get; private set; }
        protected Fsm Fsm { get; private set; }
        protected T Entity { get; private set; }
        protected Animator Anim { get; private set; }
        protected Rigidbody2D Rb {  get; private set; }

        protected EntityState(T entity , Fsm fsm , string animBoolName)
        {
            AnimBoolName = animBoolName;
            Entity = entity;
            Fsm = fsm;
            Anim = entity.Anim;
            Rb = entity.Rb;
        }

        public virtual void Enter(IState lastState)
        {
            Anim.SetBool(AnimBoolName, true);
            IsAnimationFinished = false;
        }

        public virtual void Update()
        {
            StateTimer -= Time.deltaTime;
        }

        public virtual void Exit(IState newState)
        {
            Anim.SetBool(AnimBoolName, false);
        }

        public virtual void AnimationFinishTrigger()
        {
            IsAnimationFinished = true;
        }
    }
}
