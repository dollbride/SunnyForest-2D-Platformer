using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace Platformer.GameElements
{
    public class PoolManager<T>
        where T : MonoBehaviour
    {
        public static PoolManager<T> instance 
        {
            get 
            { 
                // 외부에서 누군가가 instance를 처음으로 요구할 때 만들어줌
                if (_instance == null)
                    _instance = new PoolManager<T>();
                return _instance;
            }
        }
        private static PoolManager<T> _instance;

        private Dictionary<PoolTag, IObjectPool<T>> _pools = new Dictionary<PoolTag, IObjectPool<T>>();

        public IObjectPool<K> GetPool<K>(PoolTag tag)
            where K : MonoBehaviour
        {
            return (IObjectPool<K>)_pools[tag];
        }
        // 이걸 컴파일 하면 아래 코드가 생겨난다:
        //public IObjectPool<EnemyController> Get(PoolTag tag)
        //{
        //    return (IObjectPool<EnemyController>)_pools[tag];
        //}

        public K Get<K>(PoolTag tag)
            where K : MonoBehaviour
        {
            return GetPool<K>(tag).Get();
        }

        public void Register(PoolTag tag, IObjectPool<T> pool)
        {
            _pools.Add(tag, pool);
        }

    }

}
