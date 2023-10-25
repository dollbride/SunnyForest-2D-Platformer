using Platformer.Controllers;
using Platformer.GameElements;
using System;
using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

namespace Platformer.FSM.Character
{
    /// <summary>
    /// 위에 감지된 사다리를 타는 상태 
    /// </summary>
    public class LadderDown : CharacterStateBase
    {
        public override CharacterStateID id => CharacterStateID.LadderDown;
        public override bool canExecute => base.canExecute &&
                       (machine.currentStateID == CharacterStateID.Idle ||
                        machine.currentStateID == CharacterStateID.Move ||
                        machine.currentStateID == CharacterStateID.Dash ||
                        machine.currentStateID == CharacterStateID.Crouch ||
                        machine.currentStateID == CharacterStateID.Fall ||
                        machine.currentStateID == CharacterStateID.Jump ||
                        machine.currentStateID == CharacterStateID.DoubleJump ||
                        machine.currentStateID == CharacterStateID.DownJump) &&
                        controller.isDownLadderDetected;
        // 만약 점프해서 타는 거 말고 땅에 있을 때만 내려가고 싶다면
        // canExecute에서 점프류는 다 빼고 isGrounded 조건 추가하면 된다.

        private Ladder _ladder;
        private float _vertical;
        private bool _doExit;

        public LadderDown(CharacterMachine machine) : base(machine)
        {
        }
        
        public override void OnStateEnter()
        {
            base.OnStateEnter();

            controller.isDirectionChangeable = false;
            controller.isMovable = false;
            // 수평 입력이 들어올 때만 점프 가능하게 할 거라 점프 설정은 밑으로 뺌 
            controller.hasDoubleJumped = false;
            controller.Stop();
            rigidbody.bodyType = RigidbodyType2D.Kinematic;
            animator.Play("Ladder");
            animator.speed = 0.0f;  // 가만히 있을 땐 재생 멈춤 

            _ladder = controller.downLadder;

            // 플레이어의 위치가 downEnter보다 낮으면 현재 위치에서 시작,
            // 높으면 downEnter(진입지점)으로 이동 
            Vector2 startPos = transform.position.y < _ladder.downEnter.y ? new Vector2(_ladder.bottom.x, transform.position.y) : _ladder.downEnter;
            transform.position = startPos;
            _doExit = false;
        }

        public override void OnStateExit()
        {
            base.OnStateExit();
            rigidbody.bodyType = RigidbodyType2D.Dynamic;
            animator.speed = 1.0f;
        }

        public override CharacterStateID OnStateUpdate()
        {
            CharacterStateID nextID = base.OnStateUpdate();

            if (nextID == CharacterStateID.None)
                return id;

            _vertical = controller.vertical;
            animator.speed = Mathf.Abs(_vertical);

            // 수평 입력 값이 있을 때만 점프 가능하도록(false일 때 가능하니까) 
            controller.hasJumped = controller.horizontal == 0.0f;

            if (_doExit)
                nextID = CharacterStateID.Idle;

            return nextID;
        }

        public override void OnStateFixedUpdate()
        {
            base.OnStateFixedUpdate();
            transform.position += Vector3.up * _vertical * Time.fixedDeltaTime;

            if (transform.position.y >= _ladder.upExit.y)
            {
                transform.position = _ladder.top;
                _doExit = true;
            }
            else if (transform.position.y <= _ladder.downExit.y)
            {
                _doExit = true;
            }
        }
    }
}
