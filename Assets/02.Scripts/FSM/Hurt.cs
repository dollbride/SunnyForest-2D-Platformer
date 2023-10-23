using System;
using UnityEngine;

namespace Platformer.FSM.Character
{
    public class Hurt : CharacterStateBase
    {
        public override CharacterStateID id => CharacterStateID.Hurt;
        public Hurt(CharacterMachine machine) : base(machine)
        {

        }

        public override void OnStateEnter()
        {
            base.OnStateEnter();
            controller.isDirectionChageable = true;
            controller.isMovable = true;
            controller.hasJumped = false;
            controller.hasDoubleJumped = false;

            //controller.DepleteHp(this, 5.0f);
            //Console.WriteLine($"체력: {controller.hpValue}");

            animator.Play("Hurt");
        }

        public override CharacterStateID OnStateUpdate()
        {
            CharacterStateID nextID = base.OnStateUpdate();

            if (nextID == CharacterStateID.None)
                return id;

            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                nextID = CharacterStateID.Idle;

            return nextID;
        }
    }
}
