using Platformer.Controllers;
using Platformer.FSM.Character;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using CharacterController = Platformer.Controllers.CharacterController;

namespace Platformer.FSM
{
    public class StateMachine<T>
        where T : Enum
    {
        public T currentStateID;
        public Dictionary<T, IState<T>> states;
        private bool _isDirty;

        public void Init(IDictionary<T, IState<T>> copy)
        {
            states = new Dictionary<T, IState<T>>(copy);
            currentStateID = states.First().Key;
            states[currentStateID].OnStateEnter();
        }

        public void UpdateState()
        {
            ChangeState(states[currentStateID].OnStateUpdate());
        }

        public void FixedUpdateState()
        {
            states[currentStateID].OnStateFixedUpdate();
        }

        public void LateUpdateState()
        {
            _isDirty = false;
        }

        public bool ChangeState(T newStateID)
        {
            if (_isDirty)
                return false;
            // 바꾸려는 상태가 현재 상태와 동일하면 바꿀 필요가 없으므로 종료함
            if (Comparer<T>.Default.Compare(newStateID, currentStateID) == 0)
                return false;

            // 바꾸려는 상태가 실행 가능하지 않다면 바꾸지 않음
            if (states[newStateID].canExecute == false)
                return false;

            _isDirty = true;
            states[currentStateID].OnStateExit();   // 기존 상태에서 탈출0
            currentStateID = newStateID;            // 상태 갱신
            states[currentStateID].OnStateEnter();  // 새로운 상태로 진입
            return true;
        }
    }
}