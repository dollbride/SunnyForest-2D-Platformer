using Platformer.Effetcs;
using UnityEngine;
using UnityEngine.Pool;

namespace Platformer.Controllers
{
    public class PoolOfSlug : MonoBehaviour
    {
        public class PooledItem : MonoBehaviour
        {
            public IObjectPool<SlugController> pool;
            private SlugController _slugController;

            private void Awake()
            {
                _slugController = GetComponent<SlugController>();
            }

            public void ReturnToPool()
            {
                pool.Release(_slugController);
            }
        }

        public enum PoolType
        {
            Stack,
            LinkedList
        }
        [SerializeField] private PoolType _collectionType;
        [SerializeField] private bool _collectionCheck;

        private IObjectPool<SlugController> _pool;
        [SerializeField] private SlugController _prefab;
        [SerializeField] private string prefabName;
        [SerializeField] private int _count;
        [SerializeField] private int _countMax;

        private Vector3[] spawnPoints;   // 스폰 위치 관련 설정들
        [SerializeField] private Vector3 spawnAreaBoxCenter;
        [SerializeField] private Vector3 spawnAreaBoxSize;
        [SerializeField] private LayerMask groundMask;

        int instanceNumber = 1;

        void Start()
        {
            spawnPoints = new Vector3[_countMax];
            SpawnEntities();
        }

        void SpawnEntities()
        {
            for (int i = 0; i < _count; i++)
            {
                // 랜덤 스폰 위치 추가
                spawnPoints[i] = new Vector3(Random.Range(spawnAreaBoxCenter.x - spawnAreaBoxSize.x / 2, spawnAreaBoxCenter.x + spawnAreaBoxSize.x / 2), Random.Range(spawnAreaBoxCenter.y - spawnAreaBoxSize.y / 2, spawnAreaBoxCenter.y + spawnAreaBoxSize.y / 2), 0f);

                // Raycast로 땅 위치 감지
                Vector3 castStartPos = spawnPoints[i];
                RaycastHit2D downHit = Physics2D.Raycast(castStartPos, Vector2.down, 1.5f, groundMask);
                if (downHit)
                {
                    SlugController item = CreatedPooledItem();
                    item.transform.position = downHit.point;
                    item.name = prefabName + instanceNumber;
                }
                instanceNumber++;
            }
        }
    
        public IObjectPool<SlugController> pool
        {
            get
            {
                if (_pool == null)
                {
                    if (_collectionType == PoolType.Stack)
                        _pool = new ObjectPool<SlugController>(CreatedPooledItem,
                                                            OnGetFromPool,
                                                            OnReturnToPool,
                                                            OnDestroyPooledItem,
                                                            _collectionCheck,
                                                            _count,
                                                            _countMax);
                    else
                        _pool = new LinkedPool<SlugController>(CreatedPooledItem,
                                                            OnGetFromPool,
                                                            OnReturnToPool,
                                                            OnDestroyPooledItem,
                                                            _collectionCheck,
                                                            _countMax);
                }
                return _pool;
            }
        }
        

        private SlugController CreatedPooledItem()
        {
            SlugController item = Instantiate(_prefab);
            item.gameObject.AddComponent<PooledItem>().pool = pool;
            return item;
        }

        private void OnGetFromPool(SlugController slugController)
        {
            slugController.gameObject.SetActive(true);
        }

        private void OnReturnToPool(SlugController slugController)
        {
            slugController.gameObject.SetActive(false);
        }

        private void OnDestroyPooledItem(SlugController slugController)
        {
            Destroy(slugController.gameObject);
        }


    }
}
