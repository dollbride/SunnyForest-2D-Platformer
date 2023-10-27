using Platformer.Stats;
using UnityEngine;
using UnityEngine.UI;
using CharacterController = Platformer.Controllers.CharacterController;

namespace Platformer.UI
{
    public class MiniStatusUi : MonoBehaviour
    {
        [SerializeField] private Slider _hpBar;

        private void Start()
        {
            IHp hp = transform.root.GetComponent<IHp>();
            _hpBar.minValue = hp.hpMin;
            _hpBar.maxValue = hp.hpMax;
            _hpBar.value = hp.hpValue;

            hp.onHpChanged += (value) => _hpBar.value = value;

            // is 키워드
            // 객체가 어떤 타입으로 참조할 수 있는지 확인하고 bool 결과를 반환하는 키워드
            if (hp is CharacterController)
            {
                Vector3 originalScale = transform.localScale;
                ((CharacterController)hp).onDirectionChanged += (value) =>
                {
                    transform.localScale = value < 0 ?
                        new Vector3(-originalScale.x, originalScale.y, originalScale.z) :
                        new Vector3(+originalScale.x, originalScale.y, originalScale.z);
                };

            }

        }
    }
}



//hp.onHpChanged += Refresh;
//아래 Refresh() 함수를 람다식으로 쓴 것
//private void Refresh(float Value)
//{
//    _hpBar.value = Value;
//}