using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Platformer.FSM.Character
{
    public class WallSlide : CharacterStateBase
    {
        public override CharacterStateID id => CharacterStateID.WallSlide;
        public override bool canExecute => base.canExecute &&
                            (machine.currentStateID == CharacterStateID.Fall) &&
                            controller.isWallDetected;

        private float _damping;
        private float _speedY;

        public WallSlide(CharacterMachine machine, float damping = 0.6f) : base(machine)
        {
            _damping = damping;
        }

        public override void OnStateEnter()
        {
            base.OnStateEnter();
            controller.isDirectionChangeable = false;
            controller.isMovable = false;
            controller.hasJumped = false;
            controller.hasDoubleJumped = false;
            controller.Stop();
            rigidbody.bodyType = RigidbodyType2D.Kinematic;
            _speedY = 0.0f;
            animator.Play("WallSlide");
        }

        public override void OnStateExit()
        {
            base.OnStateExit();
            rigidbody.bodyType = RigidbodyType2D.Dynamic;
        }
        public override CharacterStateID OnStateUpdate()
        {
            CharacterStateID nextID = base.OnStateUpdate();

            if (nextID == CharacterStateID.None)
                return id;

            if (controller.isGrounded || controller.isWallDetected == false)
            {
                nextID = CharacterStateID.Idle;
            }

            return nextID;
        }

        public override void OnStateFixedUpdate()
        {
            base.OnStateFixedUpdate();

            // gravity가 가속도(시간 변화 분의 속도 변화). speed는 속도.
            //_speedY += Mathf.Abs(Physics2D.gravity.y) * (1.0f - _damping) * Time.fixedDeltaTime;
            //transform.position += Vector3.down * _speedY * Time.fixedDeltaTime;

            _speedY += Physics2D.gravity.y * (1.0f - _damping) * Time.fixedDeltaTime;
            transform.position += Vector3.up * _speedY * Time.fixedDeltaTime;
        }

    }
}
