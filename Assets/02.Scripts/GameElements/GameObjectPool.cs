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

        // ObjectPool Ŭ������ ���� �븮�ڸ� ���� �ִ�.
        // F12�� �̵��غ��� �Ķ���ͷ� ����ϴ� �� �Լ��� � �븮�ڿ� ��ϵǴ��� �� �� �ִ�.

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
            // OnDestroyPooledItem() ��ü�� ������ �ı��ϴ� ���� �ƴ�. Ǯ���� �ѾƳ��� ��. ���� ���� ����� ��.
            // �׷��� Destroy() �Լ��� ���� �߰��ߴ�.
            Destroy(gameObject);
        }


    }

}
