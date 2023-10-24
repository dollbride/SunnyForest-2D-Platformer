using Platformer.Controllers;
using System;
using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

namespace Platformer.FSM.Character
{
    public class LadderDown : CharacterStateBase
    {
        private float _climbSpeed;

        public override CharacterStateID id => CharacterStateID.LadderDown;
        public override bool canExecute => base.canExecute &&
                     (machine.currentStateID == CharacterStateID.Idle ||
                      machine.currentStateID == CharacterStateID.Move) &&
                      controller.isDownLadderDetected;

        public LadderDown(CharacterMachine machine, float climbSpeed) : base(machine)
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

            if (controller.vertical > 0)
                nextID = CharacterStateID.LadderUp;

            if (controller.isDownLadderDetected == false)
                nextID = CharacterStateID.Idle;

            return nextID;
        }

        public override void OnStateFixedUpdate()
        {
            base.OnStateFixedUpdate();
            if (controller.isDownLadderDetected)
                rigidbody.position += new Vector2(0.0f, controller.vertical * _climbSpeed);
        }
    }
}
