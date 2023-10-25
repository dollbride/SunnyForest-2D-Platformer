using System;
using UnityEngine;

namespace Platformer.FSM.Character
{
    public class Jump : CharacterStateBase
    {
        public override CharacterStateID id => CharacterStateID.Jump;
        public override bool canExecute => base.canExecute &&
                    controller.hasJumped == false &&
                    machine.currentStateID == CharacterStateID.WallSlide ||
                    machine.currentStateID == CharacterStateID.LadderUp ||
                    machine.currentStateID == CharacterStateID.LadderDown ||
                    ((machine.currentStateID == CharacterStateID.Idle || 
                    machine.currentStateID == CharacterStateID.Move) &&
                    controller.isGrounded);

        private float _jumpForce;
        public Jump (CharacterMachine machine, float jumpForce) : base(machine)
        {
            _jumpForce = jumpForce; 
        }

        public override void OnStateEnter()
        {
            base.OnStateEnter();
            controller.isDirectionChangeable = true;
            controller.isMovable = false;
            controller.hasJumped = false;
            controller.hasDoubleJumped = false;
            animator.Play("Jump");
            
            float velocityX = (machine.previousStateID == CharacterStateID.WallSlide ||
                               machine.previousStateID == CharacterStateID.LadderUp ||
                               machine.previousStateID == CharacterStateID.LadderDown) ?
                               (controller.horizontal * controller.moveSpeed) : (rigidbody.velocity.x);
            rigidbody.velocity = new Vector2(velocityX, 0.0f);
            // 이전 상태가 월슬라이드였으면 입력에 따른 새로운 속도를 쓰고(기존과 반대 방향으로 튀어오르기 가능)
            // 아니었다면 기존 속도(rigidbody.velocity.x)를 쓴다.(기존에 가던 방향 그대로 고정)

            rigidbody.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
        }

        public override CharacterStateID OnStateUpdate()
        {
            CharacterStateID nextID = base.OnStateUpdate();

            if (nextID == CharacterStateID.None)
                return id;

            if (rigidbody.velocity.y <= 0.0f)
                nextID = CharacterStateID.Fall;

            return nextID;
        }
    }
}
