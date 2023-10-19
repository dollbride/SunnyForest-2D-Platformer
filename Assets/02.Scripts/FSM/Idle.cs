using System;
using UnityEngine;

namespace Platformer.FSM.Character
{
    public class Idle : CharacterStateBase
    {
        public override CharacterStateID id => CharacterStateID.Idle;
        public Idle(CharacterMachine machine) : base(machine)
        {

        }

        public override void OnStateEnter()
        {
            base.OnStateEnter();
            controller.isDirectionChageable = true;
            controller.isMovable = true;
            animator.Play("Idle");
        }

        public override CharacterStateID OnStateUpdate()
        {
            CharacterStateID nextID = base.OnStateUpdate();

            if (nextID == CharacterStateID.None)
                return id;

            if (Mathf.Abs(controller.horizontal) > 0.0f)
                nextID = CharacterStateID.Move;

            if (controller.isGrounded == false)
                nextID = CharacterStateID.Fall;

            return nextID;
        }
    }
}
