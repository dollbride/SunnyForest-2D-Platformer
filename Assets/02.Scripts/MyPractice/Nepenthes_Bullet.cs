using UnityEngine;
using TMPro;

namespace Platformer.Effects
{
    public class Nepenthes_Bullet : MonoBehaviour
    {
        private Vector3 _move = new Vector3(0.3f, 0.0f, 0.0f);

        //public void Show(float amount)
        //{
        //    _amount.text = ((int)amount).ToString();
        //    _color.a = 1.0f;
        //    _amount.color = _color; // ������� ���� ���� 0���� ����ó�� �ߴ� �� �ٽ� ����
        //}

        private void Awake()
        {

        }
        private void Update()
        {
            transform.position += _move * Time.deltaTime;

        }
    }
}

