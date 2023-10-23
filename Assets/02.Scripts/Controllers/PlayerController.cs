using Platformer.FSM;
using System.Linq;
using UnityEngine;

namespace Platformer.Controllers
{
    public class PlayerController : CharacterController
    {
        public override float horizontal => Input.GetAxis("Horizontal");

        public override float vertical => Input.GetAxis("Vertical");

        private void Start()
        {
            // 캐릭터 컨트롤러에 있는 캐싱을 하기 위한 변수 machine (캐릭터 머신).
            // 생성(캐싱)은 플레이어머신으로 했지만 machine은 캐릭터 머신을 참조하므로
            // (근데 이렇게 생성해서 쓰는 경우엔 굳이 캐싱이라는 말 안 씀)
            machine = new PlayerMachine(this);
            var machineData = StateMachineDataSheet.GetPlayerData(machine);
            machine.Init(machineData);
        }

        protected override void Update()
        {
            base.Update();

            // 겟키는 누르고 있으면 계속 트루
            if (Input.GetKey(KeyCode.LeftAlt))
            {
                if (machine.ChangeState(CharacterStateID.DownJump) ||
                   (machine.currentStateID ==  CharacterStateID.WallSlide == false 
                    && machine.ChangeState(CharacterStateID.Jump)))
                {
                }
            }

            // 겟키다운은 누르고 있어도 한 프레임만 한번, 그 뒤로는 false
            if (Input.GetKeyDown(KeyCode.LeftAlt))
            {
                // A && B : A가 false면 B를 안 읽고 그냥 false 반환
                // A || B : A가 true면 B를 안 읽고 그냥 true 반환
                // if 조건문 안에 함수가 있기 때문에 조건이 매칭되는지를 보기 위해
                // 일단 한 번은 실행됨 
                if (machine.ChangeState(CharacterStateID.Jump) == false &&
                    machine.ChangeState(CharacterStateID.DoubleJump) == false)
                { }
            }

            if (Input.GetKey(KeyCode.RightArrow) ||
                Input.GetKey(KeyCode.LeftArrow))
            {
                machine.ChangeState(CharacterStateID.WallSlide);
            }
            else if (machine.currentStateID == CharacterStateID.WallSlide)
            {
                machine.ChangeState(CharacterStateID.Idle);
            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                machine.ChangeState(CharacterStateID.Crouch);  
            }
            else if (Input.GetKeyUp(KeyCode.DownArrow)) // 키를 떼면(KeyUp)
            {
                if (machine.currentStateID == CharacterStateID.Crouch)
                    machine.ChangeState(CharacterStateID.Idle);
            }

            //if(Input.GetKeyDown(KeyCode.LeftShift))
            //{
            //    machine.ChangeState(CharacterStateID.Dash);
            //}
            //else if (Input.GetKeyUp(KeyCode.LeftShift)) // 키를 떼면(KeyUp)
            //{
            //    if (machine.currentStateID == CharacterStateID.Dash)
            //        machine.ChangeState(CharacterStateID.Idle);
            //}

        }


    }
}