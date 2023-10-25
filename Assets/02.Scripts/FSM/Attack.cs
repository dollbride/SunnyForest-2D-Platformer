using Platformer.Animations;
using Platformer.Stats;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer.FSM.Character
{
    public class Attack : CharacterStateBase
    {
        public override CharacterStateID id => CharacterStateID.Attack;
        public override bool canExecute
        {
            get
            {
                if (base.canExecute == false)
                    return false;

                // 콤보스택이 쌓여있는 상황에서 마지막 공격이 끝났던 시각으로부터
                // 경과된 시간이 콤보 초기화 시간을 넘어갔으면 콤보 안됨 
                float elapsedTime = Time.time - _exitTimeMark;  // 마지막 공격 이후 경과된 시간
                if (_comboStack > 0 &&
                    elapsedTime > _comboResetTime)
                {
                    _comboStack = 0;
                    return false;
                }

                if (_comboStack > _comboStackMax)
                {
                    return false;
                }

                // 첫타는 무조건 오케이, 후속타는 이전 공격 히트 판정 이후 오케이 
                if ((_comboStack == 0 || (_comboStack > 0 && _hasHit)) &&
                    (machine.currentStateID == CharacterStateID.Idle ||
                     machine.currentStateID == CharacterStateID.Move ||
                     machine.currentStateID == CharacterStateID.Crouch ||
                     machine.currentStateID == CharacterStateID.Jump ||
                     machine.currentStateID == CharacterStateID.DownJump ||
                     machine.currentStateID == CharacterStateID.DoubleJump ||
                     machine.currentStateID == CharacterStateID.Fall))
                {
                    return true;
                }
                return false;
            }
        }

        private int _comboStackMax;
        private int _comboStack;
        private float _comboResetTime;
        private float _exitTimeMark;    // 마지막 공격 끝난 시간
        private bool _hasHit;           // 현재 공격이 히트 판정 되었는지

        public class AttackSetting
        {
            public int targetMax;           // 최대 타게팅 수
            public LayerMask targetMask;    // 타겟 검출 마스크
            public float damageGain;        // 공격 계수
            public Vector2 castCenter;      // 타겟 감지 형상(사각형) 범위의 중심
            public Vector2 castSize;        // 타겟 감지 형상(사각형)의 크기
            public float castDistance;      // 타겟 감지 형상(사각형)의 빔 거리 
        }

        private AttackSetting[] _attackSettings;
        private List<IHp> _targets = new List<IHp>();
        private CharacterAnimationEvents _animationEvents;

        public Attack(CharacterMachine machine, AttackSetting[] attackSettings, float comboResetTime) : base(machine)
        {
            _attackSettings = attackSettings;
            _comboStackMax = attackSettings.Length;
            _comboResetTime = comboResetTime;
            _animationEvents = animator.GetComponent<CharacterAnimationEvents>();
            // event 한정자를 적지 않았기 때문에 += 대신 그냥 = 쓰면 됨 
            _animationEvents.onHit = () =>
            {
                foreach (var target in _targets)
                {
                    if (target == null)
                        continue;
                    float damage = Random.Range(controller.damageMin, controller.damageMax) * _attackSettings[_comboStack - 1].damageGain;
                    target.DepleteHp(controller, damage);
                }
                _hasHit = true;
            };
        }

        public override void OnStateEnter()
        {
            base.OnStateEnter();
            controller.isDirectionChangeable = false;
            controller.isMovable = controller.isGrounded;
            _hasHit = false;

            AttackSetting setting = _attackSettings[_comboStack - 1];
            RaycastHit2D[] hits =
                Physics2D.BoxCastAll(origin: rigidbody.position + new Vector2(setting.castCenter.x * controller.direction, setting.castCenter.y),
                                     size: setting.castSize,
                                     angle: 0.0f,
                                     direction: Vector2.right * controller.direction,
                                     distance: setting.castDistance,
                                     layerMask: setting.targetMask);
            Vector2 origin = rigidbody.position + new Vector2(setting.castCenter.x * controller.direction, setting.castCenter.y);
            Vector2 size = setting.castSize;
            float distance = setting.castDistance;

            // 왼쪽 위 -> 오른쪽 위
            Debug.DrawLine(origin + new Vector2(-size.x / 2.0f * controller.direction, +size.y / 2.0f),
                           origin + new Vector2(+size.x / 2.0f * controller.direction, +size.y / 2.0f) + Vector2.right * controller.direction * distance);
            // 왼쪽 아래 -> 오른쪽 아래
            Debug.DrawLine(origin + new Vector2(-size.x / 2.0f * controller.direction, -size.y / 2.0f),
                           origin + new Vector2(+size.x / 2.0f * controller.direction, -size.y / 2.0f) + Vector2.right * controller.direction * distance);
            // 왼쪽 위 -> 왼쪽 아래 
            Debug.DrawLine(origin + new Vector2(-size.x / 2.0f * controller.direction, +size.y / 2.0f),
                           origin + new Vector2(-size.x / 2.0f * controller.direction, -size.y / 2.0f));
            // 오른 위 -> 오른쪽 아래
            Debug.DrawLine(origin + new Vector2(-size.x / 2.0f * controller.direction, -size.y / 2.0f),
                           origin + new Vector2(+size.x / 2.0f * controller.direction, +size.y / 2.0f) + Vector2.right * controller.direction * distance);



            // 전체 감지된 적 중에서 최대 타겟 수까지만 대상으로 등록
            _targets.Clear();
            for (int i = 0; i < hits.Length; i++)
            {
                if (_targets.Count >= setting.targetMax)
                    break;

                if (hits[i].collider.TryGetComponent(out IHp target))
                    _targets.Add(target);
            }

            animator.SetFloat("comboStack", _comboStack++);
            animator.Play("Attack");
        }
        
        public override void OnStateExit()
        {
            base.OnStateExit();
            _exitTimeMark = Time.time;
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
