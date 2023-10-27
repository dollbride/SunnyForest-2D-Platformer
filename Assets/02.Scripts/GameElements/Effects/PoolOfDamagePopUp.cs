using UnityEngine;
using UnityEngine.Pool;

namespace Platformer.Effetcs
{
    public class PoolOfDamagePopUp : MonoBehaviour
    {
        // 컴포넌트 추가 창에서는 모노비헤이비어가 붙어 있고,
        // cs파일 이름과 같은 이름의 클래스만 검색해서 붙여넣을 수 있다.
        // PooledItem은 cs파일 명과 달라서 유니티에서 컴포넌트로는 사용 못함.
        // inner 클래스(클래스 내부에 멤버처럼 존재함)
        public class PooledItem : MonoBehaviour
        {
            public IObjectPool<DamagePopUp> pool;
            private DamagePopUp _damagePopUp;

            private void Awake()
            {
                _damagePopUp = GetComponent<DamagePopUp>();
            }

            public void ReturnToPool()
            {
                pool.Release(_damagePopUp);
            }
        }

        public enum PoolType
        {
            Stack,
            LinkedList
        }
        [SerializeField] private PoolType _collectionType;
        [SerializeField] private bool _collectionCheck;

        public IObjectPool<DamagePopUp> pool
        {
            get
            {
                if (_pool == null)
                {
                    if (_collectionType == PoolType.Stack)
                        _pool = new ObjectPool<DamagePopUp>(CreatedPooledItem,
                                                            OnGetFromPool,
                                                            OnReturnToPool,
                                                            OnDestroyPooledItem,
                                                            _collectionCheck,
                                                            _count,
                                                            _countMax);
                    else
                        _pool = new LinkedPool<DamagePopUp>(CreatedPooledItem,
                                                            OnGetFromPool,
                                                            OnReturnToPool,
                                                            OnDestroyPooledItem,
                                                            _collectionCheck,
                                                            _countMax);
                }
                return _pool;
            }
        }
        private IObjectPool<DamagePopUp> _pool;
        [SerializeField] private DamagePopUp _prefab;
        [SerializeField] private int _count;
        [SerializeField] private int _countMax;

        private DamagePopUp CreatedPooledItem()
        {
            DamagePopUp item = Instantiate(_prefab);
            item.gameObject.AddComponent<PooledItem>().pool = pool;
            return item;
        }

        private void OnGetFromPool(DamagePopUp damagePopUp)
        {
            damagePopUp.gameObject.SetActive(true);
        }

        private void OnReturnToPool(DamagePopUp damagePopUp)
        {
            damagePopUp.gameObject.SetActive(false);
        }

        private void OnDestroyPooledItem(DamagePopUp damagePopUp)
        {
            Destroy(damagePopUp.gameObject);
        }


    }

}
