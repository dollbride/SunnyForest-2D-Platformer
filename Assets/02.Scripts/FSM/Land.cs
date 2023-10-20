using System;
using UnityEngine;

namespace Platformer.FSM.Character
{
    public class Land : CharacterStateBase
    {
        public override CharacterStateID id => CharacterStateID.Land;

        public Land (CharacterMachine machine) : base(machine)
        {
        }

        public override void OnStateEnter()
        {
            base.OnStateEnter();
            controller.isDirectionChageable = false;
            controller.isMovable = false;
            // 랜딩할 때 미끄러지지 않고 멈추고 싶을 때 추가하는 함수:
            controller.Stop();
            animator.Play("Land");
        }

        public override CharacterStateID OnStateUpdate()
        {
            CharacterStateID nextID = base.OnStateUpdate();

            if (nextID == CharacterStateID.None)
                return id;

            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f )
                nextID = CharacterStateID.Idle;

            return nextID;
        }
    }
}
