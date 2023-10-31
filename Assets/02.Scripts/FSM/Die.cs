using System;
using UnityEngine;

namespace Platformer.FSM.Character
{
    public class Die : CharacterStateBase
    {
        public override CharacterStateID id => CharacterStateID.Die;

        public Die(CharacterMachine machine) : base(machine)
        {
        }

        public override void OnStateEnter()
        {
            base.OnStateEnter();
            controller.isDirectionChangeable = false;
            controller.isMovable = false;
            // 랜딩할 때 미끄러지지 않고 멈추고 싶을 때 추가하는 함수:
            controller.Stop();
            trigger.enabled = false;
            //rigidbody.simulated = false;
            animator.Play("Die");
        }

        public override CharacterStateID OnStateUpdate()
        {
            CharacterStateID nextID = base.OnStateUpdate();

            if (nextID == CharacterStateID.None)
                return id;

            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f )
                transform.gameObject.SetActive(false);

            return nextID;
        }
    }
}
