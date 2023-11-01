using Platformer.Effects;
using Platformer.FSM;
using Platformer.GameElements;
using Platformer.GameElements.Pool;
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
        [SerializeField] private float _slopeAngle = 45.0f;

        private CapsuleCollider2D _trigger;

        public override void SetUp()
        {
            base.SetUp();
            _trigger.enabled = true;
        }

        protected override void Awake()
        {
            base.Awake();
            _trigger = GetComponent<CapsuleCollider2D>();
            _ai = AI.Think;
        }

        protected override void Update()
        {
            UpdateAI();
            base.Update();
        }

        protected override void FixedUpdate()
        {
            //이동하려는 위치가 유효하지 않으면 제자리
            // => (기반 타입의) FixedUpdate에서 Move() 호출 안 하게 만들기
            // => Move() 자체를 앞 땅 감지 조건 안에 넣어버리기 
            if (machine.currentStateID != CharacterStateID.Move)
            {
                base.FixedUpdate(); // Move() 실행
            }
            // 이동 중일 땐 
            else
            {
                machine.FixedUpdateState();
                // 삼각함수를 이용해서 레이캐스트 할 높이 구하기:
                Vector2 expected = rigidbody.position + move * Time.fixedDeltaTime; //예상 위치
                float distanceX = Mathf.Abs(expected.x - rigidbody.position.x); // 밑변
                float height = distanceX * MathF.Tan(_slopeAngle * Mathf.Deg2Rad); // 높이 = 밑변 * 탄젠트세타
                Vector2 origin = expected + Vector2.up * height;
                RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, height * 2.0f, groundMask);
                if (hit.collider)
                {
                    rigidbody.position = hit.point;
                }
            }


        }

        private void UpdateAI()
        {
            if (machine.currentStateID == CharacterStateID.Hurt || 
                machine.currentStateID == CharacterStateID.Die ||
                machine.currentStateID == CharacterStateID.Attack
                ) 
                return;

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
                    {
                        _ai = AI.ExectueRandomBehaviour;
                    }
                    break;
                case AI.ExectueRandomBehaviour:
                    {
                        //Range의 정수 범위는 Max에서 하나 적은 것까지 
                        var nextID = _behaviours[Random.Range(0, _behaviours.Count)];
                        if (machine.ChangeState(nextID))
                        {
                            _behaviourTimer = Random.Range(_behaviourTimeMin, _behaviourTimeMax);
                            _horizontal = Random.Range(-1.0f, 1.0f);
                            _ai = AI.WaitUntilBehaviour;
                        }
                        else
                        {
                            _ai = AI.Think;
                        }
                    }
                    break;
                case AI.WaitUntilBehaviour:
                    {
                        if (_behaviourTimer <= 0)
                        {
                            _ai = AI.Think;
                        }
                        else
                        {
                            _behaviourTimer -= Time.deltaTime;
                        }
                    }
                    break;
                case AI.Follow:
                    {
                        // 타겟 없으면 다시생각해
                        if (_target == null)
                        {
                            _ai = AI.Think;
                            return;
                        }

                        if (Vector2.Distance(transform.position, _target.position) > _targetDetectRange)
                        {
                            _target = null;
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

            // 데미지팝업 pool들을 관리하는 매니저를 통해서
            // 에너미용 데미지팝업 풀을 가져오고, 가져온 풀에서 Item을 가져온다.
            DamagePopUp damagePopUp = PoolManager.instance
                                     .Get<DamagePopUp>(PoolTag.DamagePopUp_Enemy);
            damagePopUp.transform.position = transform.position + Vector3.up * 0.5f;
            damagePopUp.Show(amount);
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

            Gizmos.color = Color.cyan;
            Gizmos.DrawWireCube((transform.position + new Vector3(0.15f * direction, 0.0f, 0.0f)), new Vector3(0.04f, 0.02f, 0.0f));

        }
    }
}