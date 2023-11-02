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
        //    _amount.color = _color; // 사라지기 직전 알파 0으로 투명처리 했던 것 다시 원복
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

