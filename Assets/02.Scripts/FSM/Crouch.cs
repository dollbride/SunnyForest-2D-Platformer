using System;
using System.Net.Http.Headers;
using UnityEngine;

namespace Platformer.FSM.Character
{
    public class Crouch : CharacterStateBase
    {
        public override CharacterStateID id => CharacterStateID.Crouch;
        public override bool canExecute => base.canExecute &&
                                        (machine.currentStateID == CharacterStateID.Idle ||
                                        machine.currentStateID == CharacterStateID.Move) &&
                                        controller.isGrounded;

        private int _step;

        public Crouch(CharacterMachine machine) : base(machine)
        {
        }

        public override void OnStateEnter()
        {
            base.OnStateEnter();
            controller.isDirectionChageable = false;
            controller.isMovable = false;
            // 랜딩할 때 미끄러지지 않고 멈추고 싶을 때 추가하는 함수:
            controller.Stop();
            animator.Play("CrouchStart");
            _step = 0;
        }

        public override CharacterStateID OnStateUpdate()
        {
            CharacterStateID nextID = base.OnStateUpdate();

            if (nextID == CharacterStateID.None)
                return id;

            switch (_step)
            {
                case 0:
                    {
                        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                        {
                            animator.Play("CouchIdle");
                            _step++;
                        }
                    }
                    break;
                case 1:
                    {
                        //nothing to do
                    }
                    break;
                default:
                    break;
            }

            return nextID;
        }
    }
}
