using Platformer.Controllers;
using System;
using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

namespace Platformer.FSM.Character
{
    public class LadderUp : CharacterStateBase
    {

        private float _climbSpeed;
        //private float _jumpForce;

        public override CharacterStateID id => CharacterStateID.LadderUp;
        public override bool canExecute => base.canExecute &&
                       (machine.currentStateID == CharacterStateID.Idle ||
                        machine.currentStateID == CharacterStateID.Move ||
                        machine.currentStateID == CharacterStateID.Jump ||
                        machine.currentStateID == CharacterStateID.DoubleJump ||
                        machine.currentStateID == CharacterStateID.DownJump) &&
                        controller.isUpLadderDetected;

        public LadderUp(CharacterMachine machine, float climbSpeed) : base(machine)
        {
            _climbSpeed = climbSpeed;
            //_jumpForce = jumpForce;
        }
        

        public override void OnStateEnter()
        {
            base.OnStateEnter();

            controller.isDirectionChangeable = false;
            controller.isMovable = false;
            controller.Stop();
            rigidbody.bodyType = RigidbodyType2D.Kinematic;
            animator.Play("Ladder");
        }

        public override void OnStateExit()
        {
            base.OnStateExit();
            rigidbody.bodyType = RigidbodyType2D.Dynamic;
            rigidbody.AddForce(Vector2.up * 3.0f);
            //rigidbody.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
            // transform.position.y
            // controller.upLadder.upExit
        }

        public override CharacterStateID OnStateUpdate()
        {
            CharacterStateID nextID = base.OnStateUpdate();

            if (nextID == CharacterStateID.None)
                return id;

            if (controller.vertical < 0)
                nextID = CharacterStateID.LadderDown;

            if ((transform.position.y + 0.18f >= controller.upLadder.upExit.y) || (transform.position.y <= controller.upLadder.downExit.y))
                nextID = CharacterStateID.Idle;

            return nextID;
        }

        public override void OnStateFixedUpdate()
        {
            base.OnStateFixedUpdate();
            rigidbody.position += new Vector2(0.0f, controller.vertical * _climbSpeed);
        }
    }
}
