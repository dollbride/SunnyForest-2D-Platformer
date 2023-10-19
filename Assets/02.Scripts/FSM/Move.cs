﻿using System;
using UnityEngine;

namespace Platformer.FSM.Character
{
    public class Move : CharacterStateBase
    {
        public override CharacterStateID id => CharacterStateID.Move;
        public Move(CharacterMachine machine) : base(machine)
        {

        }

        public override void OnStateEnter()
        {
            base.OnStateEnter();
            controller.isDirectionChageable = true;
            controller.isMovable = true;
            animator.Play("Move");
        }

        public override CharacterStateID OnStateUpdate()
        {
            CharacterStateID nextID = base.OnStateUpdate();

            if (nextID == CharacterStateID.None)
                return id;

            if (controller.horizontal == 0.0f)
                nextID = CharacterStateID.Idle;

            if (controller.isGrounded == false)
                nextID = CharacterStateID.Fall;

            return nextID;
        }
    }
}
