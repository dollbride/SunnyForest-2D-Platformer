using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace Platformer.GameElements.Pool
{
    public class PoolManager
    {
        public static PoolManager instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new PoolManager();
                }
                return _instance;
            }
        }
        private static PoolManager _instance;

        private Dictionary<PoolTag, IObjectPool<GameObject>> _pools = new Dictionary<PoolTag, IObjectPool<GameObject>>();

        public void Register(PoolTag tag, IObjectPool<GameObject> pool)
        {
            _pools.Add(tag, pool);
        }

        // 딕셔너리에 키(태그)를 입력해서 밸류로 I오브젝트풀을 받아오는 함수
        public IObjectPool<GameObject> GetPool(PoolTag tag)
        {
            return _pools[tag];
        }

        // 위의 GetPool() 함수로 받아온 I오브젝트 pool에서 아이템을 Get() 꺼내 쓰는 편의 함수
        public GameObject Get(PoolTag tag)
        {
            return _pools[tag].Get();
        }

        // 겟컴포넌트 하기 편하라고 만든 편의 함수
        public T Get<T>(PoolTag tag)
        {
            return _pools[tag].Get().GetComponent<T>();
        }


    }
}
