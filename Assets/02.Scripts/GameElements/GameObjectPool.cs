using UnityEngine;
using UnityEngine.Pool;

namespace Platformer.GameElements
{
    public class GameObjectPool : MonoBehaviour
    {
        public class PooledItem : MonoBehaviour
        {
            public IObjectPool<GameObject> pool;
            private GameObject _gameObject;

            private void Awake()
            {
                _gameObject = GetComponent<GameObject>();
            }

            public void ReturnToPool()
            {
                pool.Release(_gameObject);
            }
        }

        public enum PoolType
        {
            Stack,
            LinkedList
        }
        [SerializeField] private PoolType _collectionType;
        [SerializeField] private bool _collectionCheck;

        // ObjectPool 클래스는 여러 대리자를 갖고 있다.
        // F12로 이동해보면 파라미터로 등록하는 각 함수가 어떤 대리자에 등록되는지 알 수 있다.

        public IObjectPool<GameObject> pool
        {
            get
            {
                if (_pool == null)
                {
                    if (_collectionType == PoolType.Stack)
                        _pool = new ObjectPool<GameObject>(CreatedPooledItem,
                                                            OnGetFromPool,
                                                            OnReturnToPool,
                                                            OnDestroyPooledItem,
                                                            _collectionCheck,
                                                            _count,
                                                            _countMax);
                    else
                        _pool = new LinkedPool<GameObject>(CreatedPooledItem,
                                                            OnGetFromPool,
                                                            OnReturnToPool,
                                                            OnDestroyPooledItem,
                                                            _collectionCheck,
                                                            _countMax);
                }
                return _pool;
            }
        }
        private IObjectPool<GameObject> _pool;
        [SerializeField] private GameObject _prefab;
        [SerializeField] private int _count;
        [SerializeField] private int _countMax;

        private GameObject CreatedPooledItem()
        {
            GameObject item = Instantiate(_prefab);
            item.AddComponent<PooledItem>().pool = pool;
            return item;
        }

        private void OnGetFromPool(GameObject gameObject)
        {
            gameObject.SetActive(true);
        }

        private void OnReturnToPool(GameObject gameObject)
        {
            gameObject.SetActive(false);
        }

        private void OnDestroyPooledItem(GameObject gameObject)
        {
            // OnDestroyPooledItem() 자체는 실제로 파괴하는 것은 아님. 풀에서 쫓아내는 거. 참조 값만 지우는 것.
            // 그래서 Destroy() 함수를 따로 추가했다.
            Destroy(gameObject);
        }


    }

}
