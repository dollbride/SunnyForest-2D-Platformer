using Platformer.Effects;
using Platformer.GameElements.Pool;
using Unity.Burst.CompilerServices;
using UnityEditor.U2D.Aseprite;
using UnityEngine;

namespace Platformer.GameElements
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private PoolTag _effectPoolTag;
        [HideInInspector] public Transform owner;
        [HideInInspector] public Vector3 velocity;
        [HideInInspector] public LayerMask targetMask;
        private LayerMask _boundMask;

        private void Awake()
        {
            _boundMask = 1 << LayerMask.NameToLayer("Wall") | 1 << LayerMask.NameToLayer("Ground");
        }

        private void FixedUpdate()
        {
            // 다음 프레임 예상 위치
            Vector3 expected = transform.position + velocity * Time.fixedDeltaTime;
            // 검출해야 하는 모든 마스크에 대해 이동 예상 위치 쪽으로 레이캐스트
            RaycastHit2D hit
                       = Physics2D.Raycast(transform.position, expected - transform.position,
                         Vector3.Distance(transform.position, expected),
                         _boundMask | targetMask);

            if (hit.collider) // 뭔가 검출했다면
            {
                int layerFlag = 1 << hit.collider.gameObject.layer; // 맵 경계에 부딪힌 건지
                if ((layerFlag & _boundMask) > 0)
                    OnHitBound(hit);
                else if ((layerFlag & targetMask) > 0)  // 타겟과 부딪힌 건지
                    OnHitTarget(hit);
            }

            transform.position = expected; // 예상 위치로 이동
        }

        protected virtual void OnHitBound(RaycastHit2D hit)
        {
            gameObject.SetActive(false);
            ExplosionEffect(hit);

        }

        protected virtual void OnHitTarget(RaycastHit2D hit)
        {
            gameObject.SetActive(false);
            ExplosionEffect(hit);

        }

        private void ExplosionEffect(RaycastHit2D hit)
        {
            ParticleSystem ps = ParticleSystemPoolManager.instance.Get<ParticleSystem>(_effectPoolTag);
            ps.transform.position = hit.point;
            // 충돌체 평면의 노말 벡터(수직 방향)를 구하고
            // 이걸 축으로 해서 날아온 방향을 반전하면
            // 튕겨나가는 방향을 정할 수 있다.
            float theta = Mathf.Acos(Vector2.Dot(-velocity.normalized, hit.normal)) * Mathf.Rad2Deg;
            Vector2 look = Quaternion.Euler(0.0f, 0.0f, theta) * hit.normal;
            ps.transform.LookAt(hit.point + look);
        }


    }
}

