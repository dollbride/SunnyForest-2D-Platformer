using UnityEngine;
using TMPro;

namespace Platformer.Effetcs
{
    public class DamagePopUp : MonoBehaviour
    {
        private TMP_Text _amount;
        private float _fadeSpeed = 0.8f;
        private Vector3 _move = new Vector3(0.0f, 0.3f, 0.0f);
        private Color _color;

        public void Show(float amount)
        {
            _amount.text = ((int)amount).ToString();
            _color.a = 1.0f;
            _amount.color = _color; // 사라지기 직전 알파 0으로 투명처리 했던 것 다시 원복
        }

        private void Awake()
        {
            _amount = GetComponent<TMP_Text>();
            _color = _amount.color;
        }
        private void Update()
        {
            transform.position += _move * Time.deltaTime;
            _color.a -= _fadeSpeed * Time.deltaTime; // _color는 Color 벡터4 구조체라서 수정 가능
            _amount.color = _color;                 // TMP의 멤버로 있는 color는 구조체여도 수정 불가 

            if (_color.a < 0.0f)
                //Destroy(gameObject);
                gameObject.SetActive(false);


        }
    }
}

