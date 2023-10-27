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

            // is Ű����
            // ��ü�� � Ÿ������ ������ �� �ִ��� Ȯ���ϰ� bool ����� ��ȯ�ϴ� Ű����
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
//�Ʒ� Refresh() �Լ��� ���ٽ����� �� ��
//private void Refresh(float Value)
//{
//    _hpBar.value = Value;
//}