using System;

namespace Platformer.FSM
{
    public abstract class StateBase<T> : IState<T>
        where T : Enum
    {
        public abstract T id { get; }

        public virtual bool canExecute => true;

        private bool _hasFixedUpdated;
        // 기본적으로 true지만 이걸 상속받은 자식이 부가적인 조건을 달아서
        // 오버라이드 할 수 있도록 버츄얼 키워드 지정

        public StateBase(StateMachine<T> machine)
        {

        }

        public virtual void OnStateEnter()
        {
            UnityEngine.Debug.Log($"State Entered to {id}");
            _hasFixedUpdated = false;
        }

        public virtual void OnStateExit()
        {
        }

        public virtual void OnStateFixedUpdate()
        {
            if (_hasFixedUpdated == false)
                _hasFixedUpdated = true;
        }

        public virtual T OnStateUpdate()
        {
            return _hasFixedUpdated ? id : default(T);
            // 디폴트는 enum 중 맨 앞에 있는 상태(None)
        }
    }
}
