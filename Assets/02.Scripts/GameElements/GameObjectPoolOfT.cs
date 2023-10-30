using UnityEngine;
using UnityEngine.Pool;

namespace Platformer.GameElements
{
    public enum PoolTag
    {
        None,
        DamagePopUp_Player,
        DamagePopUp_Enemy,
        Enemy_Slug,
    }
    
    public class GameObjectPool<T> : MonoBehaviour
        where T : MonoBehaviour
    {
        new public PoolTag tag;

        public class PooledItem : MonoBehaviour
        {
            public IObjectPool<T> pool;
            private T _item;

            private void Awake()
            {
                _item = GetComponent<T>();
            }

            public void ReturnToPool()
            {
                pool.Release(_item);
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

        public IObjectPool<T> pool
        {
            get
            {
                if (_pool == null)
                {
                    if (_collectionType == PoolType.Stack)
                        _pool = new ObjectPool<T>(CreatedPooledItem,
                                                  OnGetFromPool,
                                                  OnReturnToPool,
                                                  OnDestroyPooledItem,
                                                  _collectionCheck,
                                                  _count,
                                                  _countMax);
                    else
                        _pool = new LinkedPool<T>(CreatedPooledItem,
                                                  OnGetFromPool,
                                                  OnReturnToPool,
                                                  OnDestroyPooledItem,
                                                  _collectionCheck,
                                                  _countMax);
                }
                return _pool;
            }
        }
        private IObjectPool<T> _pool;
        [SerializeField] private T _prefab;
        [SerializeField] private int _count;
        [SerializeField] private int _countMax;

        private void Awake()
        {
            PoolManager<T>.instance.Register(tag, pool);
        }

        protected virtual T CreatedPooledItem()
        {
            T item = Instantiate(_prefab);
            item.gameObject.AddComponent<PooledItem>().pool = pool;
            return item;
        }

        protected virtual void OnGetFromPool(T item)
        {
            item.gameObject.SetActive(true);
        }

        protected virtual void OnReturnToPool(T item)
        {
            item.gameObject.SetActive(false);
        }

        protected virtual void OnDestroyPooledItem(T item)
        {
            // OnDestroyPooledItem() ��ü�� ������ �ı��ϴ� ���� �ƴ�. Ǯ���� �ѾƳ��� ��. ���� ���� ����� ��.
            // �׷��� Destroy() �Լ��� ���� �߰��ߴ�.
            Destroy(item.gameObject);
        }


    }

}
