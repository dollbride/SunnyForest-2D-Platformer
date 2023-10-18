using System;
using UnityEngine;
using CharacterController = Platformer.Controllers.CharacterController;

namespace Platformer.FSM
{
    public enum CharacterStateID
    {
        None,
        Idle,
        Move,
        Jump,
        DownJump,
        DoubleJump,
        Fall,
        Land,
        Crounch,
        Hurt,
        Die,
        Attack,
        WallSlide,
        Edge,
        EdgeClimb,
        Ladder
    }

    public class CharacterStateBase : IState<CharacterStateID>
    {
        public virtual CharacterStateID id { get; }

        public virtual bool canExecute => true;
        // 기본적으로 true지만 이걸 상속받은 자식이 오버라이드 할 수 있도록 버츄얼 키워드 지정

        protected StateMachine<CharacterStateID> machine;
        protected CharacterController controller;
        protected Transform transform;
        protected Rigidbody2D rigidbody;
        protected Animator animator;

        public CharacterStateBase(StateMachine<CharacterStateID> machine)
        {
            this.machine = machine;
            this.controller = machine.owner;
            this.transform = machine.owner.transform;
            this.rigidbody = machine.owner.GetComponent<Rigidbody2D>();
            this.animator = machine.owner.GetComponentInChildren<Animator>();
        }

        public virtual void OnStateEnter()
        {
        }

        public virtual void OnStateExit()
        {
        }

        public virtual CharacterStateID OnStateUpdate()
        {
            return id;
        }

        public virtual void OnStateFixedUpdate()
        {
        }
    }
}