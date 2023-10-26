﻿using Platformer.FSM;
using Platformer.Stats;
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Platformer.Controllers
{
    public enum AI
    {
        None,
        Think,
        ExectueRandomBehaviour,
        WaitUntilBehaviour,
        Follow,
    }

    public class EnemyController : CharacterController
    {
        public override float horizontal => _horizontal;

        public override float vertical => _vertical;

        private float _horizontal;
        private float _vertical;

        [SerializeField] private AI _ai;
        private Transform _target;
        [SerializeField] private float _targetDetectRange;
        [SerializeField] private bool _autoFollow;
        [SerializeField] private bool _attackEnabled;
        [SerializeField] private float _attackRange;
        [SerializeField] private LayerMask _targetMask;
        [SerializeField] private List<CharacterStateID> _behaviours;
        [SerializeField] private float _behaviourTimeMin;
        [SerializeField] private float _behaviourTimeMax;
        private float _behaviourTimer;

        private CapsuleCollider2D _trigger;
        private Rigidbody2D _rigidbody;

        protected override void Awake()
        {
            base.Awake();
            _trigger = GetComponent<CapsuleCollider2D>();
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        protected override void Update()
        {
            UpdateAI();
            if (isGroundedForward == false)
            {
                Console.WriteLine($"달팽이 떨어짐");
                _rigidbody.velocity = Vector3.zero;
                direction *= -1;
            }

            base.Update();
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            //이동하려는 위치가 유효하지 않으면 제자리 
            if (isGrounded == false)
            {
                Console.WriteLine($"달팽이 떨어짐");
                _rigidbody.velocity = Vector3.zero;
                direction *= -1;
            }
        }

        public bool isGroundedForward
        {
            get
            {
                ground = Physics2D.OverlapBox(rigidbody.position + new Vector2(-0.12f, 0.0f) * direction,
                    new Vector2(0.04f, 0.02f), 0.0f, _groundMask);
                return ground;  // Unity 오브젝트는 결과가 null이어도 bool 타입(false)으로 반환 가능
            }
        }

        private void UpdateAI()
        {
            // 자동 따라가기 옵션이 켜져있는데
            if (_autoFollow)
            {
                // 타겟이 없으면
                if (_target == null)
                {
                    // 타겟 감지
                    Collider2D col
                        = Physics2D.OverlapCircle(rigidbody.position, _targetDetectRange, _targetMask);

                    if (col)
                        _target = col.transform;
                }
            }

            // 타겟이 감지되었으면 따라가야함
            if (_target)
            {
                _ai = AI.Follow;
            }

            switch (_ai)
            {
                case AI.Think:
                    break;
                case AI.ExectueRandomBehaviour:
                    break;
                case AI.WaitUntilBehaviour:
                    break;
                case AI.Follow:
                    {
                        // 타겟 없으면 다시생각해
                        if (_target == null)
                        {
                            _ai = AI.Think;
                            return;
                        }

                        // 타겟이 오른쪽에 있으면 오른쪽으로
                        if (transform.position.x < _target.position.x - _trigger.size.x)
                        {
                            _horizontal = 1.0f;
                        }
                        // 타겟이 왼쪽에 있으면 왼쪽으로
                        else if (transform.position.x > _target.position.x + _trigger.size.x)
                        {
                            _horizontal = -1.0f;
                        }

                        // 공격 가능하면서 공격 범위 내에 타겟이 있다면 공격, 아니면 타겟으로 이동
                        if (_attackEnabled &&
                            Vector2.Distance(transform.position, _target.position) <= _attackRange)
                        {
                            machine.ChangeState(CharacterStateID.Attack);
                        }
                        else
                        {
                            machine.ChangeState(CharacterStateID.Move);
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        public override void DepleteHp(object subject, float amount)
        {
            base.DepleteHp(subject, amount);

            //int a = 1;
            //Type type = a.GetType(); // GetType() 함수는 값의 멤버함수접근으로 호출해서 해당 값의 타입을 정보 객체를 반환
            //type = typeof(EnemyController); // typeof 키워드는 어떤 타입의 정보를 가지는 객체를 반환

            if (subject.GetType().Equals(typeof(Transform)))
                Knockback(Vector2.right * (((Transform)subject).position.x - transform.position.x < 0 ? 1.0f : -1.0f) * 1.0f);
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if ((1 << collision.gameObject.layer & _targetMask) > 0)
            {
                if (collision.TryGetComponent(out IHp target))
                {
                    if (target.invincible == false)
                        target.DepleteHp(transform, Random.Range(damageMin, damageMax));
                }
            }
        }

        protected override void OnDrawGizmosSelected()
        {
            base.OnDrawGizmosSelected();

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, _targetDetectRange);

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _attackRange);
        }
    }
}