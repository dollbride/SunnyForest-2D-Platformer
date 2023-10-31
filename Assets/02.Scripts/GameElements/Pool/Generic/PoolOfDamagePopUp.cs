using Platformer.Controllers;
using Platformer.Effetcs;
using UnityEngine;
using UnityEngine.Pool;

namespace Platformer.GameElements.Pool.Generic
{
    public class PoolOfDamagePopUp : GameObjectPool<DamagePopUp>
    {
        public class PooledItem : MonoBehaviour
        {
            public IObjectPool<DamagePopUp> pool;
            private DamagePopUp _item;

            private void Awake()
            {
                _item = GetComponent<DamagePopUp>();
            }

            // OnDisable() : 모노비헤이비어가 비활성화 될 때 호출되는 함수
            private void OnDisable()
            {
                ReturnToPool();
            }

            public void ReturnToPool()
            {
                pool.Release(_item);
                Debug.Log($"Returned to pool");
            }
        }

        private void Awake()
        {
            PoolManagerOfDamagePopUp.instance.Register(tag, pool);
        }

    }
}
