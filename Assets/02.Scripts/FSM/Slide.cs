using System;
using UnityEngine;

namespace Platformer.FSM.Character
{
    public class Slide : CharacterStateBase
    {
        public override CharacterStateID id => CharacterStateID.Slide;
        private float _distance;
        private Vector3 _targetPosition;
        private Vector3 _startPosition;

        private Vector2 _originalColliderOffset;
        private Vector2 _originalColliderSize;
        private Vector2 _crouchedColliderOffset;
        private Vector2 _crouchedColliderSize;

        public override bool canExecute => base.canExecute &&
                                        (machine.currentStateID == CharacterStateID.Idle ||
                                         machine.currentStateID == CharacterStateID.Move ||
                                         machine.currentStateID == CharacterStateID.Crouch) &&
                                         controller.isGrounded;

        public Slide(CharacterMachine machine, float distance, Vector2 crouchedColliderOffset, Vector2 crouchedColliderSize) : base(machine)
        {
            _distance = distance;
            _originalColliderOffset = trigger.offset;
            _originalColliderSize = trigger.size;
            _crouchedColliderOffset = crouchedColliderOffset;
            _crouchedColliderSize = crouchedColliderSize;
        }

        public override void OnStateEnter()
        {
            base.OnStateEnter();
            controller.isDirectionChangeable = false;
            controller.isMovable = false;
            // 랜딩할 때 미끄러지지 않고 멈추고 싶을 때 추가하는 함수:
            controller.Stop();
            collision.offset = trigger.offset = _crouchedColliderOffset;
            collision.size = trigger.size = _crouchedColliderSize;

            rigidbody.bodyType = RigidbodyType2D.Kinematic;
            _startPosition = transform.position;
            _targetPosition = transform.position + Vector3.right * controller.direction * _distance;

            animator.Play("Slide");
        }

        public override void OnStateExit()
        {
            base.OnStateExit();
            rigidbody.bodyType = RigidbodyType2D.Dynamic;
            collision.offset = trigger.offset = _originalColliderOffset;
            collision.size = trigger.size = _originalColliderSize;
        }

        public override CharacterStateID OnStateUpdate()
        {
            CharacterStateID nextID = base.OnStateUpdate();

            if (nextID == CharacterStateID.None)
                return id;

            // t는 애니메이션이 재생됨에 따라 0에서 1로 변함
            float t = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
            // 대시 속도 보정
            t = Mathf.Log10(10 + t * 90) - 1;
            
            Vector3 expected = Vector3.Lerp(_startPosition, _targetPosition, t);

            bool isValid = true;
            // todo -> expected 위치가 유효한지 확인 (맵의 경계를 벗어나거나 벽이 있을 때, etc)
            if (Physics2D.OverlapCapsule((Vector2)expected + trigger.offset, 
                                 trigger.size,
                                 trigger.direction, 
                                 0.0f,
                                 1 << LayerMask.NameToLayer("Wall")))
            {
                _startPosition = transform.position;
                _targetPosition = transform.position;
                isValid = false;
            }

            if(isValid)
                transform.position = expected;

            if (t >= 1.0f )
                nextID = CharacterStateID.Idle;

            return nextID;
        }
    }
}
