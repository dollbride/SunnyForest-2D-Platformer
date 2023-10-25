using Platformer.Controllers;
using System;
using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

namespace Platformer.FSM.Character
{
    public class LadderUp_my : CharacterStateBase
    {

        private float _climbSpeed;

        public override CharacterStateID id => CharacterStateID.LadderUp;
        public override bool canExecute => base.canExecute &&
                       (machine.currentStateID == CharacterStateID.Idle ||
                        machine.currentStateID == CharacterStateID.Move ||
                        machine.currentStateID == CharacterStateID.Jump ||
                        machine.currentStateID == CharacterStateID.DoubleJump ||
                        machine.currentStateID == CharacterStateID.DownJump) &&
                        controller.isUpLadderDetected;

        public LadderUp_my(CharacterMachine machine, float climbSpeed) : base(machine)
        {
            _climbSpeed = climbSpeed;
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
        }

        public override CharacterStateID OnStateUpdate()
        {
            CharacterStateID nextID = base.OnStateUpdate();

            if (nextID == CharacterStateID.None)
                return id;

            if (controller.vertical < 0)
                nextID = CharacterStateID.LadderDown;

            if (transform.position.y + 0.25f >= controller.upLadder.upExit.y)
            {
                rigidbody.position = controller.upLadder.top;
                //rigidbody.AddForce(Vector2.up * 3.0f);
                nextID = CharacterStateID.Idle;
            }
                
            if (transform.position.y <= controller.upLadder.downExit.y)
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
