using Platformer.Controllers;
using Platformer.GameElements.Pool;
using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

namespace Platformer.GameElements
{
    public class EnemySpawner : MonoBehaviour
    {
        private const int TRY_COUNT_MAX = 20;
        [SerializeField] private PoolTag _poolTag;
        [SerializeField] private Vector2 _size;
        [SerializeField] private int _countLimit;   // 최대 소환 개체 수
        private int _spawnedCount;     // 현재 소환된 개체 수
        private int _spawningCount;    // 소환 중인(코루틴) 수
        [SerializeField] private float _delay;      // 소환 대기 시간
        [SerializeField] private LayerMask _spawnPointMask;

        private IObjectPool<GameObject> _pool;

        private void Start()
        {
            _pool = PoolManager.instance.GetPool(_poolTag);
            StartCoroutine(SpawnAll());
        }

        private void OnApplicationQuit()
        {
            StopAllCoroutines();
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }

        private IEnumerator SpawnAll()
        {
            while (_spawnedCount + _spawningCount < _countLimit)
            {
                _spawningCount++;
                StartCoroutine(Spawn());
            }
            yield return null;
        }

        private IEnumerator Spawn() // 비동기 수행을 위해 이뉴머레이터로 반환
        {
            yield return new WaitForSeconds(_delay);  // _delay초 만큼 기다린 다음 Move Next 다음 코드 내용.

            int tryCount = TRY_COUNT_MAX;
            while (true)
            {
                Vector2 origin = (Vector2)transform.position
                             + new Vector2(Random.Range(-_size.x / 2.0f, +_size.x / 2.0f),
                                           Random.Range(-_size.y / 2.0f, +_size.y / 2.0f));

                RaycastHit2D hit =
                    Physics2D.Raycast(origin, Vector2.down, origin.y - (transform.position.y - _size.y / 2.0f), _spawnPointMask);

                Debug.DrawLine(origin,
                               origin + Vector2.down * (origin.y - (transform.position.y - _size.y / 2.0f)),
                               hit.collider ? Color.green : Color.red,
                               1.0f);

                if (hit.collider)
                {
                    EnemyController enemy = _pool.Get().GetComponent<EnemyController>();
                    // 소환된 몬스터가 죽어서 풀로 돌아갈 때 스폰 코루틴이 발동되도록 구독시킴.
                    enemy.GetComponent<GameObjectPool.PooledItem>().onReturnToPool = () =>
                    {
                        _spawnedCount--;
                        _spawningCount++;
                        StartCoroutine(Spawn());
                    };
                    enemy.SetUp();
                    enemy.transform.position = hit.point;
                    break;
                }

                tryCount--;
                if (tryCount <= 0)
                {
                    tryCount = TRY_COUNT_MAX;
                    yield return null;
                }
            }
            _spawningCount--;
            _spawnedCount++;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(transform.position, _size);
        }

    }
}
