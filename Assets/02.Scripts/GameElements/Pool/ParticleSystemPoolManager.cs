using Platformer.GameElements.Pool;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace Platformer.Effects
{
    public class ParticleSystemPoolManager
    {
        public static ParticleSystemPoolManager instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ParticleSystemPoolManager();
                }
                return _instance;
            }
        }
        private static ParticleSystemPoolManager _instance;

        private Dictionary<PoolTag, IObjectPool<ParticleSystem>> _pools = new Dictionary<PoolTag, IObjectPool<ParticleSystem>>();

        public void Register(PoolTag tag, IObjectPool<ParticleSystem> pool)
        {
            _pools.Add(tag, pool);
        }

        // 딕셔너리에 키(태그)를 입력해서 밸류로 I오브젝트풀을 받아오는 함수
        public IObjectPool<ParticleSystem> GetPool(PoolTag tag)
        {
            return _pools[tag];
        }

        // 위의 GetPool() 함수로 받아온 I오브젝트 pool에서 아이템을 Get() 꺼내 쓰는 편의 함수
        public ParticleSystem Get(PoolTag tag)
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

